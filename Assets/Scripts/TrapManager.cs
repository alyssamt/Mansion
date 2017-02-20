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

    public GameObject skeletonPrefab;
    public Text skeletonText;
    public int skeletonReload;

	// Use this for initialization
	void Start () {
        if (!gm) gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        spikesText.text = "0";
        mirrorText.text = "0";
        skeletonText.text = "0";
	}

    // Update is called once per frame
    void Update()
    {

    }

    public GameObject PlaceTrap()
    {
        Vector3 trapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        trapPos.z = 0;
        GameObject trap = Instantiate(currTrap, trapPos, Quaternion.identity);

        if (currTrap == spikesPrefab)
        {
            if (AddText(spikesText, -1) == 0)
            {
                currTrap = null;
            }
        }
        else if (currTrap == mirrorPrefab)
        {
            if (AddText(mirrorText, -1) == 0)
            {
                currTrap = null;
            }
        }
        else if (currTrap == skeletonPrefab)
        {
            if (AddText(skeletonText, -1) == 0)
            {
                currTrap = null;
            }
        }

        return trap;
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

        for (int i = 0; i != skeletonReload; i++)
        {
            inventory.Add(skeletonPrefab);
            AddText(skeletonText, 1);
        }
    }

    int AddText(Text t, int num)
    {
        int curr = Int32.Parse(t.text);
        t.text = (curr + num).ToString();
        return curr+num;
    }

    public void ResetAllTraps()
    {
        inventory.Clear();
        DestroyAllTraps();
        spikesText.text = "0";
        mirrorText.text = "0";
        skeletonText.text = "0";
    }

    public void DestroyAllTraps()
    {
        GameObject[] traps = GameObject.FindGameObjectsWithTag("Trap");
        foreach (GameObject t in traps)
        {
            Destroy(t);
        }
    }
}
