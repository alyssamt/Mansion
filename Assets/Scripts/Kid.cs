using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kid : MonoBehaviour {

	public float scareMeterMax = 1f;
	public float scareMeter = 0f;

    public int floor;

    public float speed;
    public float movex;
    public float movey;

	private bool scared = false;

    private Rigidbody2D rb;
    private GameManager gm;

	private RectTransform canvasRectT;
	private Slider scareMeterSlider;

    // Use this for initialization
    void Start()
    {
		floor = 1;
		movex = -1;
		movey = 0;

        rb = GetComponent<Rigidbody2D>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();    

		canvasRectT = GetComponentInChildren<RectTransform> ();
		scareMeterSlider = GetComponentInChildren<Slider> ();
	}

    // Update is called once per frame
    void FixedUpdate()
    {
        if (gm.playing)
        {
            rb.velocity = new Vector2(movex * speed, movey * speed);
        } else
        {
            rb.velocity = new Vector2(0, 0);
        }

		//Scare meter position and value
		Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
		scareMeterSlider.GetComponent<RectTransform>().anchoredPosition = (screenPoint - canvasRectT.sizeDelta / 2f)+new Vector2(0, 28);
		scareMeterSlider.value = scareMeter;

		if (!scared && scareMeter >= 1) {
			Debug.Log ("Scared!");
			scareMeterSlider.enabled = false;
			scared = true;
			movex *= -1;
			speed *= 2;
			FlipSprite ();
		}
    }

	void OnBecameInvisible() {
		Destroy (gameObject);
	}

	public void FlipSprite() {
		SpriteRenderer rend = GetComponent<SpriteRenderer> ();
		rend.flipX = !rend.flipX;
	}
}
