using UnityEngine;
using Unity.Barracuda;

namespace Waifu2x {

sealed public class Waifu2xScaler : System.IDisposable
{
    const int CropSize = 128;
    const int Padding = 14;
    const int InputSize = CropSize + Padding * 2;

    IWorker _worker;
    ComputeShader _compute;
    ComputeBuffer _preprocessed;

    public Waifu2xScaler(NNModel model, ComputeShader compute)
    {
        _worker = ModelLoader.Load(model).CreateWorker();
        _compute = compute;
        _preprocessed = new ComputeBuffer(InputSize * InputSize * 3, sizeof(float));
    }

    public void Dispose()
    {
        _worker?.Dispose();
        _worker = null;

        _preprocessed?.Dispose();
        _preprocessed = null;
    }

    public RenderTexture CreateScaledTexture(Texture2D source)
    {
        _compute.SetTexture(0, "_Texture", source);
        _compute.SetBuffer(0, "_Tensor", _preprocessed);
        _compute.Dispatch(0, 1, InputSize, 1);

        using (var tensor = new Tensor(1, InputSize, InputSize, 3, _preprocessed))
            _worker.Execute(tensor);

        return _worker.PeekOutput().ToRenderTexture();
    }
}

} // namespace NNCam
