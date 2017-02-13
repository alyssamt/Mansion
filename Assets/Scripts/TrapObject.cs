using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapObject : MonoBehaviour {

    public float damage;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Kid")
        {
            Kid kidScript = collision.GetComponent<Kid>();
            kidScript.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
