using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivesDisplay : MonoBehaviour {

    public int lives;
    public int maxLives;

    public GameObject life1;
    public GameObject life2;
    public GameObject life3;
    public GameObject life4;
    public GameObject life5;

	// Use this for initialization
	void Start () {
        lives = maxLives;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int LoseLife()
    {
        if (lives == 5)
        {
            life5.SetActive(false);
        }
        else if (lives == 4)
        {
            life4.SetActive(false);
        }
        else if (lives == 3)
        {
            life3.SetActive(false);
        }
        else if (lives == 2)
        {
            life2.SetActive(false);
        }
        else if (lives == 1)
        {
            life1.SetActive(false);
        }

        lives -= 1;
        return lives;
    }

    public void ResetLives()
    {
        lives = maxLives;
        life1.SetActive(true);
        life2.SetActive(true);
        life3.SetActive(true);
        life4.SetActive(true);
        life5.SetActive(true);
    }
}
