using UnityEngine;
using System.Collections;

public class FloorScroller : MonoBehaviour {
    public GameObject[] FloorSegments;
    public float ScrollSpeed;

    Camera mainCamera;
    Vector3 bottomLeft;

    void Start()
    {
        mainCamera = Camera.main;
        bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.y - transform.position.y));
    }

    void FixedUpdate()
    {
        foreach (GameObject fs in FloorSegments)
        {
            fs.transform.position = new Vector3(fs.transform.position.x, fs.transform.position.y, fs.transform.position.z + (ScrollSpeed * Time.deltaTime));
            if (fs.transform.position.z + 7 < bottomLeft.z)
                fs.transform.position = new Vector3(fs.transform.position.x, fs.transform.position.y, fs.transform.position.z + 42);
        }
    }
}
