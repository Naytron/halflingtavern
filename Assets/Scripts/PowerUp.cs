using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum PowerType{
	Speed		=	0,
	Double_Coin	=	1,
	Boot		=	2,
	Extra_Life	=	3,
}

public class PowerUp : MonoBehaviour {
	public static PowerUp instance;

	public bool isBootActive = false;
	public bool isDoubleCoinsActive = false;
	public bool isSpeedPowerActive = false;

	[SerializeField] GameObject[] AllPowerUp;//speed,coin,boot,life

	void Awake(){
		instance = this;
	}

	public void setInitialPowerCount(){
		int speedCount = PlayerPrefs.GetInt (MyPrefereces.KEY_POWERUP_COUNT_SPEED);
		if (speedCount <= 0) {
			AllPowerUp [(int) PowerType.Speed].GetComponent<Button> ().enabled = false;
			AllPowerUp [(int) PowerType.Speed].GetComponentInChildren<Text> ().text = "";
		} else {
			AllPowerUp [(int) PowerType.Speed].GetComponent<Button> ().enabled = true;
			AllPowerUp [(int) PowerType.Speed].GetComponentInChildren<Text> ().text = ""+speedCount;
		}

		int coinCount = PlayerPrefs.GetInt (MyPrefereces.KEY_POWERUP_COUNT_DOUBLE_COIN);
		if (coinCount <= 0) {
			AllPowerUp [(int) PowerType.Double_Coin].GetComponent<Button> ().enabled = false;
			AllPowerUp [(int) PowerType.Double_Coin].GetComponentInChildren<Text> ().text = "";
		} else {
			AllPowerUp [(int) PowerType.Double_Coin].GetComponent<Button> ().enabled = true;
			AllPowerUp [(int) PowerType.Double_Coin].GetComponentInChildren<Text> ().text = ""+coinCount;
		}

		int bootCount = PlayerPrefs.GetInt (MyPrefereces.KEY_POWERUP_COUNT_BOOT);
		if (bootCount <= 0) {
			AllPowerUp [(int)PowerType.Boot].GetComponent<Button> ().enabled = false;
			AllPowerUp [(int)PowerType.Boot].GetComponentInChildren<Text> ().text = "";
		} else {
			AllPowerUp [(int)PowerType.Boot].GetComponent<Button> ().enabled = true;
			AllPowerUp [(int)PowerType.Boot].GetComponentInChildren<Text> ().text = ""+bootCount;
		}

		int heartCount = PlayerPrefs.GetInt (MyPrefereces.KEY_POWERUP_COUNT_LIFE);
		if (heartCount <= 0) {
			AllPowerUp [(int)PowerType.Extra_Life].GetComponent<Button> ().enabled = false;
			AllPowerUp [(int)PowerType.Extra_Life].GetComponentInChildren<Text> ().text = "";
		} else {
			AllPowerUp [(int)PowerType.Extra_Life].GetComponent<Button> ().enabled = true;
			AllPowerUp [(int)PowerType.Extra_Life].GetComponentInChildren<Text> ().text = ""+heartCount;
		}

		for(int i=0;i<AllPowerUp.Length;i++){
			AllPowerUp[i].GetComponent<Image>().color = Color.white;
		}
		isBootActive = false;
		isDoubleCoinsActive = false;
		isSpeedPowerActive = false;
	}

	void increasePowerCount(PowerType type,int incBy){
		int currentValue = 0;
		switch(type){
		case PowerType.Speed:
			currentValue = PlayerPrefs.GetInt(MyPrefereces.KEY_POWERUP_COUNT_SPEED);
			currentValue += incBy;
			PlayerPrefs.SetInt(MyPrefereces.KEY_POWERUP_COUNT_SPEED  , currentValue);
			break;
		case PowerType.Double_Coin:
			currentValue = PlayerPrefs.GetInt(MyPrefereces.KEY_POWERUP_COUNT_DOUBLE_COIN);
			currentValue += incBy;
			PlayerPrefs.SetInt(MyPrefereces.KEY_POWERUP_COUNT_DOUBLE_COIN  , currentValue);
			break;
		case PowerType.Boot:
			currentValue = PlayerPrefs.GetInt(MyPrefereces.KEY_POWERUP_COUNT_BOOT);
			currentValue += incBy;
			PlayerPrefs.SetInt(MyPrefereces.KEY_POWERUP_COUNT_BOOT  , currentValue);
			break;
		case PowerType.Extra_Life:
			currentValue = PlayerPrefs.GetInt(MyPrefereces.KEY_POWERUP_COUNT_LIFE);
			currentValue += incBy;
			PlayerPrefs.SetInt(MyPrefereces.KEY_POWERUP_COUNT_LIFE  , currentValue);
			break;
		}
		PlayerPrefs.Save ();
	}
	//decrease power by one
	void decreasePowerCount(PowerType type){
		int currentValue = 0;
		switch(type){
		case PowerType.Speed:
			currentValue = PlayerPrefs.GetInt(MyPrefereces.KEY_POWERUP_COUNT_SPEED);
			currentValue -= 1;
			PlayerPrefs.SetInt(MyPrefereces.KEY_POWERUP_COUNT_SPEED  , currentValue);
			break;
		case PowerType.Double_Coin:
			currentValue = PlayerPrefs.GetInt(MyPrefereces.KEY_POWERUP_COUNT_DOUBLE_COIN);
			currentValue -= 1;
			PlayerPrefs.SetInt(MyPrefereces.KEY_POWERUP_COUNT_DOUBLE_COIN  , currentValue);
			break;
		case PowerType.Boot:
			currentValue = PlayerPrefs.GetInt(MyPrefereces.KEY_POWERUP_COUNT_BOOT);
			currentValue -= 1;
			PlayerPrefs.SetInt(MyPrefereces.KEY_POWERUP_COUNT_BOOT  , currentValue);
			break;
		case PowerType.Extra_Life:
			currentValue = PlayerPrefs.GetInt(MyPrefereces.KEY_POWERUP_COUNT_LIFE);
			currentValue -= 1;
			PlayerPrefs.SetInt(MyPrefereces.KEY_POWERUP_COUNT_LIFE  , currentValue);
			break;
		}
		PlayerPrefs.Save ();
		setInitialPowerCount ();
	}

