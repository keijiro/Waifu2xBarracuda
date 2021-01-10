using UnityEngine;
using Unity.Barracuda;

namespace Waifu2x {

sealed public class Waifu2xTest : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] Unity.Barracuda.NNModel _model = null;
    [SerializeField] ComputeShader _preprocessor = null;
    [SerializeField] Texture2D _testImage = null;

    #endregion

    #region Internal objects

    ComputeBuffer _preprocessed;
    RenderTexture _postprocessed;
    IWorker _worker;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _preprocessed = new ComputeBuffer(156 * 156 * 3, sizeof(float));
        _worker = ModelLoader.Load(_model).CreateWorker();

        // Preprocessing for Waifu2x
        _preprocessor.SetTexture(0, "_Texture", _testImage);
        _preprocessor.SetBuffer(0, "_Tensor", _preprocessed);
        _preprocessor.Dispatch(0, 39, 39, 1);

        using (var tensor = new Tensor(1, 156, 156, 3, _preprocessed))
            _worker.Execute(tensor);

        var output = _worker.PeekOutput();
        _postprocessed = output.ToRenderTexture();


        GetComponent<Renderer>().material.mainTexture = _postprocessed;
    }

    void OnDisable()
    {
        _preprocessed?.Dispose();
        _preprocessed = null;

        _worker?.Dispose();
        _worker = null;
    }

    void OnDestroy()
    {
        if (_postprocessed != null) Destroy(_postprocessed);
    }

    #endregion
}

} // namespace NNCam
