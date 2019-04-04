using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;

public class QRDisplay : MonoBehaviour {

    private WebCamTexture webCam = null;
    private BarcodeReader reader = null;

    // Use this for initialization
    void Start () {
#if WINDOWS_UWP
        webCam = new WebCamTexture(896, 504, 5);//1280 720
#else
        webCam = new WebCamTexture(WebCamTexture.devices[1].name);
#endif
        reader = new BarcodeReader();
        webCam.Play();

    }

    // Update is called once per frame
    void Update()
    {
        var bytes = new byte[webCam.width * webCam.height * 4];
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
        Result res = reader.Decode(bytes, webCam.width, webCam.height, type);

        if (res != null)
        {
            GetComponent<Text>().text = res.Text;
        }
        StartCoroutine(waiter());
    }

    IEnumerator waiter()
    {
        yield return new WaitForSeconds(2);
    }
}


