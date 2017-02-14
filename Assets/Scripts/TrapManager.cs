using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrapManager : MonoBehaviour {

    public GameManager gm;
    public List<GameObject> inventory;
    public GameObject currTrap = null;

    public GameObject spikesPrefab;
    public Text spikesText;
    public int spikesReload;

    public GameObject mirrorPrefab;
    public Text mirrorText;
    public int mirrorReload;

	// Use this for initialization
	void Start () {
        if (!gm) gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        spikesText.text = "0";
        mirrorText.text = "0";
	}

    // Update is called once per frame
    void Update()
    {
        if (gm.playing)
        {
            //Place trap on mouse click
            if (Input.GetMouseButtonDown(0) && gm.mode == "setup" && currTrap != null)
            {
                Vector3 trapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                trapPos.z = 0;
                Instantiate(currTrap, trapPos, Quaternion.identity);

                if (currTrap == spikesPrefab)
                {
                    if (AddText(spikesText, -1) == 0)
                    {
                        currTrap = null;
                    }
                } else if (currTrap == mirrorPrefab)
                {
                    if (AddText(mirrorText, -1) == 0)
                    {
                        currTrap = null;
                    }
                }
            }
        }
    }

    public void LoadInventory()
    {
        for (int i = 0; i != spikesReload; i++) {
            inventory.Add(spikesPrefab);
            AddText(spikesText, 1);
        }

        for (int i = 0; i != mirrorReload; i++)
        {
            inventory.Add(mirrorPrefab);
            AddText(mirrorText, 1);
        }
    }

    int AddText(Text t, int num)
    {
        int curr = Int32.Parse(t.text);
        t.text = (curr + num).ToString();
        return curr+num;
    }
}
