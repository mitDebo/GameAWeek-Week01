using UnityEngine;
using System.Collections;

public class DirectShot : MonoBehaviour {
    /**
     * The Direct Shot shots a number of shots in the general direction of the player, with a certain amount of aim error
     * It shoots shotsPerBurst bullets every timeBetweenBursts seconds. Shoots each shot somewhere within ShotError distance
     * of the player
     */
    public int ShotsPerBurst;
    public float TimeBetweenBursts;
    public float ShotError;
    public float TimeBetweenShotsInBurst;
    public float BulletSpeed;
    public GameObject[] emitters;

    GameController gameController;
    Transform player;

	void Start () {
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
            for (int i = 0; i < ShotsPerBurst; i++)
            {
                foreach (GameObject e in emitters)
                {
                    GameObject bullet = gameController.GetBasicBullet();
                    EnemyBulletMotor motor = bullet.GetComponent<EnemyBulletMotor>();
                    
                    Vector3 target = Vector3.zero;
                    if (player != null && player.gameObject.activeInHierarchy)
                        target = new Vector3(player.position.x + (Random.Range(-ShotError, ShotError)), 0f, player.position.z + (Random.Range(-ShotError, ShotError)));
                    else
                    {
                        StopShot();
                        yield return new WaitForSeconds(TimeBetweenShotsInBurst);
                    }                       

                    motor.Fire(e.transform.position, target, BulletSpeed);
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
