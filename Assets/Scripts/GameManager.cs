using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public int levelPointValue;
    public int score;
    public int level;
    public int speechDelay;
    public bool playing;
    public string mode;
    public float setupTime;
	public float levelTimerMax;
    public GameObject trap = null;
	public GameObject kidPrefab;
    public GameObject gameOverScreen;
    public GameObject levelTransitionScreen;
    public GameObject speechBubble;
    public Slider timeSlider;
    public Text hoursLeftText;
    public Text levelText;
    public Text scoreText;

	private float levelTimer;
    private Text bubbleText;
    private Text modeText;

	// Use this for initialization
	void Start () {
        score = 0;
        if (!speechBubble) speechBubble = GameObject.Find("SpeechBubble");
        bubbleText = speechBubble.GetComponentInChildren<Text>();

        if (!timeSlider) timeSlider = GameObject.Find("TimeSlider").GetComponent<Slider>();
        timeSlider.value = 1;
        modeText = timeSlider.GetComponentInChildren<Text>();

        if (!levelText) levelText = GameObject.Find("LevelText").GetComponent<Text>();
        if (!scoreText) scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        if (!gameOverScreen) gameOverScreen = GameObject.Find("GameOverScreen");
        if (!levelTransitionScreen) levelTransitionScreen = GameObject.Find("LevelTransitionScreen");
        if (!hoursLeftText) hoursLeftText = GameObject.Find("HoursLeftText").GetComponent<Text>();

        levelTransitionScreen.SetActive(false);
        gameOverScreen.SetActive(false);

		StartGame (); //REMOVE THIS ONCE USING MAIN MENU
	}

	public void StartGame() {
		level = 0;
		NextLevel ();
	}
	
	// Update is called once per frame
	void Update () {
		if (playing) {

            //Place trap on mouse click
            if (Input.GetMouseButtonDown(0) && mode == "setup" && trap != null)
            {
                Debug.Log("Setting trap");
                Debug.Log(Input.mousePosition);
                Debug.Log(Camera.main);
                Vector3 trapPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                trapPos.z = 0;
                GameObject trapClone = Instantiate(trap, trapPos, Quaternion.identity);
            }

            //Update all text
            modeText.text = mode;
            levelText.text = "Level: ";
            levelText.text += level;
            scoreText.text = "Score: ";
            scoreText.text += score;

            //Decrement timer and check if level over
			levelTimer -= Time.deltaTime;
            timeSlider.value = levelTimer/levelTimerMax;
			if (levelTimer <= 0) {
                if (mode == "setup")
                {
                    StartDefense();
                }
                else
                {
                    NextLevel();
                }
			}
		}
	}

	void NextLevel() {
		CancelInvoke ();
		playing = false;
        score += level * levelPointValue;
		level += 1;

        if (level == 1)
        {
            speechBubble.SetActive(true);
            Invoke("Speech", speechDelay);
            StartSetup();
        } else
        {
            hoursLeftText.text = "Hours Left: ";
            hoursLeftText.text += (12 - (level - 2));
            levelTransitionScreen.SetActive(true);
            mode = "setup";
            Invoke("StartSetup", 2);
        }
	}

    void StartSetup()
    {
        DestroyAllKids();
        levelTransitionScreen.SetActive(false);
        mode = "setup";
        playing = true;
        levelTimer = setupTime;
    }

    void StartDefense()
    {
        float repeatRate = levelTimerMax / 1;
        InvokeRepeating("SpawnKid", 0, repeatRate);
        mode = "defense";
        levelTimer = levelTimerMax;
    }

    public void GameOver()
    {
        playing = false;
        DestroyAllKids();
        gameOverScreen.SetActive(true);
        Text finalScoreText = GameObject.Find("FinalScoreText").GetComponent<Text>();
        finalScoreText.text = "Final Score: ";
        finalScoreText.text += score;
    }

	void SpawnKid() {
		Instantiate (kidPrefab);
	}

    void Speech()
    {
        bubbleText.text = "Every year these darned kids come in here searching for my fabled golden skull!";
        Invoke("Speech2", speechDelay);
    }

    void Speech2()
    {
        bubbleText.text = "Little do they know, they'll be instantly incinerated if they touch it!";
        Invoke("Speech3", speechDelay);
    }

    void Speech3()
    {
        Debug.Log("Speech3");
        bubbleText.text = "For their own good, I have to scare these kids away from here.";
        Invoke("SetBubbleInactive", speechDelay);
    }

    void SetBubbleInactive()
    {
        Debug.Log("Setting bubble inactive");
        speechBubble.SetActive(false);
    }

    void DestroyAllKids()
    {
        CancelInvoke();
        GameObject[] kids = GameObject.FindGameObjectsWithTag("Kid");
        foreach (GameObject kid in kids)
        {
            Destroy(kid);
        }
    }
}
