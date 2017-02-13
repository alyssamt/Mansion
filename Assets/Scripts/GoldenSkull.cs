using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldenSkull : MonoBehaviour
{

    private GameManager gm;

    // Use this for initialization
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (gm.playing)
        {
            if (collision.gameObject.tag == "Kid")
            {
                gm.GameOver();
            }
        }
    }
}
