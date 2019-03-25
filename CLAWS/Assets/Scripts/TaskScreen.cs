using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskScreen : MonoBehaviour
{
    int width = 809;
    int height = 1331;

	// Use this for initialization
	void Start ()
    {
        transform.localScale = new Vector3((float) 0.2 * Screen.width / width, (float) 0.65 * Screen.height / height, 0f);
    }
	
	// Update is called once per frame
	void Update ()
    {
        Camera camera = Camera.main;
        Vector3 centre = camera.ViewportToWorldPoint(new Vector3(0, 1, (float)20));
        Vector3 offset = new Vector3(GetComponent<Renderer>().bounds.size.x /2, -(GetComponent<Renderer>().bounds.size.y/2), 0);
        transform.position = centre + offset;
        transform.eulerAngles = camera.transform.eulerAngles;
    }
}
