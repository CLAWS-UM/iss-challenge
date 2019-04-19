using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using ZXing;
using UnityEngine.UI;

namespace QRReader
{
    /// <summary>
    /// QRコードを読み込む
    /// </summary>
    public class QRRead
    {
        private static WebCamTexture webCam = null;

        private static bool isRunning = false;

        /// <summary>
        /// QRコード認識用コルーチン
        /// </summary>
        /// <param name="action"></param>
        /// <param name="loop"></param>
        /// <returns></returns>
        public static IEnumerator QRReaderCoroutine(Action<string> action,bool loop=false)
        {
#if WINDOWS_UWP
            // HoloLens用
            if (webCam == null) webCam = new WebCamTexture(896, 504, 5);//1280 720
#else
            if (webCam == null) webCam = new WebCamTexture(WebCamTexture.devices[1].name);
#endif
            webCam.Play();
            yield return new WaitForSeconds(1.0f);
            var reader = new BarcodeReader();
            var bytes = new byte[webCam.width * webCam.height * 4];
            isRunning = true;
            while (isRunning)
            {
                // WebCamTextureからbyte配列を取得
                var dataColor = webCam.GetPixels32();
                for (var i = 0; i < dataColor.Length; i++)
                {
                    bytes[i * 4 + 0] = dataColor[i].b;
                    bytes[i * 4 + 1] = dataColor[i].g;
                    bytes[i * 4 + 2] = dataColor[i].r;
                    bytes[i * 4 + 3] = dataColor[i].a;
                }
#if WINDOWS_UWP
                var type = BitmapFormat.BGR32;
#else
                var type = RGBLuminanceSource.BitmapFormat.BGR32;
#endif
                // ZXingでQRデータの読み込み
                var res = reader.Decode(bytes, webCam.width, webCam.height, type);
                if (res != null)
                {
                    if (loop == false) StopWebCam();
                    // 結果は文字列で出力
                    if (action != null) action.Invoke(res.Text);
                }
                else
                {
                    if (action != null) action.Invoke("HAI");
                }

                yield return new WaitForSeconds(0.2f);
            }
        }

        /// <summary>
        /// WebCamを停止
        /// </summary>
        public static void StopWebCam()
        {
            isRunning = false;
            webCam.Stop();
        }
    }
}
