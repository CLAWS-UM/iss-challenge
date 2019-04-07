using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QRReader;
using UnityEngine.UI;

public class QRReaderSample : MonoBehaviour
{
    [SerializeField] private Text text;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(QRRead.QRReaderCoroutine(s => text.text = s, true));
    }
}
