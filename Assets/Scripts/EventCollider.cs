using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class EventCollider : MonoBehaviour {


	[SerializeField]
	private string message;
	[SerializeField]
	private GameObject balloon;
	[SerializeField]
	private string tagName;
	[SerializeField]
	private float messageDuration;
	[SerializeField]
	private float messageDelay;
	[SerializeField]
	private int times;
	[SerializeField]
	private GameObject[] appearances;
	[SerializeField]
	private float[] appearancesDelay;

	void Start(){
		for (int i = 0; i < appearances.Length; i++)
			appearances [i].SetActive (false);
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag (tagName) && times <= 0) {

			if (messageDelay > 0f)
				StartCoroutine (SendMessage (messageDelay));
			else if( message!= "" ){
				balloon.SetActive (true);
				balloon.transform.GetChild (0).GetComponent<Text> ().text = message;
			}

			for (int i = 0; i < appearances.Length; i++) {
				if (appearancesDelay [i] > 0) {
					StartCoroutine (Appear(appearancesDelay[i],i));
				} else {
					appearances [i].SetActive (true);
				}
			}

			if(messageDuration>0f && message != "" )
				StartCoroutine (CloseMessage( messageDuration+messageDelay ) );
			
		} else {
			times--;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag (tagName) && messageDuration<=0 ) {
			if (message != "") {
				balloon.transform.GetChild (0).GetComponent<Text> ().text = "";
				balloon.SetActive (false);
			}
		}
	}

	IEnumerator Appear(float timeToWait,int index){
		yield return new WaitForSeconds (timeToWait);
		appearances [index].SetActive (true);
	}

	IEnumerator SendMessage(float timeToWait){
		yield return new WaitForSeconds (timeToWait);
		balloon.SetActive (true);
		balloon.transform.GetChild (0).GetComponent<Text> ().text = message;
	}

	IEnumerator CloseMessage(float timeToWait){
		yield return new WaitForSeconds (timeToWait);
		balloon.transform.GetChild (0).GetComponent<Text> ().text = "";
		balloon.SetActive (false);
	}


}
