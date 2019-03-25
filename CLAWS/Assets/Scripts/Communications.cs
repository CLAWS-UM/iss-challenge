using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communications : MonoBehaviour
{
    int width = 1317;
    int height = 464;

    // Use this for initialization
    void Start()
    {
        transform.localScale = new Vector3((float)0.4 * Screen.width / width, (float)0.1 * Screen.height / height, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;
        Vector3 centre = camera.ViewportToWorldPoint(new Vector3((float)0.5, 0, (float)20));
        Vector3 offset = new Vector3(0, (GetComponent<Renderer>().bounds.size.y / 2), 0);
        transform.position = centre + offset;
        transform.eulerAngles = camera.transform.eulerAngles;
    }
}
