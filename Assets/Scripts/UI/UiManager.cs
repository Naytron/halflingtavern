using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;


public class UiManager : MonoBehaviour {
	
	public static UiManager instance;
	public Text custServedText,coinsCollectedText;
	public Text TimeLeftText,taskText,lifeText;
	public Image timeLeftImg;
	public GameObject replayButton,explainWizard;
	public GameObject restartWarnPopUp;
	public GameObject CustCoinPrefab,starsHolder,flowCoinprefab;
	public Image[] GameplayStars;
	public Sprite[] GameOverBg;
	public Text shopCurrGoldTex;
	public GameObject unlockDayPanel;
	bool showHints = true;//to show help wizard when wrong done 

	// Use this for initialization
	void Awake () {
		instance = this;
	}
	void Start () {
	}
	// Update is called once per frame
//	void Update () {

//		TestingText.text = "PConn. " + PhotonNetwork.connected + " fblogin " + MyFbManager.instance.isUserLogInFb;
		
//	}
	public void setTotalCutomersServed(int custServed){
		custServedText.text = custServed.ToString();
	}

	public void setTask(string task){
		taskText.text = "";
		if(!string.IsNullOrEmpty(task)){
			taskText.text = "Day "+GameHandler.instance.currentDay+" Task : "+task;
			
			UiManager.instance.explainWizard.SetActive(true);
			UiManager.instance.explainWizard.GetComponent<AutoOffObject>().waitTime = 2.0f;		
			explainWizard.GetComponentInChildren<Text>().text = ""+task;	
		}
	}

	public void setStarsCount(int starCollected){
		for(int i=0;i<GameplayStars.Length;i++){
			if(i<starCollected)
				GameplayStars[i].enabled = false;
			else
				GameplayStars[i].enabled = true;

		}
	}

	public void showMsgWizard(float forTime,string msgToShow){
		if (showHints && !GameHandler.instance.isPlayingTutorial) {
			//enable task explainary wizard
			UiManager.instance.explainWizard.SetActive (true);
			UiManager.instance.explainWizard.GetComponent<AutoOffObject> ().waitTime = forTime;		
			explainWizard.GetComponentInChildren<Text> ().text = "" + msgToShow;	
		}
	}

	public void OnShowHintToggle(bool isOn){
		showHints = isOn;
	}

	public void generateCollectableCoins(int coinValue,int seatNo){
		//print ("Gen coin at "+seatNo);
		GameObject coins = Instantiate (CustCoinPrefab) as GameObject;
		//set parent
		coins.transform.SetParent (Table.instance.getSeatById(seatNo).transform);
		coins.transform.localPosition = Vector3.zero;
		coins.transform.localScale = Vector3.one;
		CustomerCoins cc = coins.GetComponent<CustomerCoins> ();
		cc.setCoinValueAndSeatId (coinValue,seatNo);

	}


	public void setLifeBar(float value){
//		lifeBarImg.fillAmount = value;
	}


	public void playGameClicked(){
		GameHandler.instance.currentDay = 1;
		GameController.instance.loadDay ();
	}

	public void loadNextDayStory(){
		//if next level is unlock then play day
		int unlockDay = PlayerPrefs.GetInt(MyPrefereces.KEY_CURRENT_UNLOCKED_DAY);
		if(unlockDay >=  GameHandler.instance.currentDay ){
			PopUpsManager.instance.openScreen (MyScreens.StoryBoard);
		}else{
			//if has coins to unlock the day
			int coinNeeded = GameHandler.instance.gameData.Days[GameHandler.instance.currentDay - 1].UnlockDayCoin;				
			if (PlayerPrefs.GetInt (MyPrefereces.KEY_TOTAL_COINS) >= coinNeeded) {
				UiManager.instance.unlockDayPanel.SetActive (true);	
				string msg = "Unlock day "+GameHandler.instance.currentDay+" for "+coinNeeded+" Coins\n";
				UiManager.instance.unlockDayPanel.transform.FindChild ("ConfrimUnlockMsg").GetComponent<Text> ().text = msg;
			}
			else
				PopUpsManager.instance.showSingleButtonPopUp ("Insufficient Coins to unlock day \n You have "+PlayerPrefs.GetInt (MyPrefereces.KEY_TOTAL_COINS)+" Coins");
		}
	}

	public void unlockConfirmYes(){
		//unlock the day,reduce coins
		int coinNeeded = GameHandler.instance.gameData.Days[GameHandler.instance.currentDay - 1].UnlockDayCoin;
		GameController.instance.AddPlayerCoins = -coinNeeded;
		PlayerPrefs.SetInt(MyPrefereces.KEY_CURRENT_UNLOCKED_DAY , GameHandler.instance.currentDay);
		PlayerPrefs.Save ();
		UiManager.instance.unlockDayPanel.SetActive(false);	
	}
	public void unlockConfirmNo(){
		UiManager.instance.unlockDayPanel.SetActive(false);
	}

