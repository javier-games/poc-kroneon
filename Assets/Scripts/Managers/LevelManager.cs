using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public static LevelManager instance;

	[SerializeField]
	private float gameTime = 45f;
	private float gameTimeAmount;
	[SerializeField]
	private float timeUpdateAmount = 0.1f;
	[SerializeField]
	private int travelCounter = 3;


	public delegate void ChangedTime();
	public ChangedTime ChangedTimeEvent;
	public delegate void ChangedTravelCount ();
	public ChangedTravelCount ChangedTravelCountEvent;


	void Awake(){
		instance = this;
		ChangedTimeEvent += TimeDidChange;
		ChangedTravelCountEvent += TravelCountDidChange;
	}

	void Start () {
		GameManager.instance.ChangeStateEvent += GameStateChange;
		gameTimeAmount = gameTime;
	}

	void GameStateChange(){
		switch(GameManager.instance.currentState){
		case GameState.START:
			StopCoroutine ("TimeUpdate");
			gameTimeAmount = gameTime;
			break;
		case GameState.CUTSCENE:
			StopCoroutine ("TimeUpdate");
			break;
		case GameState.PLAYING:
			StartCoroutine ("TimeUpdate",timeUpdateAmount);
			break;
		case GameState.PAUSE:
			StopCoroutine ("TimeUpdate");
			break;
		case GameState.CONTINUE:
			StartCoroutine ("TimeUpdate",timeUpdateAmount);
			break;
		case GameState.GAME_OVER:
			StopCoroutine ("TimeUpdate");
			break;
		}

	}

	public float GetGameTime(){
		return gameTime;
	}
	public float GetGameTimeAmount(){
		return gameTimeAmount;
	}
	public float GetTravelCount(){
		return travelCounter;
	}
	public void DecreaseTravelCount(){
		if (travelCounter > 0) {
			travelCounter--;
			ChangedTravelCountEvent ();
		}
	}
	public void DecreaseGameTime(float timeToDecrease){
		if (gameTimeAmount - timeToDecrease > 0) {
			gameTimeAmount -= timeToDecrease;
		} else {
			GameManager.instance.ChangeToNewState (GameState.GAME_OVER);
			StopCoroutine ("TimeUpdate");
		}
		
	}
	private IEnumerator TimeUpdate(float timeToWait){
		yield return new WaitForSeconds (timeToWait);
		ChangedTimeEvent ();
		if (gameTimeAmount - TimeMachine.instance.GetCurrentTime () < 0f) {
			GameManager.instance.ChangeToNewState (GameState.GAME_OVER);
			StopCoroutine ("TimeUpdate");
		} else {
			StartCoroutine ("TimeUpdate", timeUpdateAmount);
		}
	}


	//Anouncements
	private void TimeDidChange(){
		//Debug.Log("The time has changed to " + gameTime);
	}
	private void TravelCountDidChange(){
		//Debug.Log("The travel count has changed to " + travelCounter);
	}

}