﻿/*  MissionClock.cs
    
    POC: Sahil Farishta
*/
    
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionClock : MonoBehaviour
{
    // Use this for initialization
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        RectTransform parentTransform = (RectTransform)transform.parent.transform;
        RectTransform currentTransform = (RectTransform)transform;
        float xLoc = (parentTransform.rect.width / 4f) - (0.9f * currentTransform.rect.width);
        float yLoc = (parentTransform.rect.height / 2.5f) - (currentTransform.rect.height / 2);
        transform.localPosition = new Vector3(xLoc, yLoc, 0);
    }
}
