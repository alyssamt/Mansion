using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private GameManager gm;

	private GameObject floor1_up;
    private GameObject floor2_down;
	private GameObject floor2_up;
    private GameObject floor3_down;

	// Use this for initialization
	void Start () {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
		floor1_up = GameObject.Find ("Floor1_Up");
        floor2_down = GameObject.Find("Floor2_Down");
		floor2_up = GameObject.Find ("Floor2_Up");
        floor3_down = GameObject.Find("Floor3_Down");
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gm.playing)
        {
			if (collision.tag == "Kid") {
				Kid kidScript = collision.GetComponent<Kid> ();
				if (gameObject.name == "Floor1_Up") {
					collision.transform.position = floor2_down.transform.position+new Vector3(1, 0, 0);
					kidScript.floor += 1;
				} else if (gameObject.name == "Floor2_Down" && kidScript.scared) {
					collision.transform.position = floor1_up.transform.position+new Vector3(1, 0, 0);
					kidScript.floor -= 1;
				} else if (gameObject.name == "Floor2_Up") {
					collision.transform.position = floor3_down.transform.position+new Vector3(-1, 0, 0);
					kidScript.floor += 1;
				} else if (gameObject.name == "Floor3_Down" && kidScript.scared) {
					collision.transform.position = floor2_up.transform.position+new Vector3(-1, 0, 0);
					kidScript.floor -= 1;
				}
				FlipSprite (collision.gameObject);
			}
        }
    }

	private void FlipSprite(GameObject obj) {
		SpriteRenderer rend = obj.GetComponent<SpriteRenderer> ();
		rend.flipX = !rend.flipX;
	}
}
