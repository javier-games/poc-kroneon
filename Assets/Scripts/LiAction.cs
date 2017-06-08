using UnityEngine;
using System.Collections;
using System;

namespace Kroneon.Li{

	// This is an action in an specific time

	public class LiAction{

		// Variables
		private Vector3 dir ;		// Stores the control of direction that was did it by Li 
		private Vector3 pos ;		// Stores the position of the action
		private float	time;		// The moment in time when Li did the action
		private bool 	hold;		// Stores the control of hold that was did it by Li  
		private bool 	jump;		// Stores the control of jump that was did it by Li

		// Constructor
		public LiAction(Vector3 dir, Vector3 pos, float time, bool hold, bool jump){
			//Initialization
			this.dir  = dir ;
			this.pos  = pos ;
			this.hold = hold;
			this.jump = jump;
			this.time = time;
		}

		// Get Methods
		public Vector3 GetDirection(){
			return dir;
		}
		public bool GetHold(){
			return hold;
		}
		public bool GetJump(){
			return jump;
		}
		public float GetTime(){
			return time;
		}
		public Vector3 GetPosition(){
			return pos;
		}

		// Debuging Methods
		public void PrintAction(){
			Debug.Log (dir + " " + pos + " " + time + " " + hold + " " + jump);
		}

	}
}
