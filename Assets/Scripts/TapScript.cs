using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Holoville.HOTween;

public class TapScript : MonoBehaviour {

	public bool isTapOpen;
	bool isFillBar;
	bool isContainCup;
	bool isIncent;
	[SerializeField] Beverages myBeverage;

	float barFillSpeed = 0.3f;
	float currentFill;

	Image fillImage;
	Image flowImage;

	CupHolder holder;


	// Use this for initialization
	void Awake () {
		fillImage = transform.FindChild("Bar").FindChild("FillImage").GetComponent<Image> ();	
		flowImage = transform.FindChild("BeerAnimation").GetComponent<Image> ();	
		holder = transform.GetComponentInChildren<CupHolder> ();
		if (myBeverage >= Beverages.Costmary)
			isIncent = true;
	}

	public void resetTap(){
		isFillBar = false;
		isTapOpen = false;
		setFillAmount (0f);	
		stopFlowAnimation();
		if(!holder.isHolderEmpty())
			Destroy(holder.transform.GetChild(0).gameObject);
	}

	// Update is called once per frame
	void Update () {
		if (isFillBar) {
			currentFill += Time.deltaTime * barFillSpeed ;
			if(currentFill <= 1f)
				setFillAmount(currentFill);	

			if(currentFill >= 0.8f)
				checkAndSendMessageofOverflow();
		}	
	}
	void playFlowAnimation(){
		flowImage.enabled = true;
	}
	void stopFlowAnimation(){
		flowImage.enabled = false;
	}
	void setFillAmount(float fillValue){
		fillImage.fillAmount = fillValue;
	}


	void openTap(){
		playFlowAnimation ();
		SoundManager.ins.playOneShotClip (AudioClipType.PourDrink);

		if (holder.isHolderEmpty ()) {
			UiManager.instance.showMsgWizard(1.0f,"Do not waste/overflow beverage, it will reduce your coins");
			print ("have'nt placed cup,wasting food");
			if(!isReducedCoin){
				StartCoroutine (reduCeOverflowCoin());
			}
		}else 
			setInitialCupFillingValues ();
	}

	public void setInitialCupFillingValues(){
	
		CupScript cup = holder.transform.GetComponentInChildren<CupScript> ();
		float cupCurrentFill = cup.currentFillBaverage + cup.currentFillIncent;
		print (""+cupCurrentFill+" "+cup.currentFillBaverage+" " + cup.currentFillIncent);
		if (!isIncent)// && cup.currentFillBaverage <= 1) 
		{
			holder.transform.GetComponentInChildren<CupScript> ().isFilling = true;
			holder.transform.GetComponentInChildren<CupScript> ().containBaverage = myBeverage;
//			cupCurrentFill = cup.currentFillBaverage;
		} else if (isIncent)// && cup.currentFillIncent <= 1) 
		{
			holder.transform.GetComponentInChildren<CupScript> ().isFilling = true;
			holder.transform.GetComponentInChildren<CupScript>().containIncent = myBeverage;
//			cupCurrentFill = cup.currentFillIncent;
		}else{
			holder.transform.GetComponentInChildren<CupScript> ().isFilling = true;
			print ("placed cup is laready filled,wasting food");
			UiManager.instance.showMsgWizard(1.0f,"placed cup is laready filled");
		}
	
		setFillAmount (cupCurrentFill);
		currentFill = cupCurrentFill;
		isFillBar = true;	
		if(!GameHandler.instance.isPlayingTutorial)
			barFillSpeed = 1f/(GameHandler.currDay.DrinkPrepTime);
	}



	void closeTap(){
		if (!holder.isHolderEmpty ()) {
			CupScript cup = holder.transform.GetComponentInChildren<CupScript> ();
			if (isIncent)
				cup.currentFillIncent = currentFill - cup.currentFillBaverage;
			else
				cup.currentFillBaverage = currentFill - cup.currentFillIncent;
			cup.isFilling = false;
			cup.overflowCup(false);
		}
		currentFill = 0;
		stopFlowAnimation ();
		SoundManager.ins.stopOneShotClip ();
		setFillAmount (0f);	
		isFillBar = false;	
//		CancelInvoke("setBarValues");
	}

	bool isReducedCoin;
	int OverflowCoinReduce = 1;
	void checkAndSendMessageofOverflow(){
		if (!holder.isHolderEmpty()) {
			if (currentFill > 1.3f) {
				holder.transform.GetComponentInChildren<CupScript> ().overflowCup (true);
				//reduce coin for overflow
				if(!isReducedCoin){
					StartCoroutine (reduCeOverflowCoin());
				}
			}
		}	
	}

	IEnumerator reduCeOverflowCoin(){
		isReducedCoin = true;
		yield return new WaitForSeconds (2.0f);
		isReducedCoin = false;
		GameController.instance.AddPlayerCoins = -OverflowCoinReduce;
		UiManager.instance.coinsCollectedText.text = GameController.instance.AddPlayerCoins + "";
	}

	public void isTapped(){
		if (isTapOpen) {
			isTapOpen = false;
			closeTap();
		} else {			
			isTapOpen = true;
			openTap();
		}
	}

}
