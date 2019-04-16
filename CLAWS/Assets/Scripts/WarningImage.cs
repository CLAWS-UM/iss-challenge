using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningImage : MonoBehaviour
{
    GameObject warningdisp;
    // Use this for initialization
    void Start() {

        warningdisp.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        warningdisp.SetActive(true);

    }
}
