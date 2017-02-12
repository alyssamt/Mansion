using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public float speed;
    private float movex;
    private float movey;
    private Rigidbody2D rb;
    private GameManager gm;

    private int floor;
    private float lastW;
    private float lastS;

    private bool ready = false;

	private float floor1_y;
	private float floor2_y;
	private float floor3_y;

    // Use this for initialization
    void Start()
    {
        floor = 3;
        lastS = Time.time;
        lastW = Time.time;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();

		floor1_y = GameObject.Find ("Floor1").transform.position.y - 1.5f;
		floor2_y = GameObject.Find ("Floor2").transform.position.y - 1.5f;
		floor3_y = GameObject.Find ("Floor3").transform.position.y - 1.5f;
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
			if (transform.position.y > floor3_y) {
				floor = 3;
			} else if (transform.position.y > floor2_y) {
				floor = 2;
			} else {
				floor = 1;
			}

			//Scare with spacebar
			if (!ready && Input.GetKeyDown("space"))
            {
                Scare();
            }
        }
    }

    void Scare()
    {
		GameObject[] kids = GameObject.FindGameObjectsWithTag ("Kid");
		foreach (GameObject kid in kids) {
			Kid kidScript = kid.GetComponent<Kid> ();
			if (kidScript.floor == floor) {
				kidScript.scareMeter += 0.1f;
				Debug.Log ("Scare meter: " + kidScript.scareMeter);
			}
		}
    }

    void EnableCollider()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;
    }
}
