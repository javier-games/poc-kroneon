using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrandpaController : MonoBehaviour {

	[SerializeField]
	private Transform target;
	[SerializeField]
	private string message;
	[SerializeField]
	private GameObject balloon;
	[SerializeField]
	private float messageDuration;
	[SerializeField]
	private int times;
	[SerializeField]
	private bool lookAt = true;

	void Start () {
		StartCoroutine (OpenMessage(messageDuration,times));
	}

	void Update () {
		if(lookAt)
			transform.LookAt (target);
	}

	IEnumerator OpenMessage(float timeToWait,int timesResidue){
		if (timesResidue > 0) {
			yield return new WaitForSeconds (timeToWait);
			balloon.SetActive (true);
			balloon.transform.GetChild (0).GetComponent<Text> ().text = message;
			StartCoroutine (CloseMessage(messageDuration,timesResidue));
		}
	}

	IEnumerator CloseMessage(float timeToWait,int timesResidue){
		yield return new WaitForSeconds (timeToWait);
		balloon.transform.GetChild (0).GetComponent<Text> ().text = "";
		balloon.SetActive (false);
		StartCoroutine (OpenMessage(messageDuration,timesResidue-1));
	}
}
