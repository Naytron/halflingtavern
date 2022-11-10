using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Customer : MonoBehaviour {

	public int customerId;
	public int customerSeatId;
	[SerializeField] GameObject[] AllChilds;
	public CustomerType currentCustomerType;

	public RequirementCloud requirementCloud;
	Animator animator;
	GameObject customerCloud;
	GameObject customerWaitImgsParent;
	[HideInInspector] public CupType myCuptype;

	public bool isPresent;
	public int moneyToGive;

//	public float waitingTime;
	public int waitDotsReduced;
	Image[] waitDots;
	float startTime;
	bool isCalTime;	
	float remTime;
	float totalTime = 30f;//50
	float secondsPerPoint;

	int demandCount;

	[HideInInspector]public Beverages currReqBeverage,currReqIncent;
	[HideInInspector]public PlateType currReqPlatetype;

	public bool isTutorialCust;
	

	// Use this for initialization
	void Start () {
		if(isTutorialCust)
			setTutorialvalues ();
	}

	void setTutorialvalues(){
		
		isPresent = true;
//		currentCustomerType = CustomerType.Dwarf01;	
		if(currentCustomerType == CustomerType.Dwarf01){
			myCuptype = CupType.WoodenCup;
			//get drink
			currReqBeverage = Beverages.Grape;
			currReqIncent = Beverages.None;
			currReqPlatetype = PlateType.Empty;
		}else{
			myCuptype = CupType.MetalCup;
			//get drink
			currReqBeverage = Beverages.Grape;
			currReqIncent = Beverages.Watergress;
			currReqPlatetype = PlateType.Empty;
		}

		moneyToGive = 0;
		requirementCloud 		= AllChilds [(int)currentCustomerType - 1].transform.GetComponentInChildren<RequirementCloud> ();
		animator				= AllChilds [(int)currentCustomerType - 1].transform.GetComponentInChildren<Animator> ();
		customerCloud 			= AllChilds [(int)currentCustomerType - 1].transform.FindChild ("Cloud").gameObject;
		customerWaitImgsParent 	= AllChilds [(int)currentCustomerType - 1].transform.FindChild ("WaitingTime").gameObject;
	}

	public void generateCustomer(int seatId , Beverages rBev,Beverages rIncent, PlateType rPlate){
		customerSeatId = seatId;
		createACustomerWithReq ();
		createRequirement (rBev,rIncent,rPlate);
		//set customer wait time		
		totalTime = GameHandler.currDay.CustomerWaitTime;
		secondsPerPoint = totalTime / 5;
		waitDotsReduced = 0;
	}

	public void reset(){
		isPresent = false;
		moneyToGive = 0;
		isCalTime = false;
		currReqIncent = Beverages.None;
		currReqBeverage = Beverages.None;
		currReqPlatetype = PlateType.Empty;
		for (int i=0; i<GameHandler.TotalCharacters; i++) {
				AllChilds [i].SetActive (false);
		}
	}



	void createACustomerWithReq(){
		isPresent = true;
		//create a customer character
		if(GameHandler.currDay.CustomerTypeLevel == 1)
			currentCustomerType = (CustomerType)Random.Range (1f,5f);//small peoples
		else if(GameHandler.currDay.CustomerTypeLevel == 2)
			currentCustomerType = (CustomerType)Random.Range (5f,9f);//large peoples
		else
			currentCustomerType = (CustomerType)Random.Range (1f,9f);//small and large


		for (int i=0; i<GameHandler.TotalCharacters; i++) {
			if((int)currentCustomerType-1 == i)
				AllChilds [i].SetActive (true);
			else
				AllChilds [i].SetActive (false);
		}
		//get wait images of this customer
		waitDots = AllChilds [(int)currentCustomerType - 1].transform.FindChild ("WaitingTime").GetComponentsInChildren<Image> ();
		deactivateAllDotimages (false);
		requirementCloud 		= AllChilds [(int)currentCustomerType - 1].transform.GetComponentInChildren<RequirementCloud> ();
		animator				= AllChilds [(int)currentCustomerType - 1].transform.GetComponentInChildren<Animator> ();
		customerCloud 			= AllChilds [(int)currentCustomerType - 1].transform.FindChild ("Cloud").gameObject;
		customerWaitImgsParent 	= AllChilds [(int)currentCustomerType - 1].transform.FindChild ("WaitingTime").gameObject;

		if (currentCustomerType > CustomerType.Halfling02)
			myCuptype = CupType.MetalCup;
		else
			myCuptype = CupType.WoodenCup;

		//create random req random exclu upper limit
		demandCount = 1;//Random.Range (1,4);
	}

	//creates a random requirement for a customer
	void createRequirement(Beverages rBev,Beverages rIncent, PlateType rPlate)
	{
		demandCount--;
		currReqBeverage = rBev;
		currReqIncent = rIncent;
		currReqPlatetype = rPlate;
//		print ("create cust req "+currReqBeverage+" "+currReqIncent+" "+currReqPlatetype);

		if(requirementCloud==null)			
			requirementCloud = AllChilds [(int)currentCustomerType - 1].transform.GetComponentInChildren<RequirementCloud> ();
		//set these requirement on to customers requirement cloud
		requirementCloud.SetRequirement (currReqBeverage,currReqIncent,currReqPlatetype);
		//start calculating time
		deactivateAllDotimages (true);
		startTime = Time.time;
		isCalTime = true;
		moneyToGive = 0;
	}

	public void addWaitTime(float time){
		startTime += time;
	}

	void calculateTime(){
		remTime = totalTime - (Time.time - startTime);
		if (remTime > 0.01f) {
			int  waitDotsRedCount = 0;
			for (int i=0; i<waitDots.Length; i++) {
				if ((remTime / secondsPerPoint) + 1 >= i)
					waitDots [i].sprite = GameHandler.instance.WaitDotSprite [1];
				else {
					waitDots [i].sprite = GameHandler.instance.WaitDotSprite [0];
					waitDotsRedCount++;
				}
			}
			if (waitDotsRedCount > waitDotsReduced)
				waitDotsReduced = waitDotsRedCount;
			
		} else {
			//check if customer is served for half order
			if(moneyToGive > 0){
				print ("Half order amount generate");
				UiManager.instance.generateCollectableCoins(moneyToGive,customerSeatId);
			}else
				Table.instance.getSeatById(customerSeatId).isSeatVacant = true;

			isCalTime = false;
			isPresent = false;
			for (int i=0; i<GameHandler.TotalCharacters; i++) {
					AllChilds [i].SetActive (false);
			}
			//reduce life
			GameHandler.instance.reduceLife();
		}
	}

	void deactivateAllDotimages(bool isActivate){
		for (int i=0; i<waitDots.Length; i++) {
			if(isActivate)
				waitDots [i].sprite = GameHandler.instance.WaitDotSprite [1];
			else
				waitDots [i].sprite = GameHandler.instance.WaitDotSprite [0];
		}
	}	
	// Update is called once per frame
	void Update () {
		if (isCalTime)
			calculateTime ();	
	}

	void myAllReqFulfilled(bool isPlateAtlast){
		//remove cloud and waiting time of this customer
		
		isCalTime = false;
		customerCloud.SetActive (false);
		customerWaitImgsParent.SetActive (false);
		//set customers served
		GameController.instance.totalCustomersServed ++;
		if (!isTutorialCust)
			checkForStars ();

		UiManager.instance.setTotalCutomersServed (GameController.instance.totalCustomersServed);

		if (isPlateAtlast)
			Invoke ("enableCloudWaitImages", 2f);
		else
			Invoke ("enableCloudWaitImages", 8f);
		
		if (isTutorialCust) {
			TutorialController.ins.closeAllMsgs();
			TutorialController.ins.Invoke("nextButtonClicked",8f);
		}
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

	void enableCloudWaitImages(){
		customerCloud.SetActive (true);
		customerWaitImgsParent.SetActive (true);
		isPresent = false;
		isCalTime = false;
		for (int i=0; i<GameHandler.TotalCharacters; i++) {
			if(AllChilds [i] != null)
				AllChilds [i].SetActive (false);
		}
		if (isTutorialCust ) {
			//print ("Gen coin at "+seatNo);
			GameObject coins = Instantiate (UiManager.instance.CustCoinPrefab) as GameObject;
			//set parent
			if(TutorialController.ins.tutorialCurrPageNo < 10)
				coins.transform.SetParent (TutorialController.ins.seat.transform);
			else
				coins.transform.SetParent (TutorialController.ins.elfSeat.transform);
			coins.transform.localPosition = Vector3.zero;
			coins.transform.localScale = Vector3.one;
			CustomerCoins cc = coins.GetComponent<CustomerCoins> ();
			cc.setCoinValueAndSeatId (0,0);
		}
		else
			UiManager.instance.generateCollectableCoins(moneyToGive,customerSeatId);
	}

	public void playDrinkAnimation(){
		animator.SetTrigger ("Drink");
	}
}
