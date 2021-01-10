using UnityEngine;

namespace Waifu2x {

static class ComputeShaderExtensions
{
    static int[] i2 = new int[2];

    public static void SetInts
      (this ComputeShader cs, string name, int x, int y)
    {
        i2[0] = x; i2[1] = y;
        cs.SetInts(name, i2);
    }
}

} // namespace Waifu2x
