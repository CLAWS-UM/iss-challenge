/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour {
    float currentTime;
    float startTime = 0f;
    float endTime = 3f;
    float warningTime = 1f; // last "warningTime" seconds of the task

    [SerializeField] Text timer;

    void Start()
    {
        currentTime = startTime;
        timer.text = currentTime.ToString("0.0");
    }

    void Update()
    {
        if (currentTime <= endTime)
        {
            currentTime += Time.deltaTime;
            // display with warning color
            if (currentTime >= (endTime - warningTime))
            {
                timer.color = Color.red;
                timer.text = currentTime.ToString("0.0");
            }
            else
            {
                timer.color = Color.black;
                timer.text = currentTime.ToString("0.0");
            }
        }
        else
        {
            timer.text = "end";
        }
    }
}
*/