	public void activatePower(int typeVal){
		PowerType type = (PowerType)typeVal;
		switch(type){
		case PowerType.Speed:
			isSpeedPowerActive = true;
			Time.timeScale = 1f/2;
			AllPowerUp[(int)PowerType.Speed].GetComponent<Image>().color = new Color(0f,1f,0f);
			Invoke("deActivateSpeedPower",10f/2);
			break;
		case PowerType.Double_Coin:
			isDoubleCoinsActive = true;
			AllPowerUp[(int)PowerType.Double_Coin].GetComponent<Image>().color = new Color(0f,1f,0f);
			Invoke("deActivateDoubleCoinsPower",10f);//deactivate after 30 sec
			break;
		case PowerType.Boot:
			isBootActive = true;
			AllPowerUp[(int)PowerType.Boot].GetComponent<Image>().color = new Color(0f,1f,0f);
			break;
		case PowerType.Extra_Life:
			GameController.instance.PlayerLife += 1;
			decreasePowerCount (PowerType.Extra_Life);
			break;
		}
	}

	public void deActivateBootPower(){
		isBootActive = false;
		decreasePowerCount (PowerType.Boot);
		AllPowerUp[(int)PowerType.Boot].GetComponent<Image>().color = Color.white;
	}
	public void deActivateDoubleCoinsPower(){
		isDoubleCoinsActive = false;
		decreasePowerCount (PowerType.Double_Coin);
		AllPowerUp[(int)PowerType.Double_Coin].GetComponent<Image>().color = Color.white;
	}
	public void deActivateSpeedPower(){
		Time.timeScale = 1.0f;
		isSpeedPowerActive = false;
		decreasePowerCount (PowerType.Speed);
		AllPowerUp[(int)PowerType.Speed].GetComponent<Image>().color = Color.white;
	}


	//purchase power

	public void purchasePower(int power){
		PowerType powerType = (PowerType)power;

		int currCoins = PlayerPrefs.GetInt (MyPrefereces.KEY_TOTAL_COINS);
		if (powerType == PowerType.Double_Coin) {
			if (currCoins >= 100) {
				PlayerPrefs.SetInt (MyPrefereces.KEY_TOTAL_COINS, currCoins - 100);
				PlayerPrefs.Save ();
				increasePowerCount (powerType, 1);
				UiManager.instance.shopCurrGoldTex.text = "Current Gold : " + PlayerPrefs.GetInt (MyPrefereces.KEY_TOTAL_COINS);
			} else {
				print ("Insufficient Coins to make purchase");
				PopUpsManager.instance.showSingleButtonPopUp ("Insufficient Coins to make purchase \n You have "+PlayerPrefs.GetInt (MyPrefereces.KEY_TOTAL_COINS)+" Coins");
			}
		} else {
			if (currCoins >= 500) {
				PlayerPrefs.SetInt (MyPrefereces.KEY_TOTAL_COINS, currCoins - 500);
				PlayerPrefs.Save ();
				increasePowerCount (powerType, 1);
				UiManager.instance.shopCurrGoldTex.text = "Current Gold : " + PlayerPrefs.GetInt (MyPrefereces.KEY_TOTAL_COINS);
			} else {
				print ("Insufficient Coins to make purchase");
				PopUpsManager.instance.showSingleButtonPopUp ("Insufficient Coins to make purchase \n You have "+PlayerPrefs.GetInt (MyPrefereces.KEY_TOTAL_COINS)+" Coins");
			}
		}


	}
}
