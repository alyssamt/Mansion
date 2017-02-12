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

    // Use this for initialization
    void Start()
    {
        floor = 3;
        lastS = Time.time;
        lastW = Time.time;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gm.playing)
        {
            movex = Input.GetAxis("Horizontal");
            movey = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(movex * speed, movey * speed);

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
                    floor -= 1;
                }
            }

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
                    floor += 1;
                }
            } else if (Input.GetKeyDown("space"))
            {
                Scare();
            }
        }
    }

    void Scare()
    {

    }

    void EnableCollider()
    {
        gameObject.GetComponent<Collider2D>().enabled = true;
    }
}
