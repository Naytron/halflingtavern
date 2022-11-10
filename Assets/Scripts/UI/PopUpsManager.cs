using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum MyScreens{
	Gameplay				=	0,
	Splash					=	1,
	Menu					=	2,
	Shop					=	3,
	GetGold					=	4,
	GamePause				=	5,
	Result					=	6,
	StoryBoard				=	7,
	Tutorial				=	8,
}

public class PopUpsManager : MonoBehaviour {

	public static PopUpsManager instance;

	public MyScreens currentOpenScreen;
	public MyScreens lastOpenScreen;
	public GameObject[] allScreens;

	void Awake(){
		instance = this;
	}

	// Use this for initialization
	void Start () {

		closeAllScreen ();

		openScreen (MyScreens.Splash);	
		currentOpenScreen = lastOpenScreen = MyScreens.Splash;
		SoundManager.ins.playBackgroudSound();
		playVideoSplash();
//		openScreenAfterSplash();
	}

	#if UNITY_STANDALONE
	public MovieTexture mTex;
	#endif
	[SerializeField] RawImage rawImg;
	void playVideoSplash(){
		#if UNITY_STANDALONE
			rawImg.texture = mTex;
			mTex.loop = true;
			mTex.Play();
		#endif

	}


//	yield return new WaitForSeconds (3.0f);
	public void openScreenAfterSplash(){

		SoundManager.ins.stopBackgroudSound();	

		if(	PlayerPrefs.GetInt(MyPrefereces.KEY_IS_STORY_PLAYED) == 0){
			print("open story panel");
			openScreen (MyScreens.StoryBoard);
			PlayerPrefs.SetInt (MyPrefereces.KEY_IS_STORY_PLAYED, 1);
			PlayerPrefs.Save();
		}
		else
			openScreen (MyScreens.Menu);

	}

	// Update is called once per frame
	void Update () {
		#if UNITY_ANDROID
			//back button press functionality
			if (Input.GetKeyUp (KeyCode.Escape)){

			}
		#endif	
	}

	public void openScreen(MyScreens screen){
		lastOpenScreen = currentOpenScreen;
		closeScreen(currentOpenScreen);
		allScreens[(int)screen].SetActive(true);
		currentOpenScreen = screen;
		if(currentOpenScreen == MyScreens.Menu){
			UiManager.instance.menuWarningPopUp(false);
		}

	}

	void closeScreen(MyScreens screen){
			allScreens[(int)screen].SetActive(false);
	}

	public void closeAllScreen(){
		for(int i=0;i<allScreens.Length;i++){
			allScreens[i].SetActive(false);
		}
	}

	public GameObject popUpSingleBtn;
	public bool isMsgPopUpOpen;
	public void showSingleButtonPopUp(string message){		
		popUpSingleBtn = Instantiate (popUpSingleBtn) as GameObject;
		Transform mainCanvas = GameObject.Find("Canvas").transform;
		popUpSingleBtn.transform.SetParent (mainCanvas);
		popUpSingleBtn.transform.localPosition = Vector3.zero;
		popUpSingleBtn.transform.localScale = Vector3.one;
		popUpSingleBtn.transform.SetAsLastSibling ();		
		MessagePopUp msgPopUp = popUpSingleBtn.GetComponent<MessagePopUp> ();
		msgPopUp.setMessageSingleOkButton (message);
		isMsgPopUpOpen = true;
	}

//	GameObject popUp;
//	public void ShowExitPopUp(){
//		if (popUp == null) {
//			popUp = Instantiate (PopUpPrefab) as GameObject;
//			popUp.transform.SetParent (mainCanvas.transform);
//			popUp.transform.localPosition = Vector3.zero;
//			popUp.transform.localScale = Vector3.one;
//			popUp.transform.SetAsLastSibling ();		
//			MessagePopUp msgPopUp = popUp.GetComponent<MessagePopUp> ();
//			msgPopUp.setMessageAndActions ("Exit Game ?", "Yes", "No", method1Call, method2Call);	
//		}
//	}
//
//	void method1Call(){
//		//		Debug.Log ("method 1 call");
//		Application.Quit ();
//		//		Destroy (popUp.gameObject);
//		//		popUp = null;
//	}
//	void method2Call(){
//		//		Debug.Log ("method 2 call");
//		Destroy (popUp.gameObject);
//		popUp = null;
//	}



}
