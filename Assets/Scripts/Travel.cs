using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kroneon.Li{

	//This is a list of actions that represent a travel

	public class Travel{


		//Variables
		private List<Action> 	actionList;		// List to store actions
		//private float  			time_f = 0;		// Total time of the travel
		private int 			index  = 0;		// Index of the current time
		private	int				count  = 0;		// Count of actions in the list
		private bool			ended  = false;	// 


		// Constructor - Initialization
		public Travel(){
			actionList = new List<Action> ();
		}


		//Get Methods
		//	Return the last or current action
		public Action GetAction(){
			return ended ? actionList [count-1] : actionList [index];
		}
		//	Return the time of the index action
		public float GetTime( int index){
			return ended ? actionList [count-1].GetTime() : actionList [index].GetTime ();
		}


		//Set Methods
		//	Define the last stade 
		public void EndTravel(){
			//	Define the total count of actions in the travel
			count = actionList.Count;
			//	The las istade is idle
			actionList [count - 1] = new Action (Vector3.zero,actionList [count - 1].GetPosition(),actionList [count - 1].GetTime(),false,false);
			//time_f = actionList [count - 1].GetTime ();
		}
		//	Add a new action to the list
		public void AddAction( Vector3 dir, Vector3 pos, float time, bool hold, bool jump){
			actionList.Add (new Action(dir,pos,time,hold,jump));
		}
		//	Set the follow values to back in time
		public void Reset(){
			index = 0;
			ended = false;
		}


		//Update Methods
		//	Adjust the action of the former to the current time
		public void TimeUpdate( float time_c){
			//	Stop increase the index (Avoid OutOfRange)
			ended = index > count-3;
			if (!ended)
				//	Adjusting the index	
				while (time_c > GetTime (index)) {
					//	Bigger Step
					if (time_c > GetTime (index))
						index += 2;
					//	Normal Step
					else
						index++;
				}
		}
		//	Adjust the current position of the former
		public void PositionUpdate( Movement former ){
			former.gameObject.GetComponent<Transform> ().position = ended ? actionList [count - 1].GetPosition () : actionList [index].GetPosition ();
		}
		public void PositionUpdate( GameObject former ){
			former.GetComponent<Transform> ().position = ended ? actionList [count - 1].GetPosition () : actionList [index].GetPosition ();
		}

			
		//Debuging Methods
		//	Show the complete list of actions
		public void ShowList(){
			foreach(Action action in actionList){
				action.PrintAction ();
			}
		}


	}
}
