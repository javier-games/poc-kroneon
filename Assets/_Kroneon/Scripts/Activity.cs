using UnityEngine;
using System.Collections;
using System;

public class Activity{
	
	private Vector3 pos ;
	private float	time;

	public Activity(Vector3 pos, float time){
		this.pos  	= pos ;
		this.time 	= time;
	}

	public float GetTime(){
		return time;
	}
	public Vector3 GetPosition(){
		return pos;
	}
	public void PrintAction(){
		Debug.Log (pos + " " + time);
	}
}