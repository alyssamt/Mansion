﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HauntedObject : MonoBehaviour {

    public GameManager gm;
    public GameObject player;
    public Color touchingColor;
    public Color possessedColor;
    public Vector3 shakeAmount;
    public float shakeTime;
    public int scareDamage;

    private int floor;
    private bool touching;
    private Player playerScript;

	// Use this for initialization
	void Start () {
        if (!gm) gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (!player) player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();

        if (transform.position.y > gm.floor3_y)
        {
            floor = 3;
        }
        else if (transform.position.y > gm.floor2_y)
        {
            floor = 2;
        }
        else
        {
            floor = 1;
        }
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R) && touching && gm.mode == "defense")
        {
            Possess();
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Player" && gm.mode == "defense")
        {
            touching = true;
            gameObject.GetComponent<SpriteRenderer>().color = touchingColor;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "Player")
        {
            touching = false;
            gameObject.GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 255);
        }
    }

    void Possess()
    {
        playerScript.possessing = true;
        player.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().color = possessedColor;
        iTween.ShakePosition(gameObject, shakeAmount, shakeTime);
        iTween.ShakeRotation(gameObject, shakeAmount, shakeTime);

        GameObject[] kids = GameObject.FindGameObjectsWithTag("Kid");
        foreach (GameObject kid in kids)
        {
            Kid kidScript = kid.GetComponent<Kid>();
            if (kidScript.floor == floor)
            {
                kidScript.alerted = false;
                kidScript.scareMeter += scareDamage;
                kidScript.Scream();
            }
        }

        Invoke("DestroyMe", shakeTime);
    }

    void DestroyMe()
    {
        playerScript.possessing = false;
        player.SetActive(true);
        Destroy(gameObject);
    }
}