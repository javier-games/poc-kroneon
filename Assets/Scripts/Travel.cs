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


		// Constructor - Initialization

		public Travel(){
			actionList = new List<Action> ();
		}



		//	Methods

		public void AddAction( Vector3 dir, Vector3 pos, float time, bool hold){
			actionList.Add (new Action(dir,pos,time,hold,true));
		}
		public void AddNullAction(Vector3 pos, float time){
			actionList.Add (new Action(Vector3.zero,pos,time,false,false));
			count++;
			final++;
			index++;
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
				true
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
		public bool IsEnable(){
			return actionList [index].IsEnable ();
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


	
	}
}