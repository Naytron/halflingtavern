using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {

	int levelNo;
	int custToServe;
	int coinsToCollect;
	int timeToServe;//in seconds
	bool isLevelCustomerBased;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//set level according to customer based or coin based
	public void setLevelValues(bool iscustBased, int target){
		isLevelCustomerBased = iscustBased;
		if (isLevelCustomerBased)
			custToServe = target;
		else
			coinsToCollect = target;	
	}

	public void loadLevel(){
		print ("Level no "+levelNo+" serve: "+custToServe+" in "+timeToServe);		
//		GameHandler.instance.totalLevelTime = timeToServe;

		GameHandler.instance.isTaskCustomer = isLevelCustomerBased;
		GameHandler.instance.totalLevelCustomers = custToServe;
		GameHandler.instance.totalCoinstoCollect = coinsToCollect;
		PopUpsManager.instance.openScreen (MyScreens.Gameplay);
		GameController.instance.startNewGame ();

		string taskStr = "";
		if (isLevelCustomerBased)
			taskStr = "Serve "+custToServe+" customers in time.";
		else
			taskStr = "Collect "+coinsToCollect+" coins in time.";
		UiManager.instance.setTask (taskStr);

	}
}
