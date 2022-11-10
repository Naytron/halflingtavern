using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {

	public static GameHandler instance;
	public static GameObject itemBeingDragged = null;
	public const int TotalCharacters = 8;

	public Sprite[] allPlates;
	public Sprite[] Beverages;
	public Sprite[] WaitDotSprite;
	public GameObject GamePanelBg;
	public GameObject IncentsParentGo;
	public GameObject emptyPlatePrefab;

	public float startTime;
//	public int totalLevelCustomers;//no of customers to be served in this level to win it
	public int maxCustomersLevel;
	public float maxTimeLevel;
	public float totalCoinstoCollect;
//	public bool isTaskCustomer;

	public int currentDay = 1;
	public static Day currDay;

	public bool isPlateAnimating;
	public bool isPlayingTutorial;

	public Canvas mainCanvas;
	public GameData gameData;

	
	GameObject slideObject;
	int seatPosition;

	void Awake(){
		instance = this;
	}

	public void getAllCustomers(){
		GameController.instance.AllCustomers = GetComponentsInChildren<Customer> ();
	}

	public void getAllTap(){
		GameController.instance.AllTaps = GetComponentsInChildren<TapScript> ();
	}
	public void getAllPlates(){
		GameController.instance.AllPlates = GetComponentsInChildren<Plate> ();
	}

	public void reduceLife(){
		//reduce player life by 1
//		if(GameController.instance.PlayerLife > 1)
		GameController.instance.PlayerLife--;
			//set life bar
		float val;
//		val = (float)GameController.instance.playerLife/GameController.instance.Total_Player_Life;
//		UiManager.instance.setLifeBar(val);
		if(GameController.instance.PlayerLife <= 0){
			print("player life finished game over");
			GameController.instance.declareGameOver();
		}
	}

}
