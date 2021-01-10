using UnityEngine;

namespace Waifu2x {

sealed public class Waifu2xTest : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] Unity.Barracuda.NNModel _model = null;
    [SerializeField] ComputeShader _compute = null;
    [SerializeField] Texture2D _source = null;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        using (var scaler = new Waifu2xScaler(_model, _compute))
          GetComponent<Renderer>().material.mainTexture =
            scaler.CreateScaledTexture(_source);
    }

    #endregion
}

} // namespace Waifu2x
