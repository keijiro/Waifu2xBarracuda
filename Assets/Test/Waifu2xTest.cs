using UnityEngine;
using UnityEngine.UI;

namespace Waifu2x {

sealed public class Waifu2xTest : MonoBehaviour
{
    #region Editable attributes

    [SerializeField] Unity.Barracuda.NNModel _model = null;
    [SerializeField] ComputeShader _compute = null;
    [Space]
    [SerializeField] Texture2D _source = null;
    [Space]
    [SerializeField] RawImage _uiOriginal = null;
    [SerializeField] RawImage _uiScaled = null;

    #endregion

    #region MonoBehaviour implementation

    void Start()
    {
        _uiOriginal.texture = _source;

        using (var scaler = new Waifu2xScaler(_model, _compute))
            _uiScaled.texture = scaler.CreateScaledTexture(_source);
    }

    #endregion
}

} // namespace Waifu2x
