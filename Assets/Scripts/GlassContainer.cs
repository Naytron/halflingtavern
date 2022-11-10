using UnityEngine;
using System.Collections;

public class GlassContainer : MonoBehaviour {

	public static GlassContainer instance;
	public GameObject MetalGlassPrefab,WoodenGlassPrefab;

	float totalWidthRack;
	float startX,currentX;
	float leftPadding = 10f;
	float cupDistance = 15f;
	int totalCups = 6;

	void Awake(){
		instance = this;
	}


	// Use this for initialization
	void Start () {
		totalWidthRack = transform.GetComponent<RectTransform> ().rect.width;
		startX = -totalWidthRack / 2;
	}

	public void createNewCupsOnRack(){
		//if already has cups delete them
		if (transform.childCount != 0) {
			for(int i=0;i<transform.childCount;i++)
				Destroy(transform.GetChild(i).gameObject);
		}
		currentX = startX + leftPadding + MetalGlassPrefab.transform.GetComponent<RectTransform> ().rect.width / 2;
		//create new 6 cups 3 metal,3 wooden
		for(int i=0;i<totalCups;i++){
			GameObject cup;

			switch(GameHandler.currDay.CupTypeLevel){
			case 1 :
				cup = (GameObject)Instantiate(WoodenGlassPrefab);
				break;
			case 2:
				cup = (GameObject)Instantiate(MetalGlassPrefab);
				break;
			default:
				if(i<totalCups/2)
					cup = (GameObject)Instantiate(MetalGlassPrefab);
				else
					cup = (GameObject)Instantiate(WoodenGlassPrefab);
				break;
			}

			cup.transform.SetParent(gameObject.transform);
			float cupWidth = cup.transform.GetComponent<RectTransform> ().rect.width;
			cup.transform.GetComponent<RectTransform> ().anchoredPosition = new Vector3( currentX , 0f , 0f );
			currentX += cupWidth + cupDistance;  
			cup.transform.localScale = Vector3.one;

		}	
	}

	public void createNewCup(Vector3 anchorPosition , CupType cuptype ){
		GameObject cup;
		if(cuptype == CupType.MetalCup)
			cup = (GameObject)Instantiate(MetalGlassPrefab);
		else
			cup = (GameObject)Instantiate(WoodenGlassPrefab);		
		cup.transform.SetParent(gameObject.transform);
		cup.transform.localScale = Vector3.one;
		cup.transform.position = anchorPosition;
	}


	// Update is called once per frame
	void Update () {
	
	}


}
