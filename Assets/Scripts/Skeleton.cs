using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Skeleton : MonoBehaviour {

    public float attack;
    public float defense;

    public float health;
    public float healthMax;

    public float timer;
    public float timerMax;

    public Vector2 sliderPos;

    private RectTransform canvasRectT;
    private Slider healthSlider;

    private AudioSource audioSrc;

    // Use this for initialization
    void Start () {
        health = 0;
        timer = timerMax;

        canvasRectT = GetComponentInChildren<RectTransform>();
        healthSlider = GetComponentInChildren<Slider>();

        audioSrc = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;

        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
        healthSlider.GetComponent<RectTransform>().anchoredPosition = (screenPoint - canvasRectT.sizeDelta / 2f) + sliderPos;
        healthSlider.value = health / healthMax;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (timer <= 0)
        {
            collision.gameObject.GetComponent<Kid>().scareMeter += attack;
            health += defense;
            if (health >= healthMax)
            {
                Destroy(gameObject);
            }
            audioSrc.Play();
            timer = timerMax;
        }
    }
}
