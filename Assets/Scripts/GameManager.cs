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

    public GameObject introScreen;
    public GameObject tutorialScreen;
    public GameObject gameScreen;
    public GameObject levelTransitionScreen;
    public GameObject gameOverScreen;

    public GameObject setupPanel;

    public TrapManager tm;
    public KidSpawner kidSpawner;
    public Slider timeSlider;
    public Text levelText;
    public Text scoreText;
    public Text middleText;

    public LivesDisplay livesDisplay;

    private float levelTimer;
    private Text modeText;
    private Player playerScript;

    private List<TrapTrigger> trapTriggers;

    // Use this for initialization
    void Start () {
        score = 0;

        if (!player) player = GameObject.Find("Player");
        playerScript = player.GetComponent<Player>();

        if (!introScreen) introScreen = GameObject.Find("IntroScreen");
        if (!tutorialScreen) tutorialScreen = GameObject.Find("TutorialScreen");
        if (!gameScreen) gameScreen = GameObject.Find("GameScreen");
        if (!levelTransitionScreen) levelTransitionScreen = GameObject.Find("LevelTransitionScreen");
        if (!gameOverScreen) gameOverScreen = GameObject.Find("GameOverScreen");

        if (!setupPanel) setupPanel = GameObject.Find("SetupPanel");

        if (!tm) tm = GameObject.Find("TrapManager").GetComponent<TrapManager>();
        if (!kidSpawner) kidSpawner = GameObject.Find("KidSpawner").GetComponent<KidSpawner>();
        if (!timeSlider) timeSlider = GameObject.Find("TimeSlider").GetComponent<Slider>();
        timeSlider.value = 1;
        modeText = timeSlider.GetComponentInChildren<Text>();
        if (!levelText) levelText = GameObject.Find("LevelText").GetComponent<Text>();
        if (!scoreText) scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
        if (!middleText) middleText = GameObject.Find("MiddleText").GetComponent<Text>();

        if (!livesDisplay) livesDisplay = GameObject.Find("Lives").GetComponent<LivesDisplay>();

        gameScreen.SetActive(true);

        floor2_y = GameObject.Find("Floor2").transform.position.y - 1.5f;
        floor3_y = GameObject.Find("Floor3").transform.position.y - 1.5f;

        string[] trapTriggerNames = new string[] { "11", "12", "13", "21", "22", "23", "31", "32", "33" };
        trapTriggers = new List<TrapTrigger>();
        foreach (string s in trapTriggerNames)
        {
            trapTriggers.Add(GameObject.Find(s).GetComponent<TrapTrigger>());
        }

        introScreen.SetActive(false);
        gameScreen.SetActive(false);
        levelTransitionScreen.SetActive(false);
        gameOverScreen.SetActive(false);

        levelTimer = setupTimerMax;
    }

	public void StartGame() {
        tm.ResetAllTraps();
        livesDisplay.ResetLives();
		level = 0;
        score = 0;
        introScreen.SetActive(true);
	}
	
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

    public void ShowTutorial()
    {
        introScreen.SetActive(false);
        tutorialScreen.SetActive(true);
    }

	public void NextLevel() {
        playing = false;
		CancelInvoke ();
        level += 1;
        score += level * levelPointValue;
        tm.LoadInventory();
        playerScript.ResetMe();
        kidSpawner.PrepareLevel(level);

        if (level == 1)
        {
            tutorialScreen.SetActive(false);
            gameScreen.SetActive(true);
            StartSetup();
        }
        else if (level == 12)
        {
            GameOver("The sun began to rise, and the old vampire went to sleep, grumbling about another successful year.");
        }
        else
        {
            levelTransitionScreen.SetActive(true);
            Text hoursLeftText = GameObject.Find("HoursLeftText").GetComponent<Text>();
            hoursLeftText.text = "Hours Left: ";
            hoursLeftText.text += (12 - (level - 2));
            Invoke("StartSetup", 2);
        }
	}

    void StartSetup()
    {
        mode = "setup";
        levelTimer = setupTimerMax;
        playing = true;
        EnableMiddleText("SETUP");
        levelTransitionScreen.SetActive(false);
        setupPanel.SetActive(true);
        foreach (TrapTrigger t in trapTriggers)
        {
            t.Render();
        }
    }

    void StartDefense()
    {
        mode = "defense";
        levelTimer = levelTimerMax;
        EnableMiddleText("DEFENSE");
        setupPanel.SetActive(false);
        foreach (TrapTrigger t in trapTriggers)
        {
            t.rend.enabled = false;
        }
    }

    void EnableMiddleText(string str)
    {
        middleText.text = str;
        middleText.enabled = true;
        Invoke("DisableMiddleText", 1.5f);
    }

    void DisableMiddleText()
    {
        middleText.enabled = false;
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

    void Clear()
    {
        DestroyAll("Kid");
        DestroyAll("Trap");
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
