using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class FoodItem : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler {

	public 	Food 		foodType;//type of food i.e bread or cheese
	public 	Image 		foodItemImg;//image of the food item
	
	private Vector3 	startPosition;
	private Transform 	startParent;
	private GameObject 	mainParent;

	void Awake(){
		foodItemImg.color = new Color(1f,1f,1f,0f);//to hide the image
	}

	// Use this for initialization
	void Start () {
		setMainParent ();
	}

	public void setMainParent(){
		mainParent = FindMainParent ();
	}

	//find the main parent of this item
	private GameObject FindMainParent(){
		GameObject mp = transform.gameObject;
		
		while(mp.transform.parent.name != GameHandler.instance.GamePanelBg.name){
			mp = mp.transform.parent.gameObject;
		}
		return mp;	
	}

	//get called when start dragging the object
	public void OnBeginDrag (PointerEventData eventData)
	{
		//if currently not dragging any object store its initial settings
		if (GameHandler.itemBeingDragged == null) 
		{
			transform.SetAsLastSibling();
			mainParent.transform.SetAsLastSibling();
			GameHandler.itemBeingDragged = transform.gameObject;
			startPosition = transform.position;
			startParent = transform.parent;
			GetComponent<CanvasGroup> ().blocksRaycasts = false;
			foodItemImg.color = new Color(1f,1f,1f,1f);
		}
	}

	//get called while dragging the object
	public void OnDrag (PointerEventData eventData)
	{
		//move the object with mouse or touch
		if (GameHandler.itemBeingDragged != null) {
			GameHandler.itemBeingDragged.transform.position = Input.mousePosition;
		}
	}

	//get called when ends dragging the object
	public void OnEndDrag (PointerEventData eventData)
	{
		//reset the object when drag ends
		if (GameHandler.itemBeingDragged != null) {
			GameHandler.itemBeingDragged.transform.position = startPosition;
			GameHandler.itemBeingDragged = null;
			foodItemImg.color = new Color(1f,1f,1f,0f);
		} 
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
	}

}
