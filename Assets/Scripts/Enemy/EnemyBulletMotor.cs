using UnityEngine;
using System.Collections;

public class EnemyBulletMotor : MonoBehaviour {
    public float Acceleration;
    public float Damage;
    public float MaxVelocity;

    Vector3 speedVector;
    Vector3 accelerationVector;
    GameController gameController;
    Transform player;
    Transform mTransform;

    Camera mainCamera;

    public void Fire(Vector3 origin, Vector3 target, float speed)
    {
        transform.position = origin;
        speedVector = (target - transform.position).normalized * speed;
    }

    void Start()
    {
        mTransform = transform;
        gameController = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag(Tags.Player).transform;
        mainCamera = Camera.main;
    }

    void FixedUpdate()
    {
        if (gameController.ShieldDeployed)
        {
            Vector3 direction = player.transform.position - mTransform.position;
            float distanceSquared = direction.sqrMagnitude;
            speedVector += direction.normalized * (Acceleration / distanceSquared);
        }
        mTransform.position = mTransform.position + (speedVector * Time.deltaTime);

        float cameraHeight = mainCamera.transform.position.y;
        Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0f, 0f, cameraHeight));
        Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, cameraHeight));
        if (mTransform.position.x < bottomLeft.x - 2 || mTransform.position.z < bottomLeft.z - 2 || mTransform.position.x > topRight.x + 2 || mTransform.position.z > topRight.z + 2)
            gameController.SendMessage("RecycleBasicBullet", gameObject);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == Tags.Player)
        {
            if (gameController.ShieldDeployed)
                player.SendMessage("ChargeShield");
            else
                player.SendMessage("DamageShield", Damage);
            gameController.SendMessage("RecycleBasicBullet", gameObject);
        }
        if (col.gameObject.tag == Tags.Shield)
        {
            if (gameController.ShieldDeployed)
                player.SendMessage("ChargeShield");
            gameController.SendMessage("RecycleBasicBullet", gameObject);
        }
    }
}
