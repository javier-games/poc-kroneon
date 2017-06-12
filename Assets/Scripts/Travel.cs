using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kroneon.Li{

	//This is a list of actions that represent a travel of Li

	public class Travel{


		//Variables
		private List<Action> 	actionList;		// List to store actions
		private float  			time_f = 0;		// Total time of the travel
		private int 			i=0;			// Index of the current time


		// Constructor - Initialization
		public Travel(){
			actionList = new List<Action> ();
		}


		// Get Methods
		public Action GetAction(){
			if ( i < actionList.Count )
				return actionList [i];
			else
				return actionList [actionList.Count-1];
		}
		public Action GetAction(int i){
			if ( i < actionList.Count )
				return actionList [i];
			else
				return actionList [actionList.Count-1];
		}
		public float GetTime(){
			if (i < actionList.Count)
				return actionList [i].GetTime ();
			else
				return actionList [actionList.Count - 1].GetTime ();
		}
		public float GetTime( int i){
			if (i < actionList.Count)
				return actionList [i].GetTime ();
			else
				return actionList [actionList.Count - 1].GetTime ();
		}
		public float GetFinalTime(){
			return time_f;
		}


		// Add and Set Methods
		public void SetFinalTime(float time){
			time_f = time;
		}
		public void AddAction( Vector3 dir, Vector3 pos, float time, bool hold, bool jump){
			actionList.Add (new Action(dir,pos,time,hold,jump));
		}
		public void Next(){
			i++;
		}
		public void Clear(){
			actionList.Clear ();
		}


		//Debuging Methods
		public void ShowList(){
			foreach(Action action in actionList){
				action.PrintAction ();
			}
		}

		// Update Methods
		public void TimeUpdate( float time_r){
			if (i < actionList.Count) {
				if ((time_r - time_f) > GetTime (i)) {
					i++;
					if (!((time_r - time_f) < GetTime (i + 1)))
						i += 2;
				}
			}
		}
		public void PositionUpdate( Movement past ){
			if( i < actionList.Count )
				past.gameObject.GetComponent<Transform>().position = actionList [i].GetPosition ();
			else {
				past.gameObject.GetComponent<Transform>().position = actionList [actionList.Count-1].GetPosition ();
			}
		}
		public void PositionUpdate( GameObject past ){
			if( i < actionList.Count )
				past.GetComponent<Transform>().position = actionList [i].GetPosition ();
			else {
				past.GetComponent<Transform>().position = actionList [actionList.Count-1].GetPosition ();
			}
		}


	}
}
