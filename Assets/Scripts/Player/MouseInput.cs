using UnityEngine;
using System.Collections;

public class MouseInput : MonoBehaviour {
    float cameraHeight;

    public bool LeftMouseButton
    {
        get { return Input.GetMouseButton(0); }
    }

    public bool RightMouseButton
    {
        get { return Input.GetMouseButton(1); }
    }

    void Start () 
    {
        Screen.showCursor = false;
        cameraHeight = Camera.main.transform.position.y;
    }
	
	void FixedUpdate() 
    {
        // Find the mouse position in the world, without letting it go outside the screen
        Vector3 cameraBottomLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, cameraHeight));
        Vector3 cameraTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cameraHeight));
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cameraHeight));
        transform.position = new Vector3(Mathf.Clamp(worldPosition.x, cameraBottomLeft.x, cameraTopRight.x), 0f, Mathf.Clamp(worldPosition.z, cameraBottomLeft.z, cameraTopRight.z));
	}
}