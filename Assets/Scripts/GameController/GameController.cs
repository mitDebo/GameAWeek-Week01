using System.Collections.Generic;

using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {
    public GameObject BasicBulletPrefab;
    public GameObject BasicEnemyPrefab;

    public int EnemiesInWave;
    public float TimeBetweenWaves;
    public float TimeBetweenFightersInWave;

    bool shieldDeployed;
    public bool ShieldDeployed
    {
        get { return shieldDeployed; }
        set { shieldDeployed = value; }
    }

    Stack<GameObject> basicBullets;
    Stack<GameObject> activeBullets;

    void Start()
    {
        basicBullets = new Stack<GameObject>();
        activeBullets = new Stack<GameObject>();
        for (int i = 0; i < 200; i++)
        {
            GameObject basicBullet = Instantiate(BasicBulletPrefab) as GameObject;
            basicBullet.SetActive(false);
            basicBullets.Push(basicBullet);
        }
        StartCoroutine("gameloop");
    }

    public GameObject GetBasicBullet()
    {
        if (basicBullets.Count > 0)
        {
            GameObject bullet = basicBullets.Pop();
            bullet.SetActive(true);
            activeBullets.Push(bullet);
            return bullet;
        }
        return null;
    }

    public void RecycleBasicBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        basicBullets.Push(bullet);
    }

    IEnumerator gameloop()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeBetweenWaves);
            Vector3 waveOrigin = new Vector3(Random.RandomRange(-6f, 6f), 0f, 9.5f);
            Vector3 waveTarget = new Vector3(Random.RandomRange(-6f, 6f), 0f, 0f);

            for (int i = 0; i < EnemiesInWave; i++)
            {
                GameObject enemy = Instantiate(BasicEnemyPrefab) as GameObject;
                enemy.transform.position = waveOrigin;
                Vector3 target = waveTarget;
                SetupEnemyOnDifficulty(enemy);
                enemy.GetComponent<EnemyMotor>().Go(target - enemy.transform.position, 3);
                yield return new WaitForSeconds(TimeBetweenFightersInWave);
            }
        }
    }

    void SetupEnemyOnDifficulty(GameObject e)
    {
        DirectShot shot = e.AddComponent<DirectShot>();
        shot.TimeBetweenShotsInBurst = Mathf.Clamp(3 - Time.time / 30f, 1, 3);
        shot.TimeBetweenBursts = Mathf.Clamp(2 - Time.time / 30f, 0.2f, 3f);
        shot.ShotsPerBurst = Mathf.Clamp(Mathf.FloorToInt(2 + Time.time / 15f), 1, 5);
        shot.ShotError = Mathf.Clamp(3 - Time.time / 45f, 0.1f, 3f);
        shot.BulletSpeed = Mathf.Clamp(3 + Time.time / 30f, 3f, 10f);
        shot.emitters = new GameObject[1];
        shot.emitters[0] = e.transform.FindChild("EnemyEmiiter").gameObject;

        if (Random.Range(0f, 1f) >= (0.66f - Time.time / 15f))
        {
            WallShot wallShot = e.AddComponent<WallShot>();
            wallShot.BulletSpeed = Mathf.Clamp(5 + Time.time / 30f, 3f, 15f);
            wallShot.emitter = e.transform.FindChild("EnemyEmiiter").gameObject;
            wallShot.ShotsPerBurst = Mathf.Clamp(Mathf.FloorToInt(4 + Time.time / 30f), 4, 8);
            wallShot.TargetSpread = 2f;
            wallShot.TimeBetweenBursts = Mathf.Clamp(3 - Time.time / 30f, 0.3f, 3f);
            wallShot.TimeBetweenShotsInBurst = 0.5f;
            wallShot.WallSpread = 0.5f;
            wallShot.WallWidth = Mathf.Clamp(Mathf.FloorToInt(3 + Time.time / 30f), 3, 5);
        }
    }
}
