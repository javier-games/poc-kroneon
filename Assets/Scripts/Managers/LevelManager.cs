using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance;

	[SerializeField]
	private float gameTime = 45f;
	[SerializeField]
	private int travelCounter = 3; 

	void Awake(){
		instance = this;
	}

	void Start () {

		GameManager.instance.ChangeStateEvent += GameStateChange;
	}

	void GameStateChange(){
		switch(GameManager.instance.currentState){
		case GameState.START:
			break;
		case GameState.CUTSCENE:
			break;
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

	public float GetGameTime(){
		return gameTime;
	}
	public float GetTimeTravelCount(){
		return travelCounter;
	}


}
