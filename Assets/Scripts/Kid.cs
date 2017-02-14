using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kid : MonoBehaviour {

    public bool alerted = false;

    public int floor;
    public int pointValue;

    public float speed;
    public float movex;
    public float movey;
    public float scareMeter = 0f;
    public float scareMeterMax = 1f;

    public float audioCountdown;
    public AudioClip shortScream;
    public AudioClip longScream;

    public Color alertedColor;

    private GameObject player;
    private Player playerScript;

    private AudioSource audioSource;

    private bool scared = false;

    private GameManager gm;
    private Rigidbody2D rb;
    private SpriteRenderer rend;

	private RectTransform canvasRectT;
	private Slider scareMeterSlider;

    // Use this for initialization
    void Start()
    {
		floor = 1;
		movex = -1;
		movey = 0;

        player = GameObject.Find("Player");
        if (player) playerScript = player.GetComponent<Player>();

        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();

		canvasRectT = GetComponentInChildren<RectTransform> ();
		scareMeterSlider = GetComponentInChildren<Slider> ();

        audioSource = GetComponent<AudioSource>();
        audioCountdown = 0;
	}
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (gm.playing)
        {
            //Movement
            if (!scared && ((alerted && playerScript.floor == floor) || PlayerNear()))
            {
                Vector2 xdir = (player.transform.position - transform.position).normalized;
                rb.velocity = new Vector2(xdir.x, movey)*speed*1.5f;
                alerted = true;
            }
            else
            {
                rb.velocity = new Vector2(movex * speed, movey * speed);
                alerted = false;
            }

            //Color
            if (alerted)
            {
                rend.color = alertedColor;
            } else
            {
                rend.color = new Color(255, 255, 255, 255);
            }

            //Scare meter position and value
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(Camera.main, transform.position);
            scareMeterSlider.GetComponent<RectTransform>().anchoredPosition = (screenPoint - canvasRectT.sizeDelta / 2f) + new Vector2(0, 28);
            scareMeterSlider.value = scareMeter / scareMeterMax;

            if (!scared && scareMeter >= scareMeterMax)
            {
                audioSource.clip = longScream;
                Scream(true);
                scareMeterSlider.enabled = false;
                scared = true;
                movex *= -1;
                speed *= 2;
                FlipSprite();
                gm.score += pointValue;
                gameObject.layer = 10;
            }

            audioCountdown -= Time.deltaTime;
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            player.GetComponent<Player>().DecrementLife();
        }
    }

    void OnBecameInvisible() {
		Destroy (gameObject);
	}

	public void FlipSprite() {
		SpriteRenderer rend = GetComponent<SpriteRenderer> ();
		rend.flipX = !rend.flipX;
	}

    public void TakeDamage(float damage)
    {
        scareMeter += damage;
        Scream();
    }

    public void Scream(bool over_ride=false)
    {
        if (over_ride || audioCountdown <= 0)
        {
            audioSource.Play();
            audioCountdown = audioSource.clip.length;
        }
    }

    bool PlayerNear()
    {
        if (playerScript)
        {
            if (playerScript.floor == floor && !playerScript.possessing)
            {
                if (movex == 1)
                {
                    return player.transform.position.x > transform.position.x;
                }
                else
                {
                    return player.transform.position.x < transform.position.x;
                }
            }
        } else
        {
            player = GameObject.Find("Player");
            if (player) playerScript = player.GetComponent<Player>();
        }
        return false;
    }

    public void GetScared(int damage)
    {
        scareMeter += damage;
        Scream();
        alerted = true;
    }
}
