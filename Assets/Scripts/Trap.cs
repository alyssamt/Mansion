using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Trap : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    //public GameManager gm;
    public TrapManager tm;
    public GameObject prefab;
    public Text remaining;

    void Awake()
    {
        //if (!gm) gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (!tm) tm = GameObject.Find("TrapManager").GetComponent<TrapManager>();
        remaining = GetComponentInChildren<Text>();
    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (remaining.text != "0")
        {
            tm.currTrap = prefab;
        } else
        {
            tm.currTrap = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}