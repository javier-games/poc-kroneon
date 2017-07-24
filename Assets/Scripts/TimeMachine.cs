using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum MachineState{Charging, Ready, Traveling}
[RequireComponent(typeof(BoxCollider))]
public class TimeMachine : MonoBehaviour {


	public static TimeMachine instance;

	public delegate void ChangeState();
	public ChangeState ChangeStateEvent;


	[SerializeField]
	private Transform traveller;
	[SerializeField]
	private GameObject formerPrefab;


	private MachineState 	state;
	private List<Transform> formerList;
	private List<Travel> 	travelList;
	private int 			index = 0;
	private Animator 		animator;

	[SerializeField]
	private float minDistance	= 1f;
	[SerializeField]
	private float travelTime	= 5f;
	[SerializeField]
	private float chargingTime	= 5f;
	[SerializeField]
	private float currentTime 	= 0f;
	private float initialTime 	= 0f;



	void Awake(){
		instance = this;
		state = MachineState.Charging;
		ChangeStateEvent += HasChangeState;
	}
	void Start(){
		travelList = new List<Travel> ();
		travelList.Add (new Travel());
		formerList = new List<Transform> ();
		animator = GetComponent<Animator> ();
	}

	public void ChangeStateTo(MachineState state){
		this.state = state;
		ChangeStateEvent ();
	}

	private void HasChangeState(){
		//Debug.Log ("New timemiche state: " + state);
	}

	public void UpdateTime(){
		currentTime = Time.time - initialTime;

		if (Vector3.Distance (transform.position, traveller.position) > minDistance &&
			state == MachineState.Charging && currentTime > chargingTime) {
			ChangeStateTo (MachineState.Ready);
			travelList [index].SetStartTime (currentTime);
			travelList [index].AddAction (traveller.position,0f,true);
			travelList [index].AddAction (traveller.GetComponent<PickingController>().GetDestination(),0.01f,true);
			animator.SetTrigger ("Close");
		}
	}

	public void AddActionAt(Vector3 point){
		if (currentTime > chargingTime && state == MachineState.Ready) {
			travelList [index].AddAction (traveller.position, currentTime);
		}
	}

	public void MoveFormers(){

		if (state == MachineState.Traveling) {
		} else {
			for (int i = 0; i < index; i++) {
				if (travelList [i].TimeToSetDestination (currentTime)) {
					formerList [i].GetComponent<FormerController> ().SetDestination (travelList [i].GetAction ().GetPosition ());
				} else {
					Debug.Log ("Its Time has finished");
				}
			}
		}
	}

	public void TimeTravel(){
		if (state == MachineState.Ready) {

			travelList [index].AddAction (traveller.position,currentTime);
			ChangeStateTo (MachineState.Traveling);
			travelList.Add (new Travel ());
			initialTime = Time.time;

			traveller.position = transform.position;
			traveller.gameObject.SetActive (false);

			GameObject former = (GameObject)Instantiate (
				                   formerPrefab, 
				                   travelList [index].GetAction (0).GetPosition (), 
				                   traveller.rotation
			                   );
			formerList.Add (former.transform);

			for (int i = 0; i <= index; i++) {
				travelList [i].SetIndex (0);
				formerList [i].position = travelList [i].GetAction ().GetPosition ();
			}
			index++;

			StartCoroutine ("StopTimeTravel", travelTime);
		}
	}

	IEnumerator StopTimeTravel(float timeToWait){
		yield return new WaitForSeconds (timeToWait);

		state = MachineState.Charging;
		initialTime = Time.time;
		traveller.gameObject.SetActive (true);
		animator.SetTrigger ("Open");
	}

	private void Disable(Transform model){
		model.GetComponentInChildren<SkinnedMeshRenderer> ().enabled =false;
		model.GetComponent<CapsuleCollider>().enabled = false;
		model.GetComponent<Rigidbody>().isKinematic = true;
		if(model.name == traveller.name)
			model.GetComponent<PickingController> ().SetMovemenActive(false);
	}
	private void Enable(Transform model){
		model.GetComponentInChildren<SkinnedMeshRenderer> ().enabled =true;
		model.GetComponent<Rigidbody>().isKinematic = false;
		model.GetComponent<CapsuleCollider>().enabled = true;
		if(model.name == traveller.name)
			model.GetComponent<PickingController> ().SetMovemenActive(true);
	}













