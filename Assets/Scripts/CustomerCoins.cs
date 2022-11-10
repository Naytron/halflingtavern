using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CustomerCoins : MonoBehaviour , IPointerClickHandler {

	int seatNo;
	int coinValue;
	bool isCollected;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setCoinValueAndSeatId(int coinVal, int seatId){
		seatNo = seatId;
		coinValue = coinVal;
	}

	public void OnPointerClick (PointerEventData eventData)
	{ 
		if (isCollected)
			return;

//		print ("Coins Collected "+coinValue);
		//add game coins and total coins
		GameController.instance.AddPlayerCoins = coinValue;

//		int savedCoins = PlayerPrefs.GetInt (MyPrefereces.KEY_TOTAL_COINS);
//		savedCoins += coinValue;
		isCollected = true;
//		PlayerPrefs.SetInt (MyPrefereces.KEY_TOTAL_COINS , savedCoins);
//		PlayerPrefs.Save ();

		UiManager.instance.coinsCollectedText.text = GameController.instance.AddPlayerCoins + "";

		if (!GameHandler.instance.isPlayingTutorial) {

			GameObject coinFlow = Instantiate (UiManager.instance.flowCoinprefab) as GameObject;
			coinFlow.transform.SetParent (GameObject.Find ("Canvas").transform);
			coinFlow.transform.localScale = Vector3.one;
			coinFlow.transform.GetComponent<RectTransform> ().anchoredPosition = transform.parent.GetComponent<RectTransform> ().anchoredPosition;
			SoundManager.ins.playOneShotClip(AudioClipType.CollectGold);
			Destroy (coinFlow, 1.2f);
			Destroy (this.gameObject, 1.2f);

		}else
			Destroy (this.gameObject);


		//set seat as vacant
		if (!GameHandler.instance.isPlayingTutorial) {
			Table.instance.getSeatById (seatNo).isSeatVacant = true;
			checkForStars ();
		}
		else
			TutorialController.ins.nextButtonClicked ();

	}

	void checkForStars(){
		if (GameController.instance.AddPlayerCoins >= GameHandler.currDay.DayCoinGoal3star) {
			print ("Got 3 Stars");
			UiManager.instance.setStarsCount (3);
		} else if (GameController.instance.AddPlayerCoins >= GameHandler.currDay.DayCoinGoal2star)  {
			print ("Got 2 Star");
			UiManager.instance.setStarsCount (2);
		} else if (GameController.instance.AddPlayerCoins >= GameHandler.currDay.DayCoinGoal)  {
			print ("Got 1 Star");
			UiManager.instance.setStarsCount (1);
		} else
			UiManager.instance.setStarsCount (0);
	}


}
