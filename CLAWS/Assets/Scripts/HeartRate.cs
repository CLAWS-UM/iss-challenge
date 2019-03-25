using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartRate : MonoBehaviour
{
    int width = 868;
    int height = 1537;

    // Use this for initialization
    void Start()
    {
        transform.localScale = new Vector3((float)0.2 * Screen.width / width, (float)0.7 * Screen.height / height, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;
        Vector3 centre = camera.ViewportToWorldPoint(new Vector3(1, 0, (float)20));
        Vector3 offset = new Vector3(-(GetComponent<Renderer>().bounds.size.x / 2), (GetComponent<Renderer>().bounds.size.y / 2), 0);
        transform.position = centre + offset;
        transform.eulerAngles = camera.transform.eulerAngles;
    }
}
