using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour {

	[SerializeField]
	private AudioClip pushClip;
	[SerializeField]
	private AudioClip releaseClip;

	private AudioSource source;
	private Animator animator;
	private bool pressed = false;

	void Start(){
		source = GetComponent<AudioSource> ();
		animator = transform.GetChild(0).GetComponent<Animator> ();
	}

	void OnTriggerEnter(Collider other){
		if (other.CompareTag ("Player")) {
			pressed = true;
			animator.SetBool ("Pushed",true);
			source.PlayOneShot (pushClip);
			if(other.GetComponent<PickingController> ())
				other.GetComponent<PickingController> ().SetCanTravel (false);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.CompareTag ("Player")) {
			pressed = false;
			animator.SetBool ("Pushed",false);
			source.PlayOneShot (releaseClip);
			if(other.GetComponent<PickingController> ())
				other.GetComponent<PickingController> ().SetCanTravel (true);
		}
	}

	public bool IsPressed(){
		return pressed;
	}
}
