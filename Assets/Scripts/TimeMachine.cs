using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kroneon.Li;

[RequireComponent(typeof(BoxCollider))]
public class TimeMachine : MonoBehaviour {


	[SerializeField] private GameObject	traveller;				//	The current character with movemoent
	[SerializeField] private GameObject	formerPrefab;			//	Prefab of the traveller
	[SerializeField] private int 		travelNum = 3;			//	Initial total times of the level.

	private List<Travel> travelList;
	private int i = 0;

	private bool travellerInside = true;						//	Flag with initial value.

	//	Use this for initialization
	void Start () {
		travelList = new List<Travel> ();
		travelList.Add (new Travel());
	}
		
	//	Public Methods
	public void AddAction( Vector3 dir, Vector3 pos, float time, bool hold, bool jump){
		travelList [i].AddAction (dir,pos,time,hold,jump);
	}
	public void Move (float time_c){
		if (i > 0) {
			//for (int j = 0; j < travelList.Count-1; j++) {
				travelList [i-1].PositionUpdate (formerPrefab);
				travelList [i-1].TimeUpdate (time_c);
				formerPrefab.GetComponent<Movement> ().Move (travelList [i-1].GetAction ().GetDirection (), travelList [i-1].GetAction ().GetHold (), travelList [i-1].GetAction ().GetJump ());
			//}
		}
	}


	//	Private Methods
	//		Trigger Methods
	void OnTriggerEnter(Collider other){
		//	If the traveller has enter he is going to travel in time and its travel couter decrease
		if ( !travellerInside && other.gameObject == traveller ) {
			travellerInside = true;
			travelNum -= 1;
			TimeTravel ();
		}
	}
	void OnTriggerExit(Collider other){
		//	If the traveller has exit the flag change.
		if( travellerInside && other.gameObject == traveller )
			travellerInside = false;
	}
	//	Timetravel
	private void TimeTravel(){
		travelList.Add (new Travel());
		travelList[i].SetFinalTime (Time.time);
		i++;
		formerPrefab.SetActive (true);
	}


}
