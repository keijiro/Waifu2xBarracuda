using UnityEngine;
using Unity.Barracuda;

namespace Waifu2x {

sealed public class Waifu2xScaler : System.IDisposable
{
    #region Public methods

    public Waifu2xScaler(NNModel model, ComputeShader compute)
    {
        _worker = ModelLoader.Load(model).CreateWorker();
        _compute = compute;
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
        _compute.SetTexture(0, "_SourceTexture", source);
        _compute.SetBuffer(0, "_InputTensor", inTensor);
        _compute.SetTexture(1, "_OutputTensor", outTensor);
        _compute.SetTexture(1, "_OutputTexture", output);

        // Invoke Waifu2x per tile
        for (var y = 0; y < source.height; y += TileSize)
        {
            for (var x = 0; x < source.width; x += TileSize)
            {
                // Preprocessing
                _compute.SetInts("_InputOffset", x, y);
                _compute.Dispatch(0, 1, inTensorSize, 1);

                // Waifu2x
                using (var tensor = new Tensor(1, inTensorSize, inTensorSize, 3, inTensor))
                    _worker.Execute(tensor);

                // Postprocessing
                _worker.PeekOutput().ToRenderTexture(outTensor);
                _compute.SetInts("_OutputOffset", x * 2, y * 2);
                _compute.Dispatch(1, 1, TileSize * 2, 1);
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

    IWorker _worker;
    ComputeShader _compute;

    #endregion
}

} // namespace Waifu2x
