using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour {

	[SerializeField]
	private AudioClip winClip;

	private AudioSource source;

	void Start(){
		source = GetComponent<AudioSource> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player" || other.tag == "Respawn") {
			GameManager.instance.ChangeToNewState (GameState.WIN);
			source.PlayOneShot (winClip);
		}
	}
}
