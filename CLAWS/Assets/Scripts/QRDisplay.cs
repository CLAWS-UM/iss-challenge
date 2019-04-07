using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using UnityEngine.Windows.Speech;
using System.Linq;

public class QRDisplay : MonoBehaviour {

    private WebCamTexture webCam = null;
    private BarcodeReader reader = null;
    private KeywordRecognizer keywordRecognizer = null;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();
    private bool scanning = false;

    IEnumerator waiter()
    {
        webCam.Pause();
        scanning = false;
        yield return new WaitForSeconds(2);
        webCam.Play();
        scanning = true;
    }

    private void KeywordRecognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        System.Action keywordAction;
        // if the keyword recognized is in our dictionary, call that Action.
        if (keywords.TryGetValue(args.text, out keywordAction))
        {
            keywordAction.Invoke();
        }
    }

    // Use this for initialization
    void Start () {
#if WINDOWS_UWP
        webCam = new WebCamTexture(896, 504, 5);//1280 720
#else
        webCam = new WebCamTexture(WebCamTexture.devices[0].name);
#endif
        reader = new BarcodeReader();

        keywords.Add("scan", () =>
        {
            webCam.Play();
            scanning = true;
        });
        keywords.Add("stop", () =>
        {
            webCam.Stop();
            scanning = false;
        });
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (scanning)
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
    }
}


