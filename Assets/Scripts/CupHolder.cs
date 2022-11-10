using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CupHolder : MonoBehaviour,IDropHandler {
	public Beverages holderBev;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}
	public void OnDrop (PointerEventData eventData)
	{
		if (GameHandler.itemBeingDragged != null && isHolderEmpty() && GameHandler.itemBeingDragged.GetComponent<CupScript>() != null ) {
			CupScript cup = GameHandler.itemBeingDragged.GetComponent<CupScript>();
			if(holderBev<=Beverages.Grape)
			{
				if(cup.containBaverage == Beverages.None || cup.containBaverage == holderBev){
					
					GameHandler.itemBeingDragged.transform.SetParent(transform);
					GameHandler.itemBeingDragged.transform.localPosition = Vector3.zero;
					GameHandler.itemBeingDragged.transform.localScale = Vector3.one;
					//set its main parent
					cup.setMainParent();
					
					GameHandler.itemBeingDragged = null;
					if(isTapOpen())
						transform.parent.GetComponent<TapScript>().setInitialCupFillingValues();
				}
			}else{				
				if(cup.containIncent == Beverages.None || cup.containIncent == holderBev){
					
					GameHandler.itemBeingDragged.transform.SetParent(transform);
					GameHandler.itemBeingDragged.transform.localPosition = Vector3.zero;
					GameHandler.itemBeingDragged.transform.localScale = Vector3.one;
					//set its main parent
					cup.setMainParent();
					
					GameHandler.itemBeingDragged = null;
					if(isTapOpen())
						transform.parent.GetComponent<TapScript>().setInitialCupFillingValues();
				}
			}

		}
	}

	public bool isHolderEmpty(){
		if (transform.childCount == 0)
			return true;
		return false;
	}

	public bool isTapOpen()
	{
		return transform.parent.GetComponent<TapScript>().isTapOpen;
	}

}



