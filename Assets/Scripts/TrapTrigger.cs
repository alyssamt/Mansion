using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapTrigger : MonoBehaviour {

    public GameManager gm;
    public TrapManager tm;

    public GameObject trap;

    public SpriteRenderer rend;

	// Use this for initialization
	void Awake () {
        if (!gm) gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (!tm) tm = GameObject.Find("TrapManager").GetComponent<TrapManager>();

        rend = GetComponent<SpriteRenderer>();
        rend.enabled = false;

        trap = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /*
    private void OnMouseEnter()
    {
        
        if (ShowHover())
            rend.enabled = true;
            
    }

    private void OnMouseExit()
    {
        rend.enabled = false;
    }
    */

    public void Render()
    {
        if (trap == null)
        {
            rend.enabled = true;
        }
    }

    private void OnMouseDown()
    {
        if (ShowHover() && tm.currTrap != null)
        {
            trap = tm.PlaceTrap();
            rend.enabled = false;
        }
    }

    private bool ShowHover()
    {
        return gm.playing && gm.mode == "setup" && trap == null;
    }
}
