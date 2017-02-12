using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kid : MonoBehaviour {

    public int floor;
    public float speed;
    public float movex;
    private float movey;
    private Rigidbody2D rb;
    private GameManager gm;

    // Use this for initialization
    void Start()
    {
        movex = -1;
        movey = 0;
        floor = 1;
        rb = GetComponent<Rigidbody2D>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gm.playing)
        {
            rb.velocity = new Vector2(movex * speed, movey * speed);
        } else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }
}
