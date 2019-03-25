using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionClock : MonoBehaviour
{
    int width = 1038;
    int height = 400;

    // Use this for initialization
    void Start()
    {
        transform.localScale = new Vector3((float)0.25 * Screen.width / width, (float)0.15 * Screen.height / height, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;
        Vector3 centre = camera.ViewportToWorldPoint(new Vector3(1, 1, (float)20));
        Vector3 offset = new Vector3(-(GetComponent<Renderer>().bounds.size.x / 2), -(GetComponent<Renderer>().bounds.size.y / 2), 0);
        transform.position = centre + offset;
        transform.eulerAngles = camera.transform.eulerAngles;
    }
}
