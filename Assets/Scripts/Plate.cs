using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class Plate : MonoBehaviour ,IDropHandler,IDragHandler,IBeginDragHandler,IEndDragHandler{

	public bool hasBread,hasCheese;
	public bool isPlateFilled;
	public bool canMove = true;
	
	Vector3 startPosition;
	Transform startParent;
	string startParentName;
	GameObject mainParent;

	// Use this for initialization
	void Start () {
		setPlateType ();	
		startParentName = transform.parent.name;
		setMainParent ();
	}

	public void setMainParent(){
		mainParent = FindMainParent ();
	}
	
	GameObject FindMainParent(){
		GameObject mp = transform.gameObject;
		
		while(mp.transform.parent.name != GameHandler.instance.GamePanelBg.name){
			mp = mp.transform.parent.gameObject;
		}
		return mp;	
	}

	public void resetPlateValues(){
		hasBread = false;
		hasCheese = false;
		isPlateFilled = false;
		canMove = true;
		setPlateType ();
	}

	public void OnDrop (PointerEventData eventData)
	{
		if (GameHandler.itemBeingDragged != null && GameHandler.itemBeingDragged.GetComponent<FoodItem>() != null) 
		{
			Food food = GameHandler.itemBeingDragged.GetComponent<FoodItem>().foodType;
			if(!hasBread && food == Food.Bread)
				hasBread = true;
			if(!hasCheese && food == Food.Cheese)
				hasCheese = true;
			isPlateFilled = true;
			setPlateType();
		}
	}

	void setPlateType(){
		if(hasBread && !hasCheese)
			transform.GetComponent<Image> ().sprite = GameHandler.instance.allPlates [(int)PlateType.OnlyBread];
		else if(hasCheese && !hasBread)
			transform.GetComponent<Image> ().sprite = GameHandler.instance.allPlates [(int)PlateType.OnlyCheese];
		else if(hasBread && hasCheese)
			transform.GetComponent<Image> ().sprite = GameHandler.instance.allPlates [(int)PlateType.BreadAndCheese];
		else
			transform.GetComponent<Image> ().sprite = GameHandler.instance.allPlates [(int)PlateType.Empty];
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		if (GameHandler.itemBeingDragged == null && isPlateFilled && canMove) {
			transform.SetAsLastSibling();
			mainParent.transform.SetAsLastSibling();
			GameHandler.itemBeingDragged = transform.gameObject;
			startPosition = transform.position;
			startParent = transform.parent;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
		}
	}
	
	public void OnDrag (PointerEventData eventData)
	{
		if (GameHandler.itemBeingDragged != null) {
			GameHandler.itemBeingDragged.transform.position = Input.mousePosition;
		}
	}

	public bool isPlateRecived;
	public void OnEndDrag (PointerEventData eventData)
	{
		if (isPlateRecived)
			return;

		if (GameHandler.itemBeingDragged != null && transform.parent == startParent) {
			GameHandler.itemBeingDragged.transform.position = startPosition;
			canMove = true;
			GameHandler.itemBeingDragged = null;
		} 
		if(isPlateFilled && transform.parent != startParent && startParent.name == startParentName){
			GenerateNewPlate();
		}
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
	}


	//to create a new empty plate on table
	public void GenerateNewPlate(){
		//create new plate at this position
		GameObject plate = (GameObject)Instantiate(GameHandler.instance.emptyPlatePrefab);
		plate.GetComponent<Plate> ().resetPlateValues();
		plate.transform.SetParent(startParent.transform);
		plate.transform.localScale = Vector3.one;
		plate.transform.localRotation = Quaternion.identity;
		plate.transform.position = startPosition;
		plate.GetComponent<CanvasGroup> ().blocksRaycasts = true;
	}

}
