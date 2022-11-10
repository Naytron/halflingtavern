using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessagePopUp : MonoBehaviour {

	 GameObject button1,button2;
	 Text messageText;
	Text button1Text,button2Text;

	public delegate void ButtonClick();
	ButtonClick button1Click,button2Click;

	// Use this for initialization
	void Awake () {
		button1 = transform.FindChild ("Button1").gameObject;
		button2 = transform.FindChild ("Button2").gameObject;
		messageText = transform.FindChild ("Message").GetComponent<Text> ();
		button1Text = button1.GetComponentInChildren<Text> ();
		button2Text = button2.GetComponentInChildren<Text> ();	
	}
	
	// Update is called once per frame
//	void Update () {
//	
//	}

	public void setMessageAndActions(string msg,string b1Name,string b2Name,ButtonClick b1Click,ButtonClick b2Click){
		messageText.text = msg;
		button1Text.text = b1Name;
		button2Text.text = b2Name;

		this.button1Click = b1Click;
		this.button2Click = b2Click;
	}

	public void OnButtonClick(Text buttonText){
		if (buttonText.text == button1Text.text)
			this.button1Click ();
		else if (buttonText.text == button2Text.text)
			this.button2Click ();
		//play audio
//		AudioManager.instance.playButtonClickSound ();
	}

	public void setMessageSingleOkButton(string msg){
		messageText.text = msg;
	}
	
	public void OnOkButtonClick(){
		Destroy (transform.gameObject);
		//play audio
//		AudioManager.instance.playButtonClickSound ();

		PopUpsManager.instance.isMsgPopUpOpen = false;

	}

}
