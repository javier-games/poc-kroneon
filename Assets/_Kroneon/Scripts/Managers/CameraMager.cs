using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMager : MonoBehaviour {

	public static CameraMager instance;

	[SerializeField] private Transform fPController;
	[SerializeField] private Transform railCamera;
	[SerializeField] private Transform gUICamera;

	void Awake(){
		instance = this;
	}

	void Start () {
		
		GameManager.instance.ChangeStateEvent += GameStateChange;

		StopCameras ();
		gUICamera.gameObject.SetActive (true);
	}

	private void StopCameras(){
		fPController.gameObject.SetActive (false);
		railCamera.gameObject.SetActive (false);
		gUICamera.gameObject.SetActive (false);
	}
		
	void GameStateChange(){
		switch(GameManager.instance.currentState){
		case GameState.START:
			StopCameras ();
			gUICamera.gameObject.SetActive (true);
			break;
		case GameState.CUTSCENE:
			StopCameras ();
			railCamera.gameObject.SetActive (true);
			break;
		case GameState.PLAYING:
			StopCameras ();
			fPController.gameObject.SetActive (true);
			break;
		case GameState.PAUSE:
			StopCameras ();
			gUICamera.gameObject.SetActive (true);
			break;
		case GameState.CONTINUE:
			StopCameras ();
			fPController.gameObject.SetActive (true);
			break;
		case GameState.GAME_OVER:
			break;
		}
	}
}
