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
			animator.SetBool ("Pushed",true);
			source.PlayOneShot (pushClip);
		}
	}

	void OnTriggerStay(Collider other){
		if (other.CompareTag ("Player")) {
			pressed = true;
		}
	}

	public bool IsPressed(){
		if (pressed) {
			pressed = false;
			return true;
		} else if(animator.GetBool("Pushed")){
			animator.SetBool ("Pushed",false);
			source.PlayOneShot (releaseClip);
		}
		return pressed;
	}

}
