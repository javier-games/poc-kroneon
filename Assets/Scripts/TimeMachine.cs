using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kroneon.Li;
using UnityEngine.PostProcessing;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class TimeMachine : MonoBehaviour {



	//	Serialized Variables

	[SerializeField] private GameObject	traveller;			//	The current character with movemoent
	[SerializeField] private GameObject	formerPrefab;		//	Prefab of the traveller
	[SerializeField] private int 		travelNum = 3;		//	Initial total times of the level.
	[SerializeField] private float 		delay = 2;			//	Delay per time travel

	//	Level
	[SerializeField] private Image		levelTime;
	[SerializeField] private float 		totalTime = 60.0f;
	[SerializeField] private Image		levelWaitTime;
	[SerializeField] private float		waitTime = 5.0f;


	//	Private Variables

	private List<Travel> travelList;						//	List of the travels
	private List<GameObject> formerList;					//	List of formers
	private int index = 0;									//	Index of travels
	private float time_i = 0;								//	Initial time of the play process
	private float time_c = 0;								//	Current time
	private bool traveling = false;

	//	Trigger Variables

	private bool travellerInside = true;

	//	Initialization

	void Start () {
		
		travelList = new List<Travel> ();
		travelList.Add (new Travel());

		formerList = new List<GameObject>();

		time_i = Time.time;

		Camera.main.transform.GetComponent<PostProcessingBehaviour> ().profile.motionBlur.enabled = false;


		// Level
		levelTime.fillAmount = 0;
		levelWaitTime.fillAmount = 0;

	}



	//Public Methods

	public void AddAction( Vector3 dir, Vector3 pos, bool hold, bool jump){

		time_c = Time.time - time_i;
		if(time_c > delay && !traveling)
			travelList [index].AddAction (dir, pos, time_c, hold, jump);

	}
		
	public void MoveFormers (){

		time_c = Time.time - time_i;

		// Level
		if (traveling) {
			levelTime.fillAmount = time_c / totalTime;
			levelWaitTime.fillAmount = 1 -time_c / waitTime;
		} else {
			levelTime.fillAmount = 1 - time_c / totalTime;
			levelWaitTime.fillAmount = time_c / waitTime;
		}


		if (traveling) {
			for ( int i = 0; i <= index && traveling; i++) {
				if (!travelList[i].DidTimeTravelFinished()) {
					if (travelList [i].AdjustTimeReverse (time_c)) {
						travelList [i].AdjustPosition (formerList [i]);
						Action aux = travelList [i].GetAction ();
						formerList [i].GetComponent<Movement> ().Move (aux.GetDirection (), aux.GetHold (), aux.GetJump ());
					} else if (index == i) {
						EndTimeTravel ();
					}
				} else {
					
				}
			}
		} else {
			for (int i = 0; i < index; i++) {
				if (!travelList [i].DidTimeTravelFinished ()) {
					if (travelList [i].AdjustTime (time_c)) {
						travelList [i].AdjustPosition (formerList [i]);
						Action aux = travelList [i].GetAction ();
						formerList [i].GetComponent<Movement> ().Move (
							aux.GetDirection (),
							aux.GetHold (),
							aux.GetJump ()
						);
					} else {
						formerList [i].GetComponent<Movement> ().Move (
							Vector3.zero,
							false,
							false
						);
						travelList [i].SetDisableTime (time_c);
					}
				}
			}
		}
	}



	//Private Methods

	private void BegingTimeTravel(){

		travelList [index].EndTravel ();
		travelList.Add (new Travel ());
		time_i = Time.time;
		time_c = Time.time - time_i;

		Disable (traveller);

		Action temp = travelList [index].GetAction ();
		GameObject former = (GameObject)Instantiate (
			formerPrefab, 
			temp.GetPosition(), 
			Quaternion.FromToRotation(Vector3.forward,temp.GetDirection())
		);
		formerList.Add (former);

		traveling = true;
		for (int i = 0; i <= index; i++) {
			travelList [i].SetIndexAsEnd ();
			formerList[i].GetComponent<Animator> ().SetFloat ("Speed",-1);
		}

		Camera.main.transform.GetComponent<PostProcessingBehaviour> ().profile.motionBlur.enabled = true;
		Time.timeScale = 2.5f;


	}

	private void EndTimeTravel(){

		time_i = Time.time;
		travelNum --;

		traveling = false;
		for (int i = 0; i <= index; i++) {
			travelList [i].SetIndexAsStart ();
			formerList[i].GetComponent<Animator> ().SetFloat ("Speed",1);
		}
		traveller.transform.position = transform.position;
		traveller.transform.rotation = transform.rotation;

		Enable (traveller);

		Camera.main.transform.GetComponent<PostProcessingBehaviour> ().profile.motionBlur.enabled = false;
		Time.timeScale = 1f;
		index++;

	}


	private void Disable(GameObject model){
		model.GetComponentInChildren<SkinnedMeshRenderer> ().enabled =false;
		model.GetComponent<CapsuleCollider>().enabled = false;
		model.GetComponent<Rigidbody>().isKinematic = true;
		if(model.name == traveller.name)
			model.GetComponent<Controller> ().MovementActive = false;
	}
	private void Enable(GameObject model){
		model.GetComponentInChildren<SkinnedMeshRenderer> ().enabled =true;
		model.GetComponent<Rigidbody>().isKinematic = false;
		model.GetComponent<CapsuleCollider>().enabled = true;
		if(model.name == traveller.name)
			model.GetComponent<Controller> ().MovementActive = true;
	}



	//	Trigger Methods

	void OnTriggerEnter(Collider other){
		//	If the traveller has enter he is going to travel in time
		if ( !travellerInside && other.tag == "Player" && time_c > delay) {
			travellerInside = true;
			if (travelNum > 0)
				BegingTimeTravel ();
		}
		if (other.tag == "Respawn")
			for (int i = 0; i < index; i++)
				if (travelList [i].Ended ()) {

					Disable (formerList [i]);
					travelList [i].SetDisableTime (time_c);
				}
	}
	void OnTriggerExit(Collider other){
		//	If the traveller has exit the flag change.
		if( travellerInside && other.gameObject == traveller )
			travellerInside = false;
	}











}


































































