using UnityEngine;
using Unity.Barracuda;

namespace Waifu2x {

sealed public class Waifu2xScaler : System.IDisposable
{
    #region Public methods

    public Waifu2xScaler(ResourceSet resources)
    {
        _resources = resources;
        _worker = ModelLoader.Load(resources.model).CreateWorker();
    }

    public void Dispose()
    {
        _worker?.Dispose();
        _worker = null;
    }

    public RenderTexture CreateScaledTexture(Texture2D source)
    {
        // Output texture
        var output = new RenderTexture(source.width * 2, source.height * 2, 0);
        output.enableRandomWrite = true;
        output.Create();

        // Input/output tensors
        var inTensorSize = TileSize + Padding * 2;
        var outTensorSize = TileSize * 2 + Padding * 2;

        using var inTensor = new ComputeBuffer(inTensorSize * inTensorSize * 3, sizeof(float));
        var outTensor = RenderTexture.GetTemporary(outTensorSize, outTensorSize, 0);

        // Compute initialization
        var cs = _resources.compute;
        cs.SetTexture(0, "Source", source);
        cs.SetInts("SourceSize", source.width, source.height);
        cs.SetBuffer(0, "NNInput", inTensor);
        cs.SetTexture(1, "NNOutput", outTensor);
        cs.SetTexture(1, "Output", output);

        // Invoke Waifu2x per tile
        for (var y = 0; y < source.height; y += TileSize)
        {
            for (var x = 0; x < source.width; x += TileSize)
            {
                // Preprocessing
                cs.SetInts("Offset", x, y);
                cs.DispatchThreads(0, inTensorSize, inTensorSize, 1);

                // Waifu2x
                using (var tensor = new Tensor(1, inTensorSize, inTensorSize, 3, inTensor))
                    _worker.Execute(tensor);

                // Postprocessing
                _worker.PeekOutput().ToRenderTexture(outTensor);
                cs.SetInts("Offset", x * 2, y * 2);
                cs.DispatchThreads(1, TileSize * 2, TileSize * 2, 1);
            }
        }

        RenderTexture.ReleaseTemporary(outTensor);
        return output;
    }

    #endregion

    #region Predefined constants

    const int TileSize = 128;
    const int Padding = 14;

    #endregion

    #region Internal objects

    ResourceSet _resources;
    IWorker _worker;

    #endregion
}

} // namespace Waifu2x
