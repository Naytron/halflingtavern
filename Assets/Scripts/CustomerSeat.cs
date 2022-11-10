using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using Holoville.HOTween;

public class CustomerSeat : MonoBehaviour , IDropHandler,IPointerClickHandler{
	public int seatId; 
	bool gotPlate;
	public Customer customer;
	public bool isSeatVacant = true;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public bool isCustomerPresent(){
		return customer.isPresent;
	}

	public int coinsToAdd;
	public void OnDrop (PointerEventData eventData)
	{
		bool willAccept = false;

		if (!customer.isPresent)
			return;
		else
			print ("has customer on seat");

		if (GameHandler.itemBeingDragged != null && isPlaceEmpty() &&(GameHandler.itemBeingDragged.GetComponent<CupScript>() != null || GameHandler.itemBeingDragged.GetComponent<Plate>() != null)) {

			if(GameHandler.itemBeingDragged.GetComponent<CupScript>() != null &&(customer.currReqBeverage != Beverages.None || customer.currReqIncent != Beverages.None))
			{
				CupScript cup = GameHandler.itemBeingDragged.GetComponent<CupScript>();
				cup.canMove = false;
				print ("cup.canMove "+customer.myCuptype+" "+cup.cupType);
				//check if served in right cup
				if(customer.myCuptype == cup.cupType)
				{
					if(customer.currReqBeverage != Beverages.None && customer.currReqIncent != Beverages.None)
					{
						if(cup.currentFillBaverage > 0.35f && customer.currReqBeverage == cup.containBaverage && cup.currentFillIncent > 0.35f && customer.currReqIncent == cup.containIncent){						
							customer.requirementCloud.ResetBeverage();
							customer.requirementCloud.ResetIncent();
							willAccept = true;
							if (!GameHandler.instance.isPlayingTutorial) {
								int val =  (customer.myCuptype == CupType.WoodenCup) ? GameHandler.currDay.DrinkValueSmall :  GameHandler.currDay.DrinkValueLarge ;
								customer.moneyToGive += PowerUp.instance.isDoubleCoinsActive ? val * 2 : val ; //15;
								//reduce wrt time served at
								reduceMoney_TimeServed();
							} else
								customer.moneyToGive = 10;
						}else
							UiManager.instance.showMsgWizard(1.0f,"Cup is not completely filled or wrong beverage");
					}else if(customer.currReqBeverage != Beverages.None && customer.currReqIncent == Beverages.None)
					{
						if(cup.currentFillBaverage > 0.8f && cup.currentFillIncent <= 0f && customer.currReqBeverage == cup.containBaverage){						
							customer.requirementCloud.ResetBeverage();
							willAccept = true;
							if (!GameHandler.instance.isPlayingTutorial) {
								int val = (customer.myCuptype == CupType.WoodenCup) ? GameHandler.currDay.DrinkValueSmall : GameHandler.currDay.DrinkValueLarge;
								customer.moneyToGive += PowerUp.instance.isDoubleCoinsActive ? val * 2 : val;//10;
								//reduce wrt time served at
								reduceMoney_TimeServed();
							} else
								customer.moneyToGive = 10;
						}else
							UiManager.instance.showMsgWizard(1.0f,"Cup is not completely filled or wrong beverage");
					}
				}else
					UiManager.instance.showMsgWizard(1.0f,"Serve in other type of cup");
			}
			print("cust req "+customer.currReqBeverage+" "+customer.currReqIncent+" "+customer.currReqPlatetype+" "+customer.customerId);
			if(customer.currReqPlatetype != PlateType.Empty && GameHandler.itemBeingDragged.GetComponent<Plate>() != null ){
					Plate plate = GameHandler.itemBeingDragged.GetComponent<Plate> ();
					plate.canMove = false;
					if(customer.currReqPlatetype == PlateType.BreadAndCheese)
					{
						if(plate.hasBread && plate.hasCheese){					
							willAccept = true;
							customer.moneyToGive+= PowerUp.instance.isDoubleCoinsActive ? GameHandler.currDay.FoodValueLarge * 2 : GameHandler.currDay.FoodValueLarge;
							//reduce wrt time served at
							reduceMoney_TimeServed();
						}else
							UiManager.instance.showMsgWizard(1.0f,"Serve bread and cheese both");
					}else//only one thing
					{
						if(customer.currReqPlatetype == PlateType.OnlyBread && plate.hasBread){
							willAccept = true;
							customer.moneyToGive+= PowerUp.instance.isDoubleCoinsActive ? GameHandler.currDay.FoodValueSmall * 2 : GameHandler.currDay.FoodValueSmall;
							//reduce wrt time served at
							reduceMoney_TimeServed();
						}else if(customer.currReqPlatetype == PlateType.OnlyCheese && plate.hasCheese){
							willAccept = true;
							customer.moneyToGive+= PowerUp.instance.isDoubleCoinsActive ? GameHandler.currDay.FoodValueSmall * 2 : GameHandler.currDay.FoodValueSmall;
							//reduce wrt time served at
							reduceMoney_TimeServed();
						}else
							UiManager.instance.showMsgWizard(1.0f,"Serve the correct food");
					}
			}else if(GameHandler.itemBeingDragged.GetComponent<Plate>() != null)
				UiManager.instance.showMsgWizard(1.0f,"Have not ordered this");
			//if correct food/drink given then only will accept
			if(willAccept){
				if(GameHandler.itemBeingDragged.GetComponent<Plate>() != null)
					gotPlate = true;
				else
					gotPlate = false;
				if(gotPlate){

					customer.currReqPlatetype = PlateType.Empty;
					GameHandler.itemBeingDragged.transform.localRotation = Quaternion.identity;
					GameHandler.itemBeingDragged.GetComponent<Plate>().isPlateRecived = true;
					GameHandler.instance.isPlateAnimating = true;
					seatPosition = (seatId-1)%3;
					if(seatId<=3) sideMultiple=-1; else sideMultiple = 1;

					objRT = GameHandler.itemBeingDragged.transform.GetComponent<RectTransform>();
					initialPos = new Vector2(469f * sideMultiple,0f);
					finalpos = new Vector2(860f * sideMultiple,0f);

					objRT.anchoredPosition = initialPos;
					HOTween.To(objRT,0.5f,new TweenParms().Prop("anchoredPosition",finalpos).OnComplete(MovePlateToCust).TimeScale(1f));

					if(customer.currReqBeverage != Beverages.None){
						customer.addWaitTime( GameHandler.currDay.OrderHalfCompleteTimeBonus );
						print("Half order done");}
				}else{					
					GameHandler.itemBeingDragged.transform.SetParent(transform);
					GameHandler.itemBeingDragged.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
					GameHandler.itemBeingDragged.transform.localScale = Vector3.one;

					customer.currReqBeverage = Beverages.None;
					customer.currReqIncent = Beverages.None;
					GameHandler.itemBeingDragged = null;				
					Invoke("consumeFood",0.1f);
					if(customer.currReqPlatetype != PlateType.Empty){
						customer.addWaitTime(GameHandler.currDay.OrderHalfCompleteTimeBonus );
						print("Half order done");
					}
				}
			}
		}
	}


