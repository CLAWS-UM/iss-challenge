using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Communications : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;
        Vector3 centre = camera.ViewportToWorldPoint(new Vector3((float)0.5, 0, (float)20));
        Vector3 offset = new Vector3(0, (GetComponent<Renderer>().bounds.size.y / 2), 0);
        transform.position = centre + offset;
    }
}
