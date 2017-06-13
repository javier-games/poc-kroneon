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
	private List<Travel> travelList;
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
		//	Getting the initial time
		time_i = Time.time;
	}


	//Public Methods
	//	 Add a new action to the current travel
	public void AddAction( Vector3 dir, Vector3 pos, bool hold, bool jump){
		time_c = Time.time - time_i;
		travelList [index].AddAction (dir,pos,time_c,hold,jump);
	}
	//	Move a prefab crontrolled by the time in previous lists
	public void Move (){
		if (index > 0) {
			/* TODO
			 * This is going to change when we spawn the prefabs
			 */
			//for (int j = 0; j < travelList.Count-1; j++) {
			travelList [index-1].PositionUpdate (formerPrefab);
			travelList [index-1].TimeUpdate (time_c);
			Action aux = travelList [index - 1].GetAction ();
			formerPrefab.GetComponent<Movement> ().Move (aux.GetDirection (), aux.GetHold (), aux.GetJump ());
			//}
		}
	}


	//Private Methods
	//	Back in time
	private void TimeTravel(){
		//	End the current travel and add a new one
		travelList[index].EndTravel();
		travelList.Add (new Travel());
		index++;
		/* TODO
		 * Here we need to generate a new spawn
		 */
		formerPrefab.SetActive (true);
		//	Back in Time
		ResetTravels ();
		time_i = Time.time;
	}
	// Move all the Travel Index to zero
	private void ResetTravels(){
		for (int i = 0; i < travelList.Count; i++)
			travelList [i].Reset ();
	}
	//	Trigger Methods
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


}
