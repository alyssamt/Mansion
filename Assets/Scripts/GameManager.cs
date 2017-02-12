using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public int level;
    public bool playing;
	public float levelTimerMax;
	public GameObject kidPrefab;

	private float levelTimer;

	// Use this for initialization
	void Start () {
		StartGame (); //REMOVE THIS ONCE USING MAIN MENU
	}

	public void StartGame() {
		level = 0;
		NextLevel ();
	}
	
	// Update is called once per frame
	void Update () {
		if (playing) {
			levelTimer -= Time.deltaTime;

			if (levelTimer <= 0) {
				NextLevel ();
			}
		}
	}

	void NextLevel() {
		CancelInvoke ();
		playing = false;
		level += 1;

		float repeatRate = levelTimerMax / 1;
		InvokeRepeating ("SpawnKid", 0, repeatRate);

		levelTimer = levelTimerMax;
		playing = true;
	}

    public void GameOver()
    {
        playing = false;
        Debug.Log("Game Over!");
    }

	void SpawnKid() {
		Instantiate (kidPrefab);
	}
}
