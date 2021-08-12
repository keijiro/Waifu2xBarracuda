using UnityEngine;
using Unity.Barracuda;

namespace Waifu2x {

[CreateAssetMenu(fileName = "Waifu2x",
                 menuName = "ScriptableObjects/Waifu2x Resource Set")]
public sealed class ResourceSet : ScriptableObject
{
    public NNModel model;
    public ComputeShader compute;
}

} // namespace Waifu2x
