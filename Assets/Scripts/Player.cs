using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public bool possessing = false;

    public float speed;
    public float epowerMax;

    public int floor;

    public LivesDisplay livesDisplay;
    public GameObject lifeLost;
    public GameObject epowerDisplay;

    public Color hurtColor;

    public AudioClip hiss;
    public AudioClip groan;

    private Vector3 origPos;

    private float movex;
    private float movey;
    private float epower;

    private GameManager gm;
    private Rigidbody2D rb;
    private SpriteRenderer rend;
    private AudioSource audioSource;
    private Slider epowerSlider;

    // Use this for initialization
    void Start()
    {
        floor = 3;
        if (!livesDisplay) livesDisplay = GameObject.Find("Lives").GetComponent<LivesDisplay>();
        if (!lifeLost) lifeLost = GameObject.Find("LifeLost");
        if (!epowerDisplay) epowerDisplay = GameObject.Find("EPower");
        epowerSlider = epowerDisplay.GetComponent<Slider>();
        epower = epowerMax;
        lifeLost.SetActive(false);
        origPos = transform.position;
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        if (gm.playing)
        {
            //E Power
            epower += Time.deltaTime;
            if (epower > epowerMax) epower = epowerMax;
            epowerSlider.value = epower/epowerMax;

			//Movement
            movex = Input.GetAxis("Horizontal");
            movey = Input.GetAxis("Vertical");
            rb.velocity = new Vector2(movex * speed, movey * speed);

            //Enable movement through floor/ceiling
            if (Input.GetKey("space"))
            {
                gameObject.layer = 15;
                rend.color = new Color(1, 1, 1, (float) 180/255);
            }
            else
            {
                gameObject.layer = 9;
                rend.color = new Color(1, 1, 1, 1);

                //Scare with E
                if (Input.GetKeyDown(KeyCode.E))
                {
                    Scare();
                }
            }

			//Calculate floor
			if (transform.position.y > gm.floor3_y) {
				floor = 3;
			} else if (transform.position.y > gm.floor2_y) {
				floor = 2;
			} else {
				floor = 1;
			}
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Kid" && !possessing)
        {
            DecrementLife();
        }
    }

    void Scare()
    {
        audioSource.clip = hiss;
        audioSource.Play();
		GameObject[] kids = GameObject.FindGameObjectsWithTag ("Kid");
		foreach (GameObject kid in kids) {
			Kid kidScript = kid.GetComponent<Kid> ();
			if (kidScript.PlayerNear(true)) {
                kidScript.GetScared(epower);
			}
		}
        epower = 0;
    }

    public void DecrementLife()
    {
        rend.color = hurtColor;
        lifeLost.SetActive(true);
        Invoke("Revert", 1);

        audioSource.clip = groan;
        audioSource.Play();

        if (livesDisplay.LoseLife() == 0)
        {
            gm.GameOver("You were staked through the heart.");
        }
    }

    void Revert()
    {
        rend.color = new Color(255, 255, 255, 255);
        lifeLost.SetActive(false);
    }

    public void ResetMe()
    {
        transform.position = origPos;
    }
}
