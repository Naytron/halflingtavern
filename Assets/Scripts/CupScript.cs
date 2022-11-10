using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CupScript : MonoBehaviour , IBeginDragHandler,IDragHandler,IEndDragHandler{

	Vector3 startPosition;
	Transform startParent;
	string containerName;
	public bool canMove = true;

	GameObject mainParent;

	public CupType cupType = CupType.None;
//	[HideInInspector]public bool isCupFilled = false;
//	[HideInInspector]public bool isIncentFilled = false;
	[HideInInspector]public bool isFilling = false;

	[HideInInspector]public Beverages containBaverage;
	[HideInInspector]public Beverages containIncent;

	[HideInInspector]public float currentFillBaverage;
	[HideInInspector]public float currentFillIncent;
//	[HideInInspector][Range(0,1)]public float currentFillBaverage;
	void Awake(){
	}

	// Use this for initialization
	void Start () {
		currentFillBaverage = 0f;
		containerName = transform.parent.name;	
//		mainParent = GameHandler.instance.GamePanelBg.transform.FindChild ("GlassesContainerMainParent").gameObject;
		setMainParent ();
		containBaverage = Beverages.None;
		containIncent = Beverages.None;
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

	// Update is called once per frame
	void Update () {
	
	}

	public void overflowCup(bool overFlow){
		if (overFlow) {
			transform.GetChild (0).GetComponent<Image> ().enabled = true;
		} else
			StartCoroutine (stopOverflow());
	}

	IEnumerator stopOverflow(){
		yield return new WaitForSeconds (2.0f);
		transform.GetChild(0).GetComponent<Image>().enabled = false;

	}


	public void OnBeginDrag (PointerEventData eventData)
	{
		if (GameHandler.itemBeingDragged == null && !isFilling && canMove) {
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

	public void OnEndDrag (PointerEventData eventData)
	{
		
		if (GameHandler.instance.isPlateAnimating)
			return;

		if (GameHandler.itemBeingDragged != null && transform.parent == startParent) {
				GameHandler.itemBeingDragged.transform.position = startPosition;
				canMove = true;
				GameHandler.itemBeingDragged = null;
		} 
		if(startParent != null && transform.parent != startParent && startParent.name == containerName){
			GlassContainer.instance.createNewCup(startPosition , cupType);
		}
		GetComponent<CanvasGroup> ().blocksRaycasts = true;
	}

}
