using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    float currentTime;
    float startTime = 0f;

    [SerializeField] Text timer;

    void Start()
    {
        currentTime = startTime;
        timer.text = currentTime.ToString("0.0");
    }

    void Update()
    {
        currentTime += Time.deltaTime;
        timer.text = currentTime.ToString("0.0");
    }
}
