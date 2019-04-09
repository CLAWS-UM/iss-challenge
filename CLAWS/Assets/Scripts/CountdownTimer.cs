using System.Collections;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public GameObject timerObj;
    public Text timerText;
    //float currentTime;
    //float startTime = 0f;
    public Stopwatch timer;
    public TimeSpan ts;

    //[SerializeField] Text timer;

    void Start()
    {
        timer = new Stopwatch();
        timer.Start();

        // Get the elapsed time as a TimeSpan value.
        ts = new TimeSpan();
        ts = timer.Elapsed;

        // Format and display the TimeSpan value.
        timerObj.GetComponent<Text>().text = ts.ToString(); // string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);


        //currentTime = startTime;

        //timerText.text = currentTime.ToString("00:00:00");
        //timer.text = currentTime.ToString("0.00");

    }

    void Update()
    {
        // Get the elapsed time as a TimeSpan value.
        ts = timer.Elapsed;

        // Format and display the TimeSpan value.
        timerObj.GetComponent<Text>().text = ts.ToString(); // ONLY IN .NET 4 :( string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);

        //currentTime += Time.deltaTime;
        //timer.text = currentTime.ToString("00:00:00");

        //timer.text = currentTime.ToString("0.00");
    }

    /*void OnGUI()
    {
        float hours = Mathf.Floor(currentTime / 3600);
        float minutes = Mathf.Floor((currentTime % 3600) / 60);
        float seconds = currentTime % 60;

        GUIStyle clockStyle = new GUIStyle();
        clockStyle.fontSize = 20;
        clockStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(865, 25, 250, 100), hours + ":" + minutes + ":" + Mathf.RoundToInt(seconds), clockStyle);
    }*/
}
