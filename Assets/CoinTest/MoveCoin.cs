using UnityEngine;
using System.Collections;
using Holoville.HOTween;
using UnityEngine.UI;

public class MoveCoin : MonoBehaviour {
	RectTransform rTrans,parentRTrans;
	public float delayVal ;
	Vector3 destination;

	// Use this for initialization
	void Start () {
//		HOTween.To (transform,1f,new TweenParms().Prop("position",new Vector3(5f,5f,0f)));
		rTrans = transform.GetComponent<RectTransform>();
//		parentRTrans = transform.GetComponentInParent<CustomerSeat>().GetComponent<RectTransform>();
		parentRTrans = transform.parent.GetComponent<RectTransform>();
//		destination = new Vector2 (487f, 312f);//-1865,-1235
		destination = new Vector3 (-900f, -700f ,0f);
//		destination = new Vector3 (-369f, -276f , 0.0f);
//		print (" "+rTrans.anchoredPosition+" "+destination+" "+parentRTrans.anchoredPosition);
//		HOTween.To (transform.GetComponent<Image>().color,1f,new TweenParms().Prop("a",1).Ease(EaseType.Linear).Delay(delayVal));

		collect ();//testing
	}

	public void collect(){
		
		Vector2 dest = (Vector2)destination - parentRTrans.anchoredPosition;
//		Vector2 fnl = ((Vector2)destination + parentRTrans.anchoredPosition);

//		print ("travel "+fnl);

		HOTween.To (rTrans,1f,new TweenParms().Prop("anchoredPosition", dest ).Ease(EaseType.Linear).Delay(delayVal));

//		HOTween.To (rTrans,1f,new TweenParms().Prop("position", (Vector3)(rTrans.position + destination) ).Ease(EaseType.Linear).Delay(delayVal));



	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