	public void reduceMoney_TimeServed(){
		customer.moneyToGive -= (int) (customer.moneyToGive * 0.2f * customer.waitDotsReduced);
	}

	RectTransform objRT;
	void MovePlateToCust(){
		finalpos += new Vector2((- 860f + setValues[seatPosition].x) * sideMultiple, setValues[seatPosition].y);// {4 - 409,185}{5 - 582,120}{6- 793,67}
		TweenParms par = new TweenParms().Prop("anchoredPosition",finalpos).OnComplete(plateReachedDest);
		par.TimeScale(1f);
		HOTween.To(objRT,0.5f,par);
	}
	void plateReachedDest(){		
		GameHandler.itemBeingDragged.transform.SetParent(transform);
		GameHandler.itemBeingDragged.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
		GameHandler.itemBeingDragged.transform.localScale = Vector3.one;

		customer.requirementCloud.ResetPlate();		
		GameHandler.itemBeingDragged.GetComponent<Plate> ().GenerateNewPlate ();

		GameHandler.itemBeingDragged = null;
		GameHandler.instance.isPlateAnimating = false;		
		Invoke("consumeFood",0.1f);
	}

//	public GameObject animateMoveGO;
//	float frac;
	public Canvas mainCanvs;
	GameObject movePlateObj;
	Vector2 finalpos,initialPos;
//	bool isUpAnimDone;
	Vector3[] setValues = {new Vector3(409f,185f,0f),new Vector3(582f,120f,0f),new Vector3(793f,67f,0f)};
	int seatPosition,sideMultiple;

//	void moveToCorner(){
////		print("moveToCorner "+GameHandler.itemBeingDragged.transform.position);
//		if(frac<1){
//			frac += Time.deltaTime * 1f;
////			animateMoveGO.transform.GetComponent<RectTransform>().anchoredPosition= Vector2.Lerp(initialPos ,finalpos , frac);
////			GameHandler.itemBeingDragged.transform.position = Vector3.Lerp(new Vector3(477f,578f,0f) ,new Vector3(860f,578f,0f) , frac);
//		}else 
//		if(!isUpAnimDone){ 
//			isUpAnimDone=true;
////			initialPos = animateMoveGO.transform.GetComponent<RectTransform>().anchoredPosition;
//			finalpos += new Vector2((- 860f + setValues[seatPosition].x) * sideMultiple, setValues[seatPosition].y);// {4 - 409,185}{5 - 582,120}{6- 793,67}
//			frac = 0f;
//		}else{
//			CancelInvoke("moveToCorner");	
//			//if object is plate reset it and add coins
//			customer.requirementCloud.ResetPlate();
////			UiManager.instance.addandSetTotalCoinsEarned(coinsToAdd);
////			animateMoveGO.transform.SetParent(transform);
//			Destroy (transform.GetChild(0).gameObject); 
////			UiManager.instance.generateCollectableCoins(coinsToAdd,seatId);
//		}
//	}
	
	public bool isPlaceEmpty(){
		if (transform.childCount == 0)
			return true;
		return false;
	}

	void consumeFood(){
		if (transform.childCount != 0) {
			Destroy (transform.GetChild (0).gameObject);	
//			UiManager.instance.generateCollectableCoins(coinsToAdd,seatId);
		}
	}
	public void OnPointerClick (PointerEventData eventData)
	{
		if (isCustomerPresent () && PowerUp.instance.isBootActive) {
//			print("Remove this customer");
			PowerUp.instance.deActivateBootPower();
			customer.reset();
		}
	}

}
