using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;

public class GameController : MonoBehaviour {

	public static GameController instance;
//	public GameObject allCustomerHolder;
	public Customer[] AllCustomers;//initialized by gamehandler on start method
	public TapScript[] AllTaps;
	public Plate[] AllPlates;

	List<int> vacantSeats;
	// Use this for initialization
	float startTime;
	[HideInInspector]public int currentCustomerCount;
	[HideInInspector]public int totalCustomersServed;

	int noOfCustWantDrink,noOfCustWantFood,noOfCustWantBoth;
	int noOfCustLeftDrink,noOfCustLeftFood,noOfCustLeftBoth;//total_cust_want - no_of_cust_served
	int noOfCustWantOnlyDrink,noOfCustWantIncentDrink;
	int noOfCustLeftOnlyDrink,noOfCustLeftIncentDrink;

	int playerLife;
	public int PlayerLife{
		get{
			return playerLife;//PlayerPrefs.GetInt(MyPrefereces.KEY_POWERUP_COUNT_LIFE);
		}
		set{
			playerLife = value;
			UiManager.instance.lifeText.text = ""+playerLife;
//			PlayerPrefs.SetInt(MyPrefereces.KEY_POWERUP_COUNT_LIFE , playerLife);
//			PlayerPrefs.Save();
		}
	}

	int coinsEarned;
	public int AddPlayerCoins{
		get{
			return coinsEarned;
		}
		set{
			if (value == 0) {
				coinsEarned = 0; 
			} else if (value < 0) {
				//reduce coins
				if (coinsEarned >= Mathf.Abs (value)) {
					coinsEarned += value;
					int savedCoins = PlayerPrefs.GetInt (MyPrefereces.KEY_TOTAL_COINS);
					savedCoins += value;
					PlayerPrefs.SetInt (MyPrefereces.KEY_TOTAL_COINS , savedCoins);
					PlayerPrefs.Save ();
				}
			} else {		
				coinsEarned += value;
				int savedCoins = PlayerPrefs.GetInt (MyPrefereces.KEY_TOTAL_COINS);
				savedCoins += value;
				PlayerPrefs.SetInt (MyPrefereces.KEY_TOTAL_COINS , savedCoins);
				PlayerPrefs.Save ();
			}
		}
	}


//	public int Total_Player_Life;
	int waveCount;
	
	public bool isDayMode;
	
	public string GameDataPath;
	public string GameDataJSON;

	void Awake () {
		instance = this;
	}

	// Update is called once per frame
	void Start () {
		GameHandler.instance.currentDay = PlayerPrefs.GetInt(MyPrefereces.KEY_CURRENT_PLAYING_DAY);

		try{ 
			GameDataPath = Application.streamingAssetsPath + "/GameData.json";	
			GameDataJSON = File.ReadAllText(GameDataPath);
			GameHandler.instance.gameData = JsonUtility.FromJson<GameData>(GameDataJSON);
			#if DEBUG
//			Debug.Log("GameData.json loaded \n" + GameDataJSON);
			#endif
		}
		catch(System.Exception e) {
			Debug.LogError (e.Message);
		}

	}