/*
[RequireComponent(typeof(BoxCollider))]
public class TimeMachine : MonoBehaviour {


	[SerializeField] private GameObject	traveller;		//	The current character with movemoent
	[SerializeField] private GameObject	formerPrefab;	//	Prefab of the traveller
	[SerializeField] private int 		travelNum = 3;	//	Initial total times of the level.

	//	Time Travel Variables
	private List<Travel> travelList;					//	List of the travels
	private List<GameObject> formerList;				//	List of formers
	private int index = 0;								//	Index of travels
	private float time_i = 0;							//	Initial time of the play process
	private float time_c = 0;							//	Current time
	[SerializeField]
	private float delay = 5;							//	Delay per time travel
	private bool traveling = false;						//	Flag for timetravel

	//	Trigger Variables
	private bool travellerInside = true;				//	Flag for eneable animation backwards


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
		//	If traveller and the machibe are not traveling.
		if (!traveling) {
			//	Stop 
			if (travelNum > 0 && time_c > delay) {
				//	Update the current time
				travelList [index].AddAction (dir, pos, time_c - delay, hold, jump);
			}
		}
	}
	//	Move a prefab crontrolled by the time in previous lists
	public void Move (){
		
		//	Update the current time
		time_c = Time.time - time_i;

		//	If traveller and the machibe are not traveling
		if (!traveling) {
			//	If the traveller has travel in time, then...
			if (index > 0) {
				//	For each travel
				for (int i = 0; i < index; i++) {
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
			}
		}

		//	If traveller and the machibe are traveling
		else {
			
			for ( int i = 0; i < index; i++ ){
				if (formerList [i].activeInHierarchy) {
					
					if (travelList [i].TimeReverse (time_c)) {
						travelList [i].PositionUpdate (formerList [i]);
						//	And animate him
						Action aux = travelList [i].GetAction ();
						formerList [i].GetComponent<Movement> ().Move (aux.GetDirection (), aux.GetHold (), aux.GetJump ());
					} else {
						if( i == index-1)
							EndTimeTravel ();
					}

				}else{
					//	Wait to appear again
				}
			}

		}
	}

	//Private Methods
	private void BegingTimeTravel(){
		
		//	End the current travel and add a new one
		travelList [index].EndTravel ();
		travelList.Add (new Travel ());
		//	Increassing the index
		index++;

		// The traveller change apparently to unactive.
		traveller.GetComponentInChildren<SkinnedMeshRenderer> ().enabled =false;
		traveller.GetComponent<Controller> ().MovementActive = false;

		//	Genetation of a former
		travelList [index-1].SetIndexToEnd ();
		Action temp = travelList [index-1].GetAction ();
		GameObject former = (GameObject)Instantiate (
			formerPrefab, 
			temp.GetPosition(), 
			Quaternion.FromToRotation(Vector3.forward,temp.GetDirection())
		);
		formerList.Add (former);

		//	Set animations backwards
		traveling = true;
		for (int i = 0; i < index; i++) {
			formerList[i].GetComponent<Animator> ().SetFloat ("Speed",-1);
		}
	}

	private void EndTimeTravel(){
		//	Back in Time
		//ResetTravels ();
		time_i = Time.time;
		travelNum --;
		//	Set animations backwards
		traveling = false;
		for (int i = 0; i < index; i++) {
			formerList[i].GetComponent<Animator> ().SetFloat ("Speed",1);
		}
		//	Reactivate the traveller
		traveller.transform.position = transform.position;
		traveller.transform.rotation = transform.rotation;
		traveller.GetComponentInChildren<SkinnedMeshRenderer> ().enabled = true;
		traveller.GetComponent<Controller> ().MovementActive = true;
	}

	//	Trigger Methods
	void OnTriggerEnter(Collider other){
		//	If the traveller has enter he is going to travel in time
		if ( !travellerInside && other.gameObject == traveller && time_c > delay ) {
			travellerInside = true;
			if (travelNum > 0)
				BegingTimeTravel ();
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
*/