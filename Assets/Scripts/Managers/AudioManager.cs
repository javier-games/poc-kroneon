using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;

	[SerializeField]
	private AudioMixer mainAudioMixer;
	[SerializeField]
	private AudioSource sourceGUI;
	[SerializeField]
	private AudioClip clickClip;

	void Awake(){
		instance = this;
	}

	void Start () {
		GameManager.instance.ChangeStateEvent += GameStateChange;
	}

	void GameStateChange(){
		switch(GameManager.instance.currentState){
		case GameState.PLAYING:
			break;
		case GameState.PAUSE:
			break;
		case GameState.CONTINUE:
			break;
		case GameState.GAME_OVER:
			break;
		}
	}
		
	public void SetSfxGroupVolume(float volume){
		mainAudioMixer.SetFloat ("SFXVolume",volume);
	}
	public void SetMusicGroupVolume(float volume){
		mainAudioMixer.SetFloat ("MusicVolume",volume);
	}
	public void ClickSFX(){
		sourceGUI.PlayOneShot (clickClip);
	}
}