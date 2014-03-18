using UnityEngine;
using System.Collections;

public class CircleShot : MonoBehaviour {
    public int ShotsPerBurst;
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
        player = GameObject.FindGameObjectWithTag(Tags.Player).transform;
        StartCoroutine("burst");
    }

    IEnumerator burst()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeBetweenBursts);
            
        }
    }
}
