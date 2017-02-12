using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private GameManager gm;
    private GameObject floor2_down;
    private GameObject floor3_down;

	// Use this for initialization
	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        floor2_down = GameObject.Find("Floor2_Down");
        floor3_down = GameObject.Find("Floor3_Down");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gm.playing)
        {
            if (gameObject.name == "Floor1_Up")
            {
                collision.transform.position = floor2_down.transform.position;
                collision.GetComponent<SpriteRenderer>().flipX = false;
            }
            else if (gameObject.name == "Floor2_Up")
            {
                collision.transform.position = floor3_down.transform.position;
                collision.GetComponent<SpriteRenderer>().flipX = true;
            }
            collision.GetComponent<Kid>().movex *= -1;
            collision.GetComponent<Kid>().floor += 1;
        }
    }
}
