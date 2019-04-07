using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    float currentTime;
    float startTime = 0f;

    //[SerializeField] Text timer;

    void Start()
    {
        currentTime = startTime;
        //timer.text = currentTime.ToString("0.00");
    
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        //timer.text = currentTime.ToString("0.00");
    }

    void OnGUI()
    {
        float hours = Mathf.Floor(currentTime / 3600);
        float minutes = Mathf.Floor((currentTime % 3600) / 60);
        float seconds = currentTime % 60;

        GUIStyle clockStyle = new GUIStyle();
        clockStyle.fontSize = 20;
        clockStyle.normal.textColor = Color.white;
        GUI.Label(new Rect(865, 25, 250, 100), hours + ":" + minutes + ":" + Mathf.RoundToInt(seconds), clockStyle);
    }
}
