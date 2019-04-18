using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.XR.WSA.WebCam;
using UnityEngine.Windows.Speech;
using HoloToolkit.Unity.InputModule;
using System.Collections.Generic;
using UnityEngine.UI;
// using System.Threading;

public class PhotoRecorder : MonoBehaviour
{
    // needed for keywords voice recognition
    private KeywordRecognizer keywordRecognizer = null;
    private Dictionary<string, System.Action> keywords = new Dictionary<string, System.Action>();

    PhotoCapture photoCaptureObject = null;

    static readonly int TotalImagesToCapture = 1;
    int capturedImageCount = 0;

    // for keywords
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
    void Start()
    {
        GameObject.Find("Photo").transform.localScale = new Vector3(0, 0, 0); // hide the icon
        Resolution cameraResolution = PhotoCapture.SupportedResolutions.OrderByDescending((res) => res.width * res.height).First();
        Texture2D targetTexture = new Texture2D(cameraResolution.width, cameraResolution.height);

        keywords.Add("take photo", () =>
        {
            GameObject.Find("Photo").transform.localScale = new Vector3(0.2f, 0.2f, 0.2f); // show the icon
            Debug.Log("started photo capture");

            PhotoCapture.CreateAsync(false, delegate (PhotoCapture captureObject) {
                Debug.Log("Created PhotoCapture Object");
                photoCaptureObject = captureObject;

                CameraParameters c = new CameraParameters();
                c.hologramOpacity = 0.0f;
                c.cameraResolutionWidth = targetTexture.width;
                c.cameraResolutionHeight = targetTexture.height;
                c.pixelFormat = CapturePixelFormat.BGRA32;

                captureObject.StartPhotoModeAsync(c, delegate (PhotoCapture.PhotoCaptureResult result) {
                    Debug.Log("Started Photo Capture Mode");
                    TakePicture();
                    // Thread.Sleep(1000);
                    // TODO uncomment this GameObject.Find("Photo").transform.localScale = new Vector3(0, 0, 0); // hide the icon
                });
            });
        });
        keywordRecognizer = new KeywordRecognizer(keywords.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += KeywordRecognizer_OnPhraseRecognized;
        keywordRecognizer.Start();
    }

    void OnCapturedPhotoToDisk(PhotoCapture.PhotoCaptureResult result)
    {
        Debug.Log("Saved Picture To Disk!");

        if (capturedImageCount < TotalImagesToCapture)
        {
            TakePicture();
        }
        else
        {
            photoCaptureObject.StopPhotoModeAsync(OnStoppedPhotoMode);
        }
    }

    public GameObject test;

    void TakePicture()
    {
        capturedImageCount++;
        Debug.Log(string.Format("Taking Picture ({0}/{1})...", capturedImageCount, TotalImagesToCapture));
        string filename = string.Format(@"CapturedImage{0}.jpg", capturedImageCount);
        string filePath = System.IO.Path.Combine(Application.persistentDataPath, filename);
        Debug.Log(filePath);
        photoCaptureObject.TakePhotoAsync(filePath, PhotoCaptureFileOutputFormat.JPG, OnCapturedPhotoToDisk);

        Text file = test.GetComponent<Text>();
        file.text = filePath;
    }

    void OnStoppedPhotoMode(PhotoCapture.PhotoCaptureResult result)
    {
        photoCaptureObject.Dispose();
        photoCaptureObject = null;

        Debug.Log("Captured images have been saved at the following path.");
        Debug.Log(Application.persistentDataPath);
    }
}