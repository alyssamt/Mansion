using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public string mode;

    public bool playing;

    public float setupTimerMax;
    public float levelTimerMax;
    public float floor2_y;
    public float floor3_y;

    public int score;
    public int level;
    public int speechDelay;
    public int levelPointValue;

    public GameObject trap = null;
    public GameObject player;
	public GameObject kidPrefab;
    public GameObject speechBubble;

    public GameObject gameScreen;
    public GameObject levelTransitionScreen;
    public GameObject gameOverScreen;

    public TrapManager tm;
    public KidSpawner kidSpawner;
    public Slider timeSlider;
    public Text levelText;
    public Text scoreText;

    private float levelTimer;
    private Text bubbleText;
    private Text modeText;

    // Use this for initialization
    void Start () {
        score = 0;

        if (!player) player = GameObject.Find("Player");
        if (!speechBubble) speechBubble = GameObject.Find("SpeechBubble");
        bubbleText = speechBubble.GetComponentInChildren<Text>();

        if (!gameScreen) gameScreen = GameObject.Find("GameScreen");
        if (!levelTransitionScreen) levelTransitionScreen = GameObject.Find("LevelTransitionScreen");
        if (!gameOverScreen) gameOverScreen = GameObject.Find("GameOverScreen");

        if (!tm) tm = GameObject.Find("TrapManager").GetComponent<TrapManager>();
        if (!kidSpawner) kidSpawner = GameObject.Find("KidSpawner").GetComponent<KidSpawner>();
        if (!timeSlider) timeSlider = GameObject.Find("TimeSlider").GetComponent<Slider>();
        timeSlider.value = 1;
        modeText = timeSlider.GetComponentInChildren<Text>();
        if (!levelText) levelText = GameObject.Find("LevelText").GetComponent<Text>();
        if (!scoreText) scoreText = GameObject.Find("ScoreText").GetComponent<Text>();

        levelTimer = setupTimerMax;

        gameScreen.SetActive(true);

        floor2_y = GameObject.Find("Floor2").transform.position.y - 1.5f;
        floor3_y = GameObject.Find("Floor3").transform.position.y - 1.5f;

        gameScreen.SetActive(false);
        levelTransitionScreen.SetActive(false);
        gameOverScreen.SetActive(false);

		//StartGame (); //REMOVE THIS ONCE USING MAIN MENU
	}

	public void StartGame() {
        bubbleText.text = "Halloween again, eh?";
        tm.inventory.Clear();
		level = 0;
		NextLevel ();
	}
	
	// Update is called once per frame
	void Update () {
		if (playing) {

            //Update all text
            modeText.text = mode;
            levelText.text = "Level: ";
            levelText.text += level;
            scoreText.text = "Score: ";
            scoreText.text += score;

            //Decrement timer and check if level over
			levelTimer -= Time.deltaTime;
            if (mode == "setup")
            {
                timeSlider.value = levelTimer / setupTimerMax;
                if (levelTimer <= 0) StartDefense();
            }
            else
            {
                timeSlider.value = levelTimer / levelTimerMax;
                if (levelTimer <= 0)
                {
                    if (NoKidsPresent())
                    {
                        NextLevel();
                    } else
                    {
                        mode = "standby";
                    }
                }
            }
		}
	}

	void NextLevel() {
		CancelInvoke ();
		playing = false;
        score += level * levelPointValue;
		level += 1;
        tm.LoadInventory();
        kidSpawner.PrepareLevel(level);

        if (level == 1)
        {
            StartSetup();
            levelTimer += 4 * speechDelay;
            speechBubble.SetActive(true);
            Invoke("Speech", speechDelay);
        } else
        {
            levelTransitionScreen.SetActive(true);
            Text hoursLeftText = GameObject.Find("HoursLeftText").GetComponent<Text>();
            hoursLeftText.text = "Hours Left: ";
            hoursLeftText.text += (12 - (level - 2));
            mode = "setup";
            Invoke("StartSetup", 2);
        }
	}

    void StartSetup()
    {
        DestroyAll("Kid");
        levelTransitionScreen.SetActive(false);
        mode = "setup";
        playing = true;
        levelTimer = setupTimerMax;
    }

    void StartDefense()
    {
        mode = "defense";
        levelTimer = levelTimerMax;
    }

    public void GameOver(string message="")
    {
        playing = false;
        Clear();
        gameOverScreen.SetActive(true);

        if (message != "")
        {
            Text gameOverText = GameObject.Find("GameOverText").GetComponent<Text>();
            gameOverText.text = message;
        }

        Text finalScoreText = GameObject.Find("FinalScoreText").GetComponent<Text>();
        finalScoreText.text = "Final Score: ";
        finalScoreText.text += score;
    }

    void Speech()
    {
        bubbleText.text = "Every year those darned kids come in here searching for my golden skull!";
        Invoke("Speech2", speechDelay);
    }

    void Speech2()
    {
        bubbleText.text = "Little do they know, it will incinerate mere mortals upon touch!";
        Invoke("Speech3", speechDelay);
    }

    void Speech3()
    {
        bubbleText.text = "For their own good, I have to scare those kids away from here.";
        Invoke("SetBubbleInactive", speechDelay);
    }

    void SetBubbleInactive()
    {
        speechBubble.SetActive(false);
    }

    void Clear()
    {
        DestroyAll("Kid");
        DestroyAll("Trap");
        player.transform.position = new Vector3(-4, 1.2f, 0);
    }

    void DestroyAll(string tag)
    {
        CancelInvoke();
        GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
        foreach (GameObject o in objs)
        {
            Destroy(o);
        }
    }

    public bool NoKidsPresent()
    {
        GameObject[] kids = GameObject.FindGameObjectsWithTag("Kid");
        return kids.Length == 0;
    }
}
