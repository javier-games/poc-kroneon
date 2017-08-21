using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainTheme : MonoBehaviour {

	[SerializeField]
	private AudioClip mainTheme;
	[SerializeField]
	private AudioClip startSound;

	private AudioSource audioSource;
	private bool reduse = false;

	void Start(){
		audioSource = GetComponent<AudioSource> ();
		audioSource.clip = mainTheme;
		audioSource.loop = true;
		audioSource.volume = 1f;
		audioSource.Play ();
		GUIManager.instance.ChangeStateEvent += ChangeState;
	}

	public void ChangeState(){
		switch (GUIManager.instance.currentState) {
		case GUIState.MENU:
			audioSource.loop = true;
			audioSource.volume = 1f;
			audioSource.Play ();
			break;

		case GUIState.VIDEO:
			audioSource.PlayOneShot (startSound);
			reduse = true;
			break;
		case GUIState.LOADING:
			audioSource.PlayOneShot (startSound);
			reduse = true;
			break;
		}
	}

	void Update(){
		if (reduse) {
			audioSource.volume = audioSource.volume - 0.005f;
			if (audioSource.volume <= 0f) {
				reduse = false;
				audioSource.volume = 0f;
			}
		}
	}
}
