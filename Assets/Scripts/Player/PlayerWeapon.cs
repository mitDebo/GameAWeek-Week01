using System.Collections.Generic;

using UnityEngine;
using System.Collections;

public class PlayerWeapon : MonoBehaviour {
    public GameObject BulletPrefab;
    public GameObject[] BulletEmitter;
    public float TimeBetweenFire;

    MouseInput mouseInput;
    Stack<GameObject> bulletPool;
    Shield shield;

	void Start () 
    {
        mouseInput = GetComponent<MouseInput>();
        shield = GetComponent<Shield>();
        bulletPool = new Stack<GameObject>();

        // Instantiate a number of bullets for use in the dead bullets array list
        for (int i = 0; i < BulletEmitter.Length * 10; i++)
        {
            GameObject bullet = Instantiate(BulletPrefab) as GameObject;
            bullet.SetActive(false);
            bulletPool.Push(bullet);
        }
        StartCoroutine("fire");
	}

    public void RecycleBullet(GameObject bullet)
    {
        bullet.SetActive(false);
        bulletPool.Push(bullet);
    }

    // The script for firing a gun
    IEnumerator fire()
    {
        while (true)
        {
            bool foundBullet = false;
            if (!mouseInput.RightMouseButton && mouseInput.LeftMouseButton && shield.Power > 0)
            {
                foreach (GameObject e in BulletEmitter)
                {
                    GameObject b = findDeadBullet();
                    if (b != null)
                    {
                        foundBullet = true;
                        b.SetActive(true);
                        b.SendMessage("FireBullet", e.transform.position);
                    }
                }
                if (foundBullet)
                    shield.ReduceShieldFromShot();
                yield return new WaitForSeconds(TimeBetweenFire);
            }
            yield return 0;
        }
    }

    GameObject findDeadBullet()
    {
        if (bulletPool.Count > 0)
            return bulletPool.Pop();
        return null;
    }
}
