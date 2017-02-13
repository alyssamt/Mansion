using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Trap : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public GameManager gm;
    public GameObject prefab;
    public Text remaining;

    private SpriteRenderer sprite;

    void Awake()
    {
        if (!gm) gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        sprite = GetComponent<SpriteRenderer>();
        remaining = GetComponentInChildren<Text>();
    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (remaining.text != "0")
        {
            gm.trap = prefab;
        } else
        {
            gm.trap = null;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

    }
}