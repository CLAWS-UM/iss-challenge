using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour {
    float currentTime;
    float startTime = 5f;
    float warningTime = 2f;

    [SerializeField] Text timer;

    void Start()
    {
        currentTime = startTime;
        timer.text = currentTime.ToString("0.0");
    }

    void Update()
    {
        if (currentTime >= 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime >= warningTime)
            {
                timer.color = Color.black;
                timer.text = currentTime.ToString("0.0");
            }
            else
            {
                timer.color = Color.red;
                timer.text = currentTime.ToString("0.00");
            }
        }
        else
        {
            timer.text = "end";
        }
    }
}
