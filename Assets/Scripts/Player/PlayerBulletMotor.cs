using UnityEngine;
using System.Collections;

public class PlayerBulletMotor : MonoBehaviour {
    public float Speed;

    float cameraHeight;
    GameObject player;

    void Start()
    {
        cameraHeight = Camera.main.transform.position.y;
        player = GameObject.FindGameObjectWithTag(Tags.Player);
    }

    public void FireBullet(Vector3 origin)
    {
        transform.position = origin;
    }

	void FixedUpdate () {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + (Time.deltaTime * Speed));
        Vector3 cameraTopRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cameraHeight));
        
        if (transform.position.z > cameraTopRight.z)
            KillBullet();
	}

    public void KillBullet()
    {
        player.SendMessageUpwards("RecycleBullet", this.gameObject);
    }
}
