using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
public class Travel{



	//Variables

	private List<Action> 	actionList;			//	List to store actions
	private float			startTime = 0;		//	Start time
	private int 			index	= 0;		//	Index of the current time


	// Constructor - Initialization

	public Travel(){
		actionList = new List<Action> ();
	}
		
	public Action GetAction(){
		return actionList [index];
	}
	public Action GetAction(int i){
		return actionList [i];
	}
	public float GetStartTime(){
		return startTime;
	}
	public void SetIndex(int index){
		this.index = index;
	}
	public void SetStartTime(float startTime){
		this.startTime = startTime;
	}

	//	Methods

	public void AddAction( Vector3 pos, float currentTime){
		actionList.Add (new Action(pos,currentTime-startTime));
	}
	public void AddAction( Vector3 pos, float currentTime,bool activator){
		actionList.Add (new Action(pos,currentTime));
	}

	public bool TimeToSetDestination(float currentTime){
		bool setDestination = false;
		if( currentTime > actionList[index].GetTime() - actionList[0].GetTime()){
			index ++;
			if (index > actionList.Count - 1) {
				setDestination = true;
				index = actionList.Count - 1;
			}
		}
		return !setDestination;
	}

	public void ShowList(){
		Debug.Log ("----------------------------------");
		foreach(Action action in actionList){
			action.PrintAction ();
		}
	}



}