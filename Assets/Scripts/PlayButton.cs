using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PlayButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler {

	public GameObject gm;

	private Text playText;
	private GameObject mainMenu;
	private GameObject gameScreen;

	// Use this for initialization
	void Start () {
		if (!gm)
			GameObject.Find ("GameManager");
		playText = gameObject.GetComponent <Text> ();
		mainMenu = GameObject.Find ("MainMenu");
		gameScreen = GameObject.Find ("GameScreen");
		gameScreen.SetActive (false);
	}

	// Update is called once per frame
	void Update () {

	}

	public void OnPointerClick(PointerEventData eventData) {
		gm.GetComponent<GameManager>().StartGame ();
		mainMenu.SetActive (false);
		gameScreen.SetActive (true);
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
