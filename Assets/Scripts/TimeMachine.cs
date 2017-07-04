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
	private List<bool> enableList;
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
		enableList = new List<bool> ();

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
				if (travelList [i].AdjustTimeReverse (time_c)) {
					if (enableList [i]) {
						travelList [i].AdjustPosition (formerList [i]);
						Action aux = travelList [i].GetAction ();
						formerList [i].GetComponent<Movement> ().Move (aux.GetDirection (), aux.GetHold (), aux.GetJump ());
					} else {
						if (travelList [i].IsEnable ()) {
							enableList [i] = true;
							Enable (formerList [i]);
						}
					}

				} else if (index == i) {
					EndTimeTravel ();
				}

			}
		} else {
			for (int i = 0; i < index; i++) {
				if (enableList [i]) {
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
					}
				} else {
					travelList [i].AddNullAction (transform.position,time_c);
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
		enableList.Add (true);

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
					enableList [i] = false;
					Disable (formerList [i]);
				}
	}
	void OnTriggerExit(Collider other){
		//	If the traveller has exit the flag change.
		if( travellerInside && other.gameObject == traveller )
			travellerInside = false;
	}
		
}