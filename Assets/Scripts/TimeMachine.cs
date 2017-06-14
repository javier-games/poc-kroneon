using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kroneon.Li;

[RequireComponent(typeof(BoxCollider))]
public class TimeMachine : MonoBehaviour {


	[SerializeField] private GameObject	traveller;		//	The current character with movemoent
	[SerializeField] private GameObject	formerPrefab;	//	Prefab of the traveller
	[SerializeField] private int 		travelNum = 3;	//	Initial total times of the level.

	//	Time Travel Variables
	private List<Travel> travelList;					//	List of the travels
	private List<GameObject> formerList;					//	List of formers
	private int index = 0;
	private float time_i = 0;							//	Initial time of the play process
	private float time_c = 0;							//	Current time

	//	Trigger Variables
	private bool travellerInside = true;				//	Flag with initial value.


	//	Use this for initialization
	void Start () {
		//	Creatinhg a new list with a travel in it
		travelList = new List<Travel> ();
		travelList.Add (new Travel());
		//	Creating a new list for the formers
		formerList = new List<GameObject>();
		//	Getting the initial time
		time_i = Time.time;
	}


	//Public Methods
	//	 Add a new action to the current travel
	public void AddAction( Vector3 dir, Vector3 pos, bool hold, bool jump){
		//	Update the current time
		time_c = Time.time - time_i;
		//	Stop 
		if (travelNum > 0) {
			//	Update the current time
			travelList [index].AddAction (dir, pos, time_c, hold, jump);
		}
	}
	//	Move a prefab crontrolled by the time in previous lists
	public void Move (){
		//	If the traveller has travel in time, then...
		if (index > 0)
			//	For each travel
			for (int i = 0; i < index; i++)
				//	If the travel is active, then...
				if (formerList [i].activeInHierarchy) {
					//	Update time and position of the traveller
					travelList [i].PositionUpdate (formerList [i]);
					travelList [i].TimeUpdate (time_c);
					//	And animate him
					Action aux = travelList [i].GetAction ();
					formerList [i].GetComponent<Movement> ().Move (aux.GetDirection (), aux.GetHold (), aux.GetJump ());
				}
	}


	//Private Methods
	//	Back in time
	private void TimeTravel(){		
		//	End the current travel and add a new one
		travelList [index].EndTravel ();
		travelList.Add (new Travel ());
		//	Generating a new former
		GameObject former = (GameObject)Instantiate (formerPrefab, transform.position, transform.rotation);
		formerList.Add (former);
		//	Increassing the index
		index++;
		//	Back in Time
		ResetTravels ();
		time_i = Time.time;
		travelNum --;
	}
	// Reset all the travels
	private void ResetTravels(){
		// Move all the Travel Indexes to zero
		for (int i = 0; i < travelList.Count; i++) {
			travelList [i].Reset ();
			// Re-activate all the formers
			if (i < formerList.Count) {
				formerList [i].SetActive (true);
			}
		}
	}
	//	Trigger Methods
	void OnTriggerEnter(Collider other){
		//	If the traveller has enter he is going to travel in time
		if ( !travellerInside && other.gameObject == traveller ) {
			travellerInside = true;
			if(travelNum>0)
				TimeTravel ();
		}
		//	If the respawn enter to the machine it set to unactive
		if (other.tag == "Respawn")
			for (int i = 0; i < index; i++)
				if (travelList [i].Ended ())
					other.gameObject.SetActive (false);
	}
	void OnTriggerExit(Collider other){
		//	If the traveller has exit the flag change.
		if( travellerInside && other.gameObject == traveller )
			travellerInside = false;
	}


}
