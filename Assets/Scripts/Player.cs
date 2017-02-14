using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public bool possessing = false;

    public float speed;

    public int floor;
    public int lives;
    public int damage;
    public int maxLives;

    public GameObject livesDisplay;

    public Color hurtColor;

    public AudioClip hiss;
    public AudioClip groan;

    private bool ready = false;

    private float movex;
    private float movey;

    private GameManager gm;
    private Rigidbody2D rb;
    private SpriteRenderer rend;
    private AudioSource audioSource;

    // Use this for initialization
    void Start()
    {
        floor = 3;
        lives = maxLives;
        if (!livesDisplay) livesDisplay = GameObject.Find("Lives");
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gm.playing)
        {
			//Movement
            movex = Input.GetAxis("Horizontal");
            movey = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(movex * speed, movey * speed);

			//Go through ceiling
            if (Input.GetKey(KeyCode.W) && ready == false)
            {
                ready = true;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                ready = false;
            }
            if (Input.GetKeyDown("space") && ready == true)
            {
                ready = false;
                if (floor != 3)
                {
                    //Go through ceiling
                    gameObject.GetComponent<Collider2D>().enabled = false;
                    Invoke("EnableCollider", 0.5f);
                }
            }

			//Go through floor
            if (Input.GetKey(KeyCode.S) && ready == false)
            {
                ready = true;
            }
            if (Input.GetKeyUp(KeyCode.S))
            {
                ready = false;
            }

            if (Input.GetKeyDown("space") && ready == true)
            {
                ready = false;
                if (floor != 1)
                {
                    //Go through floor
                    gameObject.GetComponent<Collider2D>().enabled = false;
                    Invoke("EnableCollider", 0.5f);
                }
            }

			//Calculate floor
			if (transform.position.y > gm.floor3_y) {
				floor = 3;
			} else if (transform.position.y > gm.floor2_y) {
				floor = 2;
			} else {
				floor = 1;
			}

			//Scare with spacebar
			if (Input.GetKeyDown(KeyCode.E))
            {
                Scare();
            }
        }
    }

    void Scare()
    {
        audioSource.clip = hiss;
        audioSource.Play();
		GameObject[] kids = GameObject.FindGameObjectsWithTag ("Kid");
		foreach (GameObject kid in kids) {
			Kid kidScript = kid.GetComponent<Kid> ();
			if (kidScript.floor == floor) {
                kidScript.GetScared(damage);
			}
		}
    }

    void EnableCollider()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;
    }

    public void DecrementLife()
    {
        rend.color = hurtColor;
        Invoke("RevertColor", 1);

        audioSource.clip = groan;
        audioSource.Play();

        lives -= 1;

        Transform heart = livesDisplay.transform.GetChild(lives);
        heart.gameObject.SetActive(false);

        if (lives == 0)
        {
            gm.GameOver("You were staked through the heart.");
        }
    }

    void RevertColor()
    {
        rend.color = new Color(255, 255, 255, 255);
    }
}
