using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	public GameObject gm;
    public GameObject mainMenu;
    public GameObject gameScreen;

    private Text playText;

	// Use this for initialization
	void Start () {
		if (!gm) GameObject.Find ("GameManager");
		if (!mainMenu) mainMenu = GameObject.Find ("MainMenu");
		if (!gameScreen) gameScreen = GameObject.Find ("GameScreen");
        playText = gameObject.GetComponent<Text>();
    }

	// Update is called once per frame
	void Update () {

	}

	public void OnPointerClick(PointerEventData eventData) {
        if (gameObject.name == "Play")
        {
            gm.GetComponent<GameManager>().StartGame();
            mainMenu.SetActive(false);
            gameScreen.SetActive(true);
        }
        else if (gameObject.name == "Quit")
        {
            Application.Quit();
        }
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		playText.color = Color.gray;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		playText.color = Color.white;
	}
}
