using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	
public class Travel{



	//Variables

	private List<Action> 	actionList;			// List to store actions
	private int 			index	= 0;		// Index of the current time	


	// Constructor - Initialization

	public Travel(){
		actionList = new List<Action> ();
	}



	//	Methods

	public void AddAction( Vector3 pos, float time){
		actionList.Add (new Action(pos,time));
	}

	public Action GetAction(){
		return actionList [index];
	}
	public Action GetAction(int i){
		return actionList [i];
	}

	public void SetIndex(int index){
		this.index = index;
	}

	public bool TimeToSetDestination(float currentTime){
		bool setDestination = false;
		Debug.Log (currentTime +" "+ actionList[index].GetTime() +" "+ actionList[0].GetTime());
		if( currentTime > actionList[index].GetTime() - actionList[0].GetTime()){
			index ++;
			if (index > actionList.Count - 1) {
				//setDestination = true;
				index = actionList.Count - 1;
			}
		}
		return !setDestination;
	}


	/*
	public void EndTravel(Vector3 endPosition,float endTime){
		AddAction (endPosition,endTime);
		count = actionList.Count;
		index = count-1;
	}
	public bool Ended(){
		return index >= count - 1; 
	}

	public bool AdjustTime(float currentTime){
		bool outOfRange = false;
		if( currentTime > actionList[index].GetTime() - actionList[0].GetTime() && !outOfRange ){
			index ++;
			if (index > count - 1) {
				outOfRange = true;
				index = count - 1;
			}
		}
		return !outOfRange;
	}

	public bool AdjustTimeReverse(float currentTime){
		bool outOfRange = false;
		if (currentTime > actionList [count-1].GetTime() - actionList [index].GetTime()) {
			index --;
			if (index < 0) {
				index = 0;
				outOfRange = true;
			}
		}
		return !outOfRange;
	}*/

	public void ShowList(){
		Debug.Log ("----------------------------------");
		foreach(Action action in actionList){
			action.PrintAction ();
		}
	}



}