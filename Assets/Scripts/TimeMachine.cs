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
	private Transform		traveller;
	[SerializeField]
	private GameObject		formerPrefab;


	private MachineState 	state;
	private List<Transform> formerList;
	private List<Travel> 	travelList;
	private int 			index = 0;
	private Animator 		animator;


	[SerializeField]
	private float minDistance	= 1f;
	[SerializeField]
	private float travelTime	= 0f;
	[SerializeField]
	private float chargingTime	= 5f;
	private float currentTime 	= 0f;
	private float initialTime 	= 0f;



	//	Initialization

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



	//	Action and Movement Methods

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
					Disable (formerList[i]);
				}
			}
		}
	}
	private void Disable(Transform model){
		model.GetComponentInChildren<SkinnedMeshRenderer> ().enabled =false;
		model.GetComponent<CapsuleCollider>().enabled = false;
		model.GetComponent<Rigidbody>().isKinematic = true;
		if (model.name == traveller.name)
			model.GetComponent<PickingController> ().SetMovemenActive (false);
		else
			model.GetComponent<FormerController> ().DisableSpotLight ();
	}
	private void Enable(Transform model){
		model.GetComponentInChildren<SkinnedMeshRenderer> ().enabled =true;
		model.GetComponent<Rigidbody>().isKinematic = false;
		model.GetComponent<CapsuleCollider>().enabled = true;
		if(model.name == traveller.name)
			model.GetComponent<PickingController> ().SetMovemenActive(true);
		else
			model.GetComponent<FormerController> ().EnableSpotLight ();
	}




	//	Time Methods

	public void ChangeStateTo(MachineState state){
		this.state = state;
		ChangeStateEvent ();
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
	public void TimeTravel(){
		if (state == MachineState.Ready && LevelManager.instance.GetTravelCount()>0) {

			LevelManager.instance.DecreaseTravelCount ();

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
		for (int i = 0; i < index; i++) {
			Enable (formerList[i]);
		}
		animator.SetTrigger ("Open");

		LevelManager.instance.DecreaseGameTime (travelList[index-1].GetStartTime());
	}



	//	Advertise Methods

	private void HasChangeState(){
		//Debug.Log ("New timemiche state: " + state);
	}



	//	Get Methods

	public float GetChargingTime(){
		return chargingTime;
	}
	public float GetCurrentTime(){
		return currentTime;
	}
}