	/*
	 *
	public static TimeMachine instance;

	[SerializeField] private GameObject	traveller;			//	The current character with movemoent
	[SerializeField] private GameObject	formerPrefab;		//	Prefab of the traveller

	private List<Travel> travelList;						//	List of the travels
	private List<GameObject> formerList;					//	List of formers
	private int	index = 0;

	[SerializeField]
	private float delayTime   = 5f;
	private float initialTime = 0f;							//	Initial time of the play process
	private float currentTime = 0f;							//	Current time
	private bool  traveling = false;

	//	Trigger Variables
	private bool travellerInside = true;


	void Awake(){
		instance = this;
	}
	void Start(){
		travelList = new List<Travel> ();
		travelList.Add (new Travel());
		formerList = new List<GameObject>();
		initialTime = Time.time;
	}


	public void AddActionAt(Vector3 point){
		currentTime = Time.time - initialTime;
		if (currentTime > delayTime && !traveling) {
			travelList [index].AddAction (point,currentTime);
		}
	}

	public void MoveFormers(){
		currentTime = Time.time - initialTime;

		if (traveling) {
			for ( int i = 0; i <= index && traveling; i++) {
				if (travelList [i].AdjustTimeReverse (currentTime)) {
					formerList [i].GetComponent<FormerController> ().SetDestination (travelList[i].GetAction().GetPosition());
				} else if (index == i) {
					EndTimeTravel ();
				}

			}
		} else {
			for (int i = 0; i < index; i++) {
				if (enableList [i]) {
					if (travelList [i].AdjustTime (time_c)) {

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


	private void BegingTimeTravel(){
		travelList [index].EndTravel (traveller.transform.position,currentTime);
		travelList.Add (new Travel ());
		initialTime = Time.time;
		currentTime = Time.time - initialTime;

		Disable (traveller);

		GameObject former = (GameObject)Instantiate (
			formerPrefab, 
			travelList [index].GetAction ().GetPosition(), 
			Quaternion.FromToRotation(Vector3.forward,transform.forward)
		);
		formerList.Add (former);

		traveling = true;
		for (int i = 0; i <= index; i++) {
			formerList[i].GetComponent<Animator> ().SetFloat ("Speed",-1);
		}

		//Time.timeScale = 2.5f;
	}

	private void EndTimeTravel(){
		
	}


	void OnTriggerEnter(Collider other){
		if ( !travellerInside && other.tag == "Player" && currentTime > delayTime) {
			travellerInside = true;
			if (LevelManager.instance.GetTimeTravelCount () > 0) {
				BegingTimeTravel ();
			}
		}
		if (other.tag == "Respawn")
			for (int i = 0; i < index; i++)
				if (travelList [i].Ended ()) {
					//Disable (formerList [i]);
				}
	}
	void OnTriggerExit(Collider other){
		//	If the traveller has exit the flag change.
		if( travellerInside && other.gameObject == traveller )
			travellerInside = false;
	}

	private void Disable(GameObject model){
		model.GetComponentInChildren<SkinnedMeshRenderer> ().enabled =false;
		model.GetComponent<CapsuleCollider>().enabled = false;
		model.GetComponent<Rigidbody>().isKinematic = true;
		if(model.name == traveller.name)
				model.GetComponent<PickingController> ().SetMovemenActive(false);
	}
	private void Enable(GameObject model){
		model.GetComponentInChildren<SkinnedMeshRenderer> ().enabled =true;
		model.GetComponent<Rigidbody>().isKinematic = false;
		model.GetComponent<CapsuleCollider>().enabled = true;
		if(model.name == traveller.name)
			model.GetComponent<PickingController> ().SetMovemenActive(true);
	}*/


	////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

	/*


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
	[SerializeField] private ManagerScenes level;
	private bool stopCounting = false;
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
			travelList [index].AddAction (pos, time_c, hold);
			for (int i = 0; i < index && traveling; i++) {
				if (!enableList [i]) {
					
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
		if (levelTime.fillAmount >= 1 && !stopCounting) {
			level.Reload ();
		}


		if (traveling) {
			for ( int i = 0; i <= index && traveling; i++) {
				if (travelList [i].AdjustTimeReverse (time_c)) {
					

				} else if (index == i) {
					EndTimeTravel ();
				}

			}
		} else {
			for (int i = 0; i < index; i++) {
				if (enableList [i]) {
					if (travelList [i].AdjustTime (time_c)) {
						
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
			Quaternion.FromToRotation(Vector3.forward,transform.forward)
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
		//if(model.name == traveller.name)
		//	model.GetComponent<Controller> ().MovementActive = false;
	}
	private void Enable(GameObject model){
		model.GetComponentInChildren<SkinnedMeshRenderer> ().enabled =true;
		model.GetComponent<Rigidbody>().isKinematic = false;
		model.GetComponent<CapsuleCollider>().enabled = true;
		//if(model.name == traveller.name)
		//	model.GetComponent<Controller> ().MovementActive = true;
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

	public void StopCounting(){
		stopCounting = true;
	}
		*/
}