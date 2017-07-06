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
	[SerializeField] private Image		levelWaitTimeHUD;
	[SerializeField] private Text 		textWaitTime;
	[SerializeField] private float		waitTime = 5.0f;
	private AudioSource sourse;


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
		sourse = levelTime.transform.parent.GetComponent<AudioSource> ();


	}



	//Public Methods

	public void AddAction( Vector3 dir, Vector3 pos, bool hold){

		time_c = Time.time - time_i;
		if (time_c > delay && !traveling) {
			travelList [index].AddAction (dir, pos, time_c, hold);
			for (int i = 0; i < index && traveling; i++) {
				if (!enableList [i]) {
					travelList [i].AddNullAction (transform.position,time_c);
				}
			}
		}
	}
		
	public void MoveFormers (){

		time_c = Time.time - time_i;

		// Level
		if (traveling) {
			levelTime.fillAmount = 0;
			levelWaitTime.fillAmount =  1;
			levelWaitTime.fillAmount = 1;
		} else {
			float aux = time_c / totalTime;
			levelTime.fillAmount = aux;
			if (levelTime.fillAmount > 0.75f) {
				sourse.pitch = 1f + Mathf.Clamp01 ((aux - 0.75f) / (0.25f)) * 0.6f;
				sourse.volume = 0.3f + Mathf.Clamp01 ((aux - 0.75f) / (0.25f)) * 0.5f;
			} else {
				sourse.pitch = 1;
			}
			if (levelTime.fillAmount > 0.82f) {
				levelTime.color = new Color (
					1,
					(1-(aux-0.82f)/(0.18f)),
					(1-(aux-0.82f)/(0.18f)),
					(0.8f+(aux/5))
				); 
			} else {

				levelTime.color = new Color (1, 1, 1, (0.8f+(aux/5)));
			}

			if (travelNum > 0) {
				levelWaitTime.fillAmount = time_c / waitTime;
				levelWaitTimeHUD.fillAmount = time_c / waitTime;
				textWaitTime.text = travelNum.ToString ();

				if (levelWaitTimeHUD.fillAmount < 0.98) {
					levelWaitTimeHUD.color = new Color ((40f / 255f), (145f / 255f), 0, (150f / 255f));
				} else {
					levelWaitTimeHUD.color = new Color (1, 1, 1);
					if ((time_c / waitTime) < 1)
						levelWaitTimeHUD.transform.GetComponent<Animator> ().SetTrigger ("Scale");
				}
			} else {
				levelWaitTime.fillAmount =  0;
				levelWaitTimeHUD.fillAmount = 0;
				textWaitTime.text = travelNum.ToString ();
			}
		}


		if (traveling) {
			for ( int i = 0; i <= index && traveling; i++) {
				if (travelList [i].AdjustTimeReverse (time_c)) {
					if (enableList [i]) {
						travelList [i].AdjustPosition (formerList [i]);
						Action aux = travelList [i].GetAction ();
						formerList [i].GetComponent<Movement> ().Move (aux.GetDirection (), aux.GetHold ());
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
							aux.GetHold ()
						);
					} else {
						formerList [i].GetComponent<Movement> ().Move (
							Vector3.zero,
							false
						);
						enableList [i] = false;
						Disable (formerList[i]);
					}
				} else {
					//travelList [i].AddNullAction (transform.position,time_c);
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

	public bool IsTraveling(){
		return traveling;
	}
	public void DecreaseTotalTime (float timeMinus){
		totalTime -= timeMinus;
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

	public void TimeTravel(){
		if (travelNum > 0 && time_c > delay && !traveling)
			BegingTimeTravel ();
	}
		
}