using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Kroneon.Li{
	
	public class Travel{



		//Variables

		private List<Action> 	actionList;			// List to store actions
		private int 			index	= 0;		// Index of the current time
		private	int				count	= 0;		// Count of actions in the list
		private int				final	= 0;		//
		private int				start	= 0;
		private float	  		disableTime = 0;



		// Constructor - Initialization

		public Travel(){
			actionList = new List<Action> ();
		}



		//	Methods

		public void AddAction( Vector3 dir, Vector3 pos, float time, bool hold, bool jump){
			actionList.Add (new Action(dir,pos,time,hold,jump));
		}
		public void EndTravel(){

			count = actionList.Count;
			start = 0;
			final = count - 1;
			index = final;
			actionList [final] = new Action (
				Vector3.zero,
				actionList [count - 1].GetPosition(),
				actionList [count - 1].GetTime(),
				false,
				false
			);
		}
		public void SetIndexAsEnd(){
			final = index;
		}
		public void SetIndexAsStart(){
			start = index;
			/*for(int i = 0; i<start; i++){
				actionList.RemoveAt (0);
				index--;
				final--;
				count--;
			}
			start = index;*/
		}
		public bool Ended(){
			return index >= count - 1; 
		}
		public Action GetAction(){
			return actionList [index];
		}

		public bool AdjustTime(float time_c){
			bool outOfRange = false;
			while( time_c > actionList[index].GetTime() - actionList[start].GetTime() && !outOfRange ){
				index += 2;
				if (index > count - 1) {
					outOfRange = true;
					index = count - 1;

				}
			}
			return !outOfRange;
		}

		public bool AdjustTimeReverse(float time_c){
			bool outOfRange = false;
			while (time_c > actionList [final].GetTime() - actionList [index].GetTime() && !outOfRange) {
				index -= 2;
				if (index < 0) {
					index = 0;
					outOfRange = true;
				}
			}
			return !outOfRange;
		}

		public void AdjustPosition(GameObject former){
			former.transform.position = actionList [index].GetPosition ();
		}

		public void ShowList(){
			Debug.Log ("----------------------------------");
			foreach(Action action in actionList){
				action.PrintAction ();
			}
		}

		public void SetDisableTime(float disableTime){
			this.disableTime = disableTime;
		}

		public float GetDisableTime(){
			return disableTime;
		}
			
		public bool DidTimeTravelFinished(){
			return disableTime > 0 ? true : false;
		}

		public bool TimeToEnable(float currentTime, ref float lastTime){
			Debug.Log (disableTime+" "+currentTime+" "+lastTime);
			disableTime = disableTime - (currentTime - lastTime);
			lastTime = currentTime;


			if( disableTime<=0 ){
				
				disableTime = 0;
				Debug.Log (disableTime+" "+currentTime+" "+lastTime);
				return true;
			}
			else{Debug.Log (disableTime+" "+currentTime+" "+lastTime);
				return false;}
			
		}
	
	}
}




































































/*
namespace Kroneon.Li{

	//This is a list of actions that represent a travel

	public class Travel{
		
		//Variables
		private List<Action> 	actionList;		// List to store actions
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
		public bool Ended(){
			return ended;
		}
		public void SetIndexToEnd(){
			index = count - 1; 
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
			//	Stop increase the index (Avoid OutOfRange)
			ended = index > count-3;
		}
		//	Adjust the action of the former to the last time
		public bool TimeReverse( float time_c){
			//	Stop decrease the index (Avoid OutOfRange)
			index-=1;
			if (index < 0) {
				index = 0;
				return false;
			}
			return true;
		}
		//	Adjust the current position of the former
		public void PositionUpdate( Movement former ){
			former.gameObject.GetComponent<Transform> ().position = ended ? actionList [count - 1].GetPosition () : actionList [index].GetPosition ();
		}
		public void PositionUpdate( GameObject former ){
			former.GetComponent<Transform> ().position = ended ? actionList [count - 1].GetPosition () : actionList [index].GetPosition ();
			Debug.Log (former.name + " " + former.transform.position + " " + index + " " + ended);
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
*/