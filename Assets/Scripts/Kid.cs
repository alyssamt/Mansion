using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kid : MonoBehaviour {

    public bool alerted = false;
    public bool scared = false;

    public int floor;
    public int pointValue;

    public float speed;
    public float scareMeter = 0f;
    public float scareMeterMax = 1f;
    public float alertRadius;

    public float audioCountdown;
    public AudioClip shortScream;
    public AudioClip longScream;

    public Color alertedColor;

    public Vector2 sliderPos;

    private GameObject player;
    private Player playerScript;

    private AudioSource audioSource;

    private GameManager gm;
    private Rigidbody2D rb;
    private SpriteRenderer rend;

	private RectTransform canvasRectT;
	private Slider scareMeterSlider;

    // Use this for initialization
    void Start()
    {
		floor = 1;

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
            if (scared)
            {
                alerted = false;
                if (floor == 2)
                {
                    rb.velocity = new Vector2(-speed*3, 0);
                }
                else
                {
                    rb.velocity = new Vector2(speed*3, 0);
                }
            }
            else
            {
                if (PlayerNear(alerted))
                {
                    Vector2 xdir = (player.transform.position - transform.position).normalized;
                    rb.velocity = new Vector2(xdir.x, 0) * speed * 1.5f;
                    alerted = true;
                }
                else
                {
                    alerted = false;
                    if (floor == 2)
                    {
                        rb.velocity = new Vector2(speed, 0);
                    }
                    else
                    {
                        rb.velocity = new Vector2(-speed, 0);
                    }
                }
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
            scareMeterSlider.GetComponent<RectTransform>().anchoredPosition = (screenPoint - canvasRectT.sizeDelta / 2f) + sliderPos;
            scareMeterSlider.value = scareMeter / scareMeterMax;

            if (!scared && scareMeter >= scareMeterMax)
            {
                audioSource.clip = longScream;
                Scream(true);
                scareMeterSlider.enabled = false;
                scared = true;
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

    public bool PlayerNear(bool myalerted)
    {
        if (playerScript)
        {
            if (playerScript.floor == floor && !playerScript.possessing && !scared)
            {
                if (myalerted && Vector3.Distance(player.transform.position, transform.position) <= alertRadius) return true;

                if (floor == 2)
                {
                    return (player.transform.position.x > transform.position.x);
                }
                else
                {
                    return (player.transform.position.x < transform.position.x);
                }
            }
        } else
        {
            player = GameObject.Find("Player");
            if (player) playerScript = player.GetComponent<Player>();
        }
        return false;
    }

    public void GetScared(float damage)
    {
        scareMeter += damage;
        Scream();
        alerted = true;
    }
}
