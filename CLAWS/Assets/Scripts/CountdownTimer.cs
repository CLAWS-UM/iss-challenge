/*  CountdownTimer.cs

    Contains the logic for the EVA countdown timer 
    displayed in the upper-right corner of the interface.

    POCs: Cesar Mu & Riley Schnee
*/

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
    public Stopwatch timer;
    public TimeSpan ts;

    void Start()
    {
        timer = new Stopwatch();
        timer.Start();

        // Get the elapsed time as a TimeSpan value.
        ts = new TimeSpan();
        ts = timer.Elapsed;

        // Format and display the TimeSpan value.
        timerObj.GetComponent<Text>().text = ts.ToString(); 
        // string.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);
    }

    void Update()
    {
        // Get the elapsed time as a TimeSpan value.
        ts = timer.Elapsed;

        // Format and display the TimeSpan value.
        timerObj.GetComponent<Text>().text = ts.ToString(); 
        // ONLY IN .NET 4 :( string.Format("{0:00}:{1:00}:{2:00}", 
        //    ts.Hours, ts.Minutes, ts.Seconds);
    }
}
