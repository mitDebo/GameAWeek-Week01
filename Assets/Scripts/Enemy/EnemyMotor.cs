using UnityEngine;
using System.Collections;

public class EnemyMotor : MonoBehaviour {
    public int Health;

    Transform mTransform;
    GameController gameController;
    Color originalColor;
    GameObject meshChild;
    GameObject emitter;
    Vector3 travelDirection;

    void Start()
    {
        mTransform = transform;
        gameController = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
        meshChild = transform.FindChild("Mesh").gameObject;
        originalColor = meshChild.renderer.material.color;
        emitter = transform.FindChild("DeathParticles").gameObject;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == Tags.PlayerBullet)
        {
            Health--;
            col.gameObject.SendMessage("KillBullet");
            if (Health <= 0)
            {
                emitter.SetActive(true);
                collider.enabled = false;
                meshChild.SetActive(false);
                SendMessage("StopShot");
                Destroy(gameObject, 3);
            }
        }
    }

    void Update()
    {
        Vector3 botLeft = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.y));
        Vector3 botRight = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, Camera.main.transform.position.y));

        if (transform.position.x < botLeft.x || transform.position.x > botRight.x || transform.position.z < botLeft.z)
            Destroy(gameObject);
    }

    void FixedUpdate()
    {
        transform.position += travelDirection * Time.deltaTime;
    }

    public void Go(Vector3 direction, float speed)
    {
        travelDirection = direction.normalized * speed;
    }
}
