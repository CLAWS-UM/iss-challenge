/*  WarningImage.cs

    This file contains the definition of the WarningImage 
    object, which shows up in the upper-middle area of the 
    display in the case of an EVA emergency.

    POC: Emily Rassel
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningImage : MonoBehaviour
{
    GameObject warningdisp;

    // Use this for initialization
    void Start() 
    {
        warningdisp.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        warningdisp.SetActive(true);

    }
}
