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
			return actionList [i];
		}
		public Action GetAction(int i){
			return actionList [i];
		}
		public float GetTime(){
			return actionList [i].GetTime ();
		}
		public float GetTime( int i){
			return actionList[i].GetTime ();
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

			// Update in "Real Time"
			//Debug.Log ("T="+t_current+"   FT="+GetFinalTime ()+"   T-FT="+(t_current - GetFinalTime ())+"   Ti="+GetTime (i)+"   Ti+1="+GetTime (i+1));

			if (	(time_r - time_f) > GetTime(i)	) {
				i++;
				if (!((time_r - time_f) < GetTime (i+1)))
					i += 2;
			}
		}
		public void PositionUpdate( Movement past ){
			past.gameObject.GetComponent<Transform>().position = actionList [i].GetPosition ();
		}
		public void PositionUpdate( GameObject past ){
			past.GetComponent<Transform>().position = actionList [i].GetPosition ();
		}

	}
}
