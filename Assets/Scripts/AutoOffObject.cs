using UnityEngine;
using System.Collections;

public class AutoOffObject : MonoBehaviour {

	public float waitTime;
	Animator animator;
	// Use this for initialization
	void OnEnable () {
		animator = GetComponent<Animator> ();
		//Invoke("disableObject",waitTime);
		StartCoroutine (disableObject());
	}

	IEnumerator disableObject(){
		yield return new WaitForSeconds (waitTime);
		animator.SetTrigger("FadeIn");
		yield return new WaitForSeconds (0.5f);
		gameObject.SetActive(false);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
