using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RequirementCloud : MonoBehaviour {

	public Image beverageImg,incentImg,foodImg;

	// Use this for initialization
	void Awake () {

		beverageImg = transform.FindChild ("Bevrage").GetComponent<Image> ();
		incentImg = transform.FindChild ("Incent").GetComponent<Image> ();
		foodImg = transform.FindChild ("FoodPlate").GetComponent<Image> ();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetRequirement(Beverages bev,Beverages inc,PlateType food){
		beverageImg.sprite = incentImg.sprite = foodImg.sprite = null;
		//setImages
		if(bev != Beverages.None)
			this.beverageImg.sprite = GameHandler.instance.Beverages[(int)bev-1];
		if(inc != Beverages.None)
			this.incentImg.sprite = GameHandler.instance.Beverages[(int)inc-1];
		if(food != PlateType.Empty)
			this.foodImg.sprite = GameHandler.instance.allPlates[(int)food];
	}

	public void changePlateReq(PlateType plateType){
		if(plateType != PlateType.Empty)
			this.foodImg.sprite = GameHandler.instance.allPlates[(int)plateType];
	}

	public void ResetBeverage(){	
		this.beverageImg.sprite = null;
		transform.SendMessageUpwards ("playDrinkAnimation");
		if (this.beverageImg.sprite == null && this.incentImg.sprite == null && this.foodImg.sprite == null)
			transform.SendMessageUpwards ("myAllReqFulfilled",false);
	}
	public void ResetIncent(){	
		this.incentImg.sprite = null;
		transform.SendMessageUpwards ("playDrinkAnimation");
		if (this.beverageImg.sprite == null && this.incentImg.sprite == null && this.foodImg.sprite == null)
			transform.SendMessageUpwards ("myAllReqFulfilled",false);
	}
	public void ResetPlate(){	
		this.foodImg.sprite = null;
		if (this.beverageImg.sprite == null && this.incentImg.sprite == null && this.foodImg.sprite == null)
			transform.SendMessageUpwards ("myAllReqFulfilled",true);
	}		

}