	public void startNewGame(){

		if (GameHandler.itemBeingDragged != null) {
			print("gh itbd is not null");
			if(GameHandler.itemBeingDragged.GetComponent<CupScript>() != null)
				GameHandler.itemBeingDragged.GetComponent<CupScript>().OnEndDrag(null);
			else if(GameHandler.itemBeingDragged.GetComponent<FoodItem>() != null)
				GameHandler.itemBeingDragged.GetComponent<FoodItem>().OnEndDrag(null);
			else if(GameHandler.itemBeingDragged.GetComponent<Plate>() != null)
				GameHandler.itemBeingDragged.GetComponent<Plate>().OnEndDrag(null);

			GameHandler.itemBeingDragged = null;
		}

		if(AllCustomers.Length == 0)
			GameHandler.instance.getAllCustomers ();
		if(AllTaps.Length == 0)
			GameHandler.instance.getAllTap ();
//		if(AllPlates.Length == 0)
			GameHandler.instance.getAllPlates ();
		Table.instance.clarTable ();
		
		GlassContainer.instance.createNewCupsOnRack();

		//destroy old customers if any
		for (int i=0; i<AllCustomers.Length; i++)
			AllCustomers [i].reset ();
		for (int i=0; i<AllTaps.Length; i++)
			AllTaps [i].resetTap ();
		for (int i=0; i<AllPlates.Length; i++)
			AllPlates [i].resetPlateValues ();

		currentCustomerCount = 0;
		totalCustomersServed = 0;
		UiManager.instance.setTotalCutomersServed (GameController.instance.totalCustomersServed);
		AddPlayerCoins = 0;
		UiManager.instance.coinsCollectedText.text = "0";
		UiManager.instance.setLifeBar(1f);
		
		//set requirement probability
		noOfCustWantDrink 	= (int)(GameHandler.instance.maxCustomersLevel  * GameHandler.currDay.OrderJustDrinkPercent);
		noOfCustWantBoth 	= (int)(GameHandler.instance.maxCustomersLevel  * GameHandler.currDay.OrderJustFoodPercent);
		noOfCustWantFood 	= GameHandler.instance.maxCustomersLevel - (noOfCustWantDrink + noOfCustWantBoth);


		noOfCustLeftDrink 	= noOfCustLeftFood = noOfCustLeftBoth = 0;

		noOfCustWantOnlyDrink = getcustWantonlyDrink ();
		noOfCustWantIncentDrink = (noOfCustWantDrink + noOfCustWantBoth) - noOfCustWantOnlyDrink;
		noOfCustLeftOnlyDrink = noOfCustLeftIncentDrink = 0;

		print ("INCENT :: "+noOfCustWantOnlyDrink+" "+noOfCustWantIncentDrink+" "+(noOfCustWantDrink + noOfCustWantBoth));
//		print (""+GameHandler.instance.maxCustomersLevel+" "+noOfCustWantDrink+" "+noOfCustWantFood+" "+noOfCustWantBoth);
		vacantSeats = new List<int> ();
		StopAllCoroutines();//stop all other coroutines
		StartCoroutine(createCustomer());

		GameHandler.instance.startTime = Time.time;
		InvokeRepeating ("calacTime",0.1f,0.1f);
		//set initial power count
		PowerUp.instance.setInitialPowerCount ();
//		if (isDayMode) {
			UiManager.instance.starsHolder.SetActive(true);
			UiManager.instance.setStarsCount (0);
//		}else
//			UiManager.instance.starsHolder.SetActive(false);

		//show incents only when in game
		if(GameHandler.instance.currentDay >= 3)	
			GameHandler.instance.IncentsParentGo.SetActive(true);
		else
			GameHandler.instance.IncentsParentGo.SetActive(false);


	}


	float elapseTime,remTime;
	System.TimeSpan tspan;

	void calacTime(){
		elapseTime = Time.time - GameHandler.instance.startTime;
		remTime = GameHandler.instance.maxTimeLevel - elapseTime;
//		print("Calculating time "+remTime);
		if (remTime > 0) {
			UiManager.instance.timeLeftImg.fillAmount = remTime / GameHandler.instance.maxTimeLevel;
		} else {		
			print("Time over Gameover");
			if(AddPlayerCoins >= GameHandler.currDay.DayCoinGoal){
//				GameController.instance.declareResult ();
				//wait till all customers have gone
				StartCoroutine(endGameWhenAllCustomersGone());
			}else
				GameController.instance.declareGameOver ();
		}
	}

	IEnumerator endGameWhenAllCustomersGone(){
		getAVacantCustomerSeat ();
		print ("waiting to end the game "+currVacantSeats+" "+currentCustomerCount+" "+GameHandler.currDay.CustomerMaxForDay);
		yield return new WaitUntil (()=>(currVacantSeats == 6 && currentCustomerCount == GameHandler.currDay.CustomerMaxForDay ));
		GameController.instance.declareResult ();
	}

	int currVacantSeats = 0;
	int getAVacantCustomerSeat(){
		vacantSeats.Clear ();
		for (int i=0; i<AllCustomers.Length; i++) {
			if (Table.instance.getSeatById (i + 1).isSeatVacant) {
				vacantSeats.Add (i);
			}
		}
		currVacantSeats = vacantSeats.Count ;
			if (currVacantSeats > 0) {
				int rand = Random.Range (0, vacantSeats.Count);
				int index = vacantSeats [rand];
				return index;
			}
				return -1;
	}

