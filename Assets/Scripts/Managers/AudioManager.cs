using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

	public static AudioManager instance;
	public AudioSource musicSource;
	public AudioSource sfxSource;
	public AudioClip[]	sfxCollection;
	public AudioMixer mainAudioMixer;

	void Awake(){
		instance = this;
	}

	void Start () {
		GameManager.instance.ChangeStateEvent += GameStateChange;
		musicSource.Play();
	}

	void GameStateChange(){
		switch(GameManager.instance.currentState){
		case GameState.PLAYING:
			break;
		case GameState.PAUSE:
			musicSource.mute = true;
			break;
		case GameState.CONTINUE:
			musicSource.mute = false;
			break;
		case GameState.GAME_OVER:
			musicSource.Stop ();
			break;
		}
	}

	public void playShot(){
		sfxSource.PlayOneShot (sfxCollection[0]);
	}
	public void PlayWave(){
		sfxSource.PlayOneShot (sfxCollection[1]);
	}
	public void SetSfxGroupVolume(float volume){
		mainAudioMixer.SetFloat ("SoundFXVolume",volume);
	}
	public void SetMusicGroupVolume(float volume){
		mainAudioMixer.SetFloat ("MusicVolume",volume);
	}
}