	public void pauseGameClicked(){
		Time.timeScale = 0;
		PopUpsManager.instance.openScreen(MyScreens.GamePause);
	}

	public void resumeGameClicked(){
		Time.timeScale = 1;
		PopUpsManager.instance.openScreen(MyScreens.Gameplay);
		PowerUp.instance.setInitialPowerCount ();
	}

	//Menu
	public void unlimitedGameClicked(){
		//load levels screen		
//		PopUpsManager.instance.openScreen (MyScreens.LevelsScreen);
//		PopUpsManager.instance.openScreen (MyScreens.Gameplay);
//		GameController.instance.startNewGame ();
		//load unlimited game
		GameController.instance.isDayMode = false;
		PopUpsManager.instance.openScreen (MyScreens.Gameplay);
		GameController.instance.startNewGame ();	
		UiManager.instance.setTask ("");
	}

	public void GetGoldClicked(){
		PopUpsManager.instance.openScreen (MyScreens.GetGold);		
	}

	public void ShopClicked(){
		PopUpsManager.instance.openScreen (MyScreens.Shop);		
		shopCurrGoldTex.text = "Current Gold : " + PlayerPrefs.GetInt (MyPrefereces.KEY_TOTAL_COINS);
	}
	public void updateShopScreenValue(){
	
	}

	
	//Back buttons
	public void BackToMenuClicked(){
		PopUpsManager.instance.openScreen(MyScreens.Menu);
		if(Time.timeScale == 0){
			StopAllCoroutines();
			Time.timeScale = 1;
		}
	}
	public void BackToShopClicked(){
		ShopClicked ();
	}
	public void BackFromShopClicked(){
		if(Time.timeScale == 0)
			PopUpsManager.instance.openScreen(MyScreens.GamePause);	
		else
			PopUpsManager.instance.openScreen(MyScreens.Menu);	
	}
	
	public void menuWarningPopUp(bool isShow){
		restartWarnPopUp.SetActive(isShow);
	}


//	public void SoundToggleClicked(){
//		int sound = PlayerPrefs.GetInt (MyPrefs.KEY_IS_SOUNDON,1);
//		if (sound == 0) {
//			sound = 1;
//			PlayerPrefs.SetInt (MyPrefs.KEY_IS_SOUNDON, sound);
//			PlayerPrefs.Save ();
//			BlockSoundimg.enabled = false;
//			AudioManager.instance.playSound();
//		} else {
//			sound = 0;
//			PlayerPrefs.SetInt (MyPrefs.KEY_IS_SOUNDON, sound);
//			PlayerPrefs.Save ();
//			BlockSoundimg.enabled = true;
//			AudioManager.instance.stopSound();
//		}
//	}
//	public void MusicToggleClicked(){
//		int music = PlayerPrefs.GetInt (MyPrefs.KEY_IS_MUSICON,1);
//		if (music == 0) {
//			music = 1;
//			PlayerPrefs.SetInt (MyPrefs.KEY_IS_MUSICON, music);
//			PlayerPrefs.Save ();
//			BlockMusicimg.enabled = false;
//			AudioManager.instance.playMusic();
//		} else {
//			music = 0;
//			PlayerPrefs.SetInt (MyPrefs.KEY_IS_MUSICON, music);
//			PlayerPrefs.Save ();
//			BlockMusicimg.enabled = true;
//			AudioManager.instance.stopMusic();
//		}
//	}
//
//	public void setSoundimages(){
//		
//		int sound = PlayerPrefs.GetInt (MyPrefs.KEY_IS_SOUNDON,1);
//		int music = PlayerPrefs.GetInt (MyPrefs.KEY_IS_MUSICON,1);
//		if (sound == 1)
//			BlockSoundimg.enabled = false;
//		else
//			BlockSoundimg.enabled = true;
//		if (music == 1)
//			BlockMusicimg.enabled = false;
//		else
//			BlockMusicimg.enabled = true;
//
//	
//	}
	//Exit Game
//	public void exitYesClicked(){
//		PopUpsManager.instance.allScreens [(int)MyScreens.Exit].SetActive (false);
//		Application.Quit ();
//		
//		//play audio
//		AudioManager.instance.playButtonClickSound ();
//	}
//	public void exitNoClicked(){
//		Time.timeScale = 1.0f;
//		PopUpsManager.instance.allScreens [(int)MyScreens.Exit].SetActive (false);
//		
//		//play audio
//		AudioManager.instance.playButtonClickSound ();
//	}


}
