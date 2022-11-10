using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Dustbin : MonoBehaviour , IDropHandler{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnDrop (PointerEventData eventData)
	{
		if (GameHandler.itemBeingDragged != null && (GameHandler.itemBeingDragged.GetComponent<CupScript> () != null || GameHandler.itemBeingDragged.GetComponent<Plate> () != null)) 
		{
			GameHandler.itemBeingDragged.transform.SetParent(transform);
			GameHandler.itemBeingDragged.GetComponent<Image>().enabled = false;
			Plate plate= GameHandler.itemBeingDragged.GetComponent<Plate> () ;
			if(plate != null){
				GameController.instance.AddPlayerCoins = -GameHandler.currDay.FoodRuinedPenalty;
				UiManager.instance.coinsCollectedText.text = GameController.instance.AddPlayerCoins + "";
			}else{				
				GameController.instance.AddPlayerCoins = -GameHandler.currDay.DrinkRuinedPenalty;
				UiManager.instance.coinsCollectedText.text = GameController.instance.AddPlayerCoins + "";
			}
			GameHandler.itemBeingDragged = null;
			Invoke("consumeFood",5.0f);
		}
	}

	void consumeFood(){
		if (transform.childCount != 0)
			Destroy (transform.GetChild(0).gameObject);
	}

}