	int presetCustomersCount(){
		int count = 0;
		for (int i=0; i<AllCustomers.Length; i++) {
			if (Table.instance.getSeatById (i + 1).isCustomerPresent()) {
				count++;
			}
		}
		return count;
	}

	IEnumerator createCustomer(){
		int vacantSeatno = getAVacantCustomerSeat ();
		print ("create cust "+vacantSeats.Count+" "+vacantSeatno);
		if (vacantSeatno == -1) 
		{
			yield return new WaitForSeconds (5f);//wait for 5 sec thn recheck
			StartCoroutine (createCustomer ());
		}else if (presetCustomersCount() >= GameHandler.currDay.CustomerMaxAtOnce) 
		{
			yield return new WaitForSeconds (5f);//wait for 5 sec thn recheck
			StartCoroutine (createCustomer ());
		}else 
		{
			getRequirementOfthisCust();//get requirement on the basis of probability
			AllCustomers [vacantSeatno].generateCustomer (vacantSeatno+1  , reqbev , reqinc , reqfood);
			Table.instance.getSeatById (vacantSeatno+1).isSeatVacant = false;
			currentCustomerCount++;
			print ("creating cust "+vacantSeatno);

			yield return new WaitForSeconds (Random.Range(GameHandler.currDay.CustomerDelayMin,GameHandler.currDay.CustomerDelayMax));

			if (currentCustomerCount < GameHandler.instance.maxCustomersLevel){
				StartCoroutine (createCustomer ());
			}
		}
	}

	Beverages reqbev,reqinc;
	PlateType reqfood;
	void getRequirementOfthisCust(){
		reqbev 	=  	Beverages.None;
		reqinc  = 	Beverages.None;
		reqfood = 	PlateType.Empty;
		switch(chooseType ()){
			case 0:
				noOfCustLeftDrink ++;
				getDrinkByProb();
				break;
			case 1:
				noOfCustLeftFood++;			
				reqfood = (PlateType)((GameHandler.currDay.FoodTypeLevel == 1) ? 2 : 3);
				break;
			case 2:
				noOfCustLeftBoth++;
				getDrinkByProb();
				reqfood = (PlateType)((GameHandler.currDay.FoodTypeLevel == 1) ? 2 : 3);
				break;
		}
	}

	int chooseDrinkType ()
	{
		int totalDrinks = noOfCustWantDrink + noOfCustWantBoth;
		int value = 0;
		List<int> lst = new List<int> ();
		if (noOfCustLeftOnlyDrink < noOfCustWantOnlyDrink) 
			lst.Add (0);
		if (noOfCustLeftIncentDrink < noOfCustWantIncentDrink) 
			lst.Add (1);
		value = lst[Random.Range(0,lst.Count)];
		lst.Clear ();
		return value;
	}

	void getDrinkByProb(){
		if (chooseDrinkType () == 0) {
			noOfCustLeftOnlyDrink++;
			reqbev = (Beverages)getBeverageAccToDay ();
			reqinc = Beverages.None;
		} else {
			noOfCustLeftIncentDrink++;
			reqbev = (Beverages)getBeverageAccToDay ();
			reqinc = (Beverages)getIncentAccToDay ();
		}
		print ("INCENT Left :: d "+noOfCustLeftOnlyDrink+" i "+noOfCustLeftIncentDrink);
	}

	int chooseType ()
	{
		int value = 0;
		List<int> lst = new List<int> ();
		if (noOfCustLeftDrink < noOfCustWantDrink) 
			lst.Add (0);
	 	if (noOfCustLeftFood < noOfCustWantFood) 
			lst.Add (1);
		if (noOfCustLeftBoth < noOfCustWantBoth)
			lst.Add (2);
		value = lst[Random.Range(0,lst.Count)];
		lst.Clear ();
		return value;
	}




	public void declareResult(){
		StopAllCoroutines();
		CancelInvoke("calacTime");
		StartCoroutine (declareResultCoroutine());
	}

