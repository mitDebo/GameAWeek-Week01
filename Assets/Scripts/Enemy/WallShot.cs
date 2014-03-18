using UnityEngine;
using System.Collections;

public class WallShot : MonoBehaviour {
    /**
     * Wall shot fires a wall of bullets
     */
    public int ShotsPerBurst;
    public int WallWidth;
    public float WallSpread;
    public float TargetSpread;
    public float TimeBetweenBursts;
    public float TimeBetweenShotsInBurst;
    public float BulletSpeed;
    public GameObject emitter;

    GameController gameController;
    Transform player;

    void Start()
    {
        gameController = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
        player = null;
        GameObject p = GameObject.FindGameObjectWithTag(Tags.Player);
        if (p != null)
            player = p.transform;
        StartCoroutine("burst");
    }

    IEnumerator burst()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeBetweenBursts);
            Vector3 currentBurstTarget = Vector3.zero;
            if (player != null && player.gameObject.activeInHierarchy)
                currentBurstTarget = player.position;
            else
                StopCoroutine("burst");
            for (int i = 0; i < ShotsPerBurst; i++)
            {
                for (int k = 0; k < WallWidth; k++)
                {
                    int offset = k - Mathf.FloorToInt(WallWidth / 2f);
                    GameObject bullet = gameController.GetBasicBullet();
                    EnemyBulletMotor motor = bullet.GetComponent<EnemyBulletMotor>();

                    Vector3 origin = new Vector3(emitter.transform.position.x + (offset * WallSpread), 0f, emitter.transform.position.z);
                    Vector3 target = new Vector3(origin.x + (currentBurstTarget.x - emitter.transform.position.x) + (offset * TargetSpread), 0f, origin.z + (currentBurstTarget.z - emitter.transform.position.z));
                    motor.Fire(origin, target, BulletSpeed);
                }
                yield return new WaitForSeconds(TimeBetweenShotsInBurst);
            }
        }
    }

    public void StopShot()
    {
        StopCoroutine("burst");
    }
}
