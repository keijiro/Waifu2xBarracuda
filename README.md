Waifu2xBarracuda
================

**Waifu2xBarracuda** is a [Waifu2x] implementation with the Unity [Barracuda]
NN inference library.

Comparisons
-----------

- Left: Bilinear filter only
- Right: 200% upscaling with Waifu2x + Bilinear filter

[Waifu2x]: https://github.com/nagadomi/waifu2x
[Barracuda]: https://docs.unity3d.com/Packages/com.unity.barracuda@latest

![screenshot](https://i.imgur.com/Fo7B9aG.png)

![screenshot](https://i.imgur.com/DlCMLzu.png)

![screenshot](https://i.imgur.com/cp0k45a.png)

![screenshot](https://i.imgur.com/6sewded.png)

How To Use
----------

- Create a `Waifu2x.Waifu2xScaler` object with specifying a Waifu2x ONNX model
  and the pre/post-processing compute shader.
- Call `CreateScaledTexture` with a source texture. It returns a `RenderTexture`
  object with 200% size.
- Dispose the `Waifu2x.Waifu2xScaler` object when it's no longer needed.

```csharp
using (var scaler = new Waifu2xScaler(model, compute))
{
    texture_foo_2x = scaler.CreateScaledTexture(texture_foo);
    texture_bar_2x = scaler.CreateScaledTexture(texture_bar);
    ...
}
```

Acknowledgement
---------------

### ONNX files

The ONNX files contained in the `Assets/Waifu2x/ONNX` are converted by
[tcyrus]. Please check the [waifu2x-onnx] repository for further details.

[tcyrus]: https://github.com/tcyrus
[waifu2x-onnx]: https://github.com/tcyrus/waifu2x-onnx

### Test assets

This repository contains the following image materials:

- Pepper & Carrot by David Revoy (CC BY 4.0)

  https://www.peppercarrot.com/en/static6/sources&page=sketchbook

- 【物語×放置ゲームコレクション】ゴスロリ吸血鬼 from Niconi Commons

  https://commons.nicovideo.jp/material/nc160359

- ジュエルセイバーFREE

  http://www.jewel-s.jp/
