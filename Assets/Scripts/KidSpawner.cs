using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KidSpawner : MonoBehaviour {

    public GameManager gm;
    public GameObject kidPrefab;
    public float spawnTimerMax;

    private float spawnTimer;

	// Use this for initialization
	void Start () {
        if (!gm) gm = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	// Update is called once per frame
	void Update () {
		if (gm.playing && gm.mode == "defense")
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0 || gm.NoKidsPresent())
            {
                Instantiate(kidPrefab);
                spawnTimer = spawnTimerMax;
            }
        }
	}

    public void PrepareLevel(int level)
    {
        spawnTimerMax = gm.levelTimerMax / level;
        spawnTimer = 0;
    }
}
