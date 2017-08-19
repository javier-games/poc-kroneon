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
	private int times;

	void OnTriggerEnter(Collider other){
		if (other.CompareTag (tagName) && times <= 0) {
			balloon.SetActive (true);
			balloon.transform.GetChild (0).GetComponent<Text> ().text = message;
			//StartCoroutine (CloseMessage( messageDuration) );
		} else {
			times--;
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag (tagName)) {
			balloon.transform.GetChild (0).GetComponent<Text> ().text = "";
			balloon.SetActive (false);
		}
	}

	/*IEnumerator CloseMessage(float timeToWait){
		yield return new WaitForSeconds (timeToWait);

	}*/


}