	IEnumerator declareResultCoroutine(){
		print("declareResultCoroutine");
		yield return new WaitForSeconds (1.5f);
		PopUpsManager.instance.openScreen(MyScreens.Result);
		UiManager.instance.replayButton.SetActive(false);
		UiManager.instance.unlockDayPanel.SetActive(false);

		PopUpsManager.instance.allScreens[(int)MyScreens.Result].transform.FindChild("Result_Bg").GetComponent<Image>().sprite = UiManager.instance.GameOverBg[2];//success 100%

		string resultMsg; 
		resultMsg = "Day " + GameHandler.instance.currentDay + " - Task Complete" ;

		PopUpsManager.instance.allScreens[(int)MyScreens.Result].transform.FindChild("ResultMsg").GetComponent<Text>().text = resultMsg;
		//increase day
		GameHandler.instance.currentDay++;
		PlayerPrefs.SetInt(MyPrefereces.KEY_CURRENT_PLAYING_DAY,GameHandler.instance.currentDay);
		PlayerPrefs.Save();
	}

	public void declareGameOver(){
		
		StartCoroutine (declareGameOverCoroutine());		
		CancelInvoke("calacTime");
	}

	IEnumerator declareGameOverCoroutine(){
		yield return new WaitForSeconds (0.5f);
		PopUpsManager.instance.openScreen(MyScreens.Result);
		PopUpsManager.instance.allScreens[(int)MyScreens.Result].transform.FindChild("Result_Bg").GetComponent<Image>().sprite = UiManager.instance.GameOverBg[0];

		UiManager.instance.replayButton.SetActive(true);
		UiManager.instance.unlockDayPanel.SetActive(false);
//		string resultMsg; 
//		resultMsg = "Your Sacked!";
		
		PopUpsManager.instance.allScreens[(int)MyScreens.Result].transform.FindChild("ResultMsg").GetComponent<Text>().text = "";

		StopAllCoroutines();
	}
	
	int getBeverageAccToDay(){	
		switch(GameHandler.currDay.DrinkTypeLevel){
		case 1:
			return 2;
		case 2:
			return Random.Range (2, 4);
		case 3: default:
			return Random.Range (1, 4);
		}		
	}

	int getIncentAccToDay(){	
		int inc = 0;
		switch(GameHandler.currDay.DrinkAddInTypeLevel){
		case 0:			inc = 0;			break;
		case 1:			inc = 4;			break;
		case 2:			inc = Random.Range (4, 6);			break;
		case 3:			inc = Random.Range (4, 7);			break;
		}
		print ("inc selected "+inc);
		return inc;
	}

	int getcustWantonlyDrink(){
		int totalDrinks = noOfCustWantDrink + noOfCustWantBoth;
		totalDrinks = totalDrinks - (int)(totalDrinks * GameHandler.currDay.DrinkAddInPercent);
		return totalDrinks;		
	}
	//menu play game click or from story panel
	public void loadDay(){
		isDayMode = true;

		//load current day values from json gamedata
		if(GameHandler.instance.currentDay == 1)
			PlayerLife = GameHandler.instance.gameData.PlayerLives;

		GameHandler.currDay = GameHandler.instance.gameData.Days [GameHandler.instance.currentDay - 1];
		print("Load Day " + GameHandler.instance.currentDay+",time:"+GameHandler.currDay.DayCompleteTime);

		GameHandler.instance.totalCoinstoCollect = GameHandler.currDay.DayCoinGoal;
		GameHandler.instance.maxCustomersLevel = GameHandler.currDay.CustomerMaxForDay;//maxCust;
		GameHandler.instance.maxTimeLevel = GameHandler.currDay.DayCompleteTime;

		PopUpsManager.instance.openScreen (MyScreens.Gameplay);
		GameController.instance.startNewGame ();
				
		UiManager.instance.setTask ("Collect "+GameHandler.currDay.DayCoinGoal+" gold as quikly as possible.");

//		if (GameHandler.instance.currentDay > 7)
			UiManager.instance.starsHolder.SetActive (true);
//		else
//			UiManager.instance.starsHolder.SetActive (false);		
	}

}
