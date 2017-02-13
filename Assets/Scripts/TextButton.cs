using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TextButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Color defaultColor;
    public Color hoverColor;

    public GameObject gm;

    public GameObject mainMenu;
    public GameObject gameScreen;
    public GameObject gameOverScreen;

    private Text myText;

    // Use this for initialization
    void Start()
    {
        if (!gm) GameObject.Find("GameManager");
        if (!mainMenu) mainMenu = GameObject.Find("MainMenu");
        if (!gameScreen) gameScreen = GameObject.Find("GameScreen");
        if (!gameOverScreen) gameOverScreen = GameObject.Find("GameOverScreen");
        myText = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameObject.name == "PlayAgainButton")
        {
            mainMenu.SetActive(false);
            gameScreen.SetActive(true);
            gameOverScreen.SetActive(false);
            gm.GetComponent<GameManager>().StartGame();
        } else if (gameObject.name == "MainMenuButton")
        {
            mainMenu.SetActive(true);
            gameScreen.SetActive(false);
            gameOverScreen.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myText.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myText.color = defaultColor;
    }
}
