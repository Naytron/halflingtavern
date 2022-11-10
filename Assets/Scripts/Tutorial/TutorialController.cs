using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour {

	public static TutorialController ins;
	
	public GameObject[] allHelpMsgs;
	public GameObject nextButton;
	public GameObject tutorialPanelExpWizard;
	public GameObject smallCust, bigCust;
	public Text wizardText;
	
	public int tutorialCurrPageNo;

	public CupScript woodenCup,metalCup;
	public CustomerSeat seat,elfSeat;
	
	string[] msg ;
	int msgNo = 0;
	bool isNext7Called = false;
	int PagesInDay1 = 9;

	// Use this for initialization
	void Awake () {
		ins = this;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (tutorialCurrPageNo == 5) {
			if (woodenCup.transform.parent.GetComponent<CupHolder> () != null) {
				nextButtonClicked();
			}
		}else if(tutorialCurrPageNo == 6)
		{
			if(woodenCup.currentFillBaverage >= 0.8f){
				nextButtonClicked();
			}			
		}else if(tutorialCurrPageNo == 7)
		{
			if(!isNext7Called){
				isNext7Called = true;				
				nextButton.SetActive (true);
//				Invoke("nextButtonClicked",2.0f);
			}			
		}else if (tutorialCurrPageNo == 14) {
			if (metalCup.currentFillBaverage >= 0.5f) {
				nextButtonClicked();
			}				
		}
		else if (tutorialCurrPageNo == 15) {
			if (metalCup.currentFillBaverage >= 0.5f && metalCup.currentFillIncent >= 0.5f) {
				nextButtonClicked();
			}				
		}
	}

	int tutorialDay;
	public void initTutorial(int playDay){
		tutorialDay = playDay;
		//set tutorial panel
		woodenCup.canMove = false;
		metalCup.canMove = false;
		msgNo = 0;
		if (tutorialDay == 1) {
			msg = new string[4];
			msg [0] = "Today is training day, and you look like you just rolled out of your halfling hole";
			msg [1] = "Here comes our first customer now. Try not to muck it up.";
			msg [2] = "You get the gist, some of your orders will become a bit more complex.";
			msg [3] = "Your a spry young lad, you will sort it out. See you tomorrow.";

			smallCust.SetActive (true);
			bigCust.SetActive (false);
			tutorialCurrPageNo = 0;
		} else if (tutorialDay == 3) {
			msg = new string[3];
			//for day 3
			msg [0] = "Congratulations, you have made it this far without mucking it up.. too mouch";
			msg [1] = "Keep it up young halfling";
			msg [2] = "I will make a man out of you yet";
			smallCust.SetActive (false);
			bigCust.SetActive (true);
			tutorialCurrPageNo = 9;
		}

		tutorialPanelExpWizard.SetActive (true);
		wizardText.text = msg [msgNo];	
	}

	void showHelp(int index){
		print("show help "+index);
		if((tutorialDay==1 && index==0) || (tutorialDay==3 && index==9))
			tutorialPanelExpWizard.SetActive(false);
		
		for (int i=0; i<allHelpMsgs.Length; i++) {
			if(index==i)
				allHelpMsgs[i].SetActive(true);
			else
				allHelpMsgs[i].SetActive(false);
		}
	}
	public void closeAllMsgs(){
		print("closeAllMsgs allHelpMsgs ");
		for (int i=0; i<allHelpMsgs.Length; i++) {
			allHelpMsgs[i].SetActive(false);
		}
	}

	public void nextButtonClicked(){		
		if (tutorialCurrPageNo == 0 && msgNo == 0) {
			msgNo++;
			wizardText.text = msg [msgNo];	
		} else if (tutorialDay == 3) {
			if (tutorialCurrPageNo < allHelpMsgs.Length) {
				print ("Day 3 tutorial page no " + tutorialCurrPageNo);
				if (tutorialCurrPageNo == 13) {
					nextButton.SetActive (false);
					metalCup.canMove = true;
					tutorialCurrPageNo++;
				} else {
					print ("else curr page");
					tutorialCurrPageNo++;
				}	
				showHelp (tutorialCurrPageNo - 1);

			} else if (tutorialCurrPageNo == allHelpMsgs.Length) {
				allHelpMsgs [tutorialCurrPageNo - 1].SetActive (false);
				tutorialPanelExpWizard.SetActive (true);
				wizardText.text = msg [1 + msgNo];
				if (msgNo == 1) {
					tutorialCurrPageNo++;
				} else
					msgNo = 1;

				GameHandler.instance.isPlayingTutorial = false;
				nextButton.SetActive (true);
			} else {
				GameController.instance.loadDay ();
				PlayerPrefs.SetInt (MyPrefereces.KEY_IS_DAY3_TUT_DONE, 1);
				PlayerPrefs.Save ();
			}
			
		}else if (tutorialDay == 1) {
			if (tutorialCurrPageNo < PagesInDay1) {
				print ("ttutorial page no " + tutorialCurrPageNo);
				if (tutorialCurrPageNo == 4) {
					nextButton.SetActive (false);
					woodenCup.canMove = true;
					tutorialCurrPageNo++;
				}
				if (tutorialCurrPageNo == 5) {
					if (woodenCup.transform.parent.GetComponent<CupHolder> () != null) {
						//check if cup is placed on table
						print ("cup placed ");
						tutorialCurrPageNo++;
					}				
				} else if (tutorialCurrPageNo == 6) {
					if (woodenCup.currentFillBaverage >= 0.8f) {
						//check if cup is placed on table
						tutorialCurrPageNo++;
						isNext7Called = false;
						msgNo = 0;
					}				
				} else {
					print ("else curr page");
					tutorialCurrPageNo++;
					if (tutorialCurrPageNo == 8)
						nextButton.SetActive (false);
				}

				showHelp (tutorialCurrPageNo - 1);

			} else if (tutorialCurrPageNo == PagesInDay1) {
				allHelpMsgs [tutorialCurrPageNo - 1].SetActive (false);
				tutorialPanelExpWizard.SetActive (true);
				wizardText.text = msg [2 + msgNo];
				if (msgNo == 1) {
					tutorialCurrPageNo++;
				} else
					msgNo = 1;

				GameHandler.instance.isPlayingTutorial = false;
				nextButton.SetActive (true);
			} else
				GameController.instance.loadDay ();
		}

		print("Next btn clicked "+tutorialCurrPageNo+" "+tutorialDay+" "+msgNo);		


	}
}
