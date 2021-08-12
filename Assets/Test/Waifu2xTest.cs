using UnityEngine;
using UnityEngine.UI;

namespace Waifu2x {

sealed public class Waifu2xTest : MonoBehaviour
{
    [SerializeField] Texture2D _source = null;
    [SerializeField] ResourceSet _resources = null;
    [SerializeField] RawImage _uiOriginal = null;
    [SerializeField] RawImage _uiScaled = null;

    void Start()
    {
        _uiOriginal.texture = _source;
        using (var scaler = new Waifu2xScaler(_resources))
            _uiScaled.texture = scaler.CreateScaledTexture(_source);
    }
}

} // namespace Waifu2x
