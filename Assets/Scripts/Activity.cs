using UnityEngine;
using System.Collections;
using System;

	//	This is an action in an specific time

	public class Activity{

		//	Variables
		private Vector3 pos ;		// Stores the position of the action
		private float	time;		// The moment in time when Li did the action

		//	Constructor
	public Activity(Vector3 pos, float time){
			//Initialization
			this.pos  	= pos ;
			this.time 	= time;
		}

		public float GetTime(){
			return time;
		}
		public Vector3 GetPosition(){
			return pos;
		}

		// Debuging Methods
		public void PrintAction(){
			Debug.Log (pos + " " + time);
		}

	}