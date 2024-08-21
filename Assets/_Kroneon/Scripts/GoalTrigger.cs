using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour {

	[SerializeField]
	private AudioClip winClip;
	[SerializeField]
	private float delayTime;

	private AudioSource source;

	void Start(){
		source = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player" || other.tag == "Respawn") {
			StartCoroutine (ChangeState(delayTime));
		}
	}

	IEnumerator ChangeState(float timeToWait){
		yield return new WaitForSeconds (timeToWait);
		GameManager.instance.ChangeToNewState (GameState.WIN);
		source.PlayOneShot (winClip);
	}
}
