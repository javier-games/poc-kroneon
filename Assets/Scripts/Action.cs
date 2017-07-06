using UnityEngine;
using System.Collections;
using System;

namespace Kroneon.Li{

	//	This is an action in an specific time

	public class Action{

		//	Variables
		private Vector3 dir ;		// Stores the control of direction that was did it by Li 
		private Vector3 pos ;		// Stores the position of the action
		private float	time;		// The moment in time when Li did the action
		private bool 	hold;		// Stores the control of hold that was did it by Li
		private bool	enable;		// Determinate if the former is enable.

		//	Constructor
		public Action(Vector3 dir, Vector3 pos, float time, bool hold, bool enable){
			//Initialization
			this.dir  	= dir ;
			this.pos  	= pos ;
			this.hold 	= hold;
			this.time 	= time;
			this.enable = enable;
		}

		//	Get Methods
		public Vector3 GetDirection(){
			return dir;
		}
		public bool GetHold(){
			return hold;
		}
		public float GetTime(){
			return time;
		}
		public Vector3 GetPosition(){
			return pos;
		}
		public bool IsEnable(){
			return enable;
		}

		// Debuging Methods
		public void PrintAction(){
			Debug.Log (dir + " " + pos + " " + time + " " + hold);
		}

	}
}
