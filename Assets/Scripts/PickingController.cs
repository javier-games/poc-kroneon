using UnityEngine.AI;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Movement))]
public class PickingController: MonoBehaviour {

	[SerializeField]
	private float minDistanceToTarget = 0.2f;
	[SerializeField]
	private GameObject destinyPrefab;
	[SerializeField]
	private GameObject balloon;
	[SerializeField]
	private float messageDuration;


	private NavMeshAgent agent;
	private RaycastHit mouseHit;
	private Quaternion rotation;
	private Vector3		position;
	private Movement movement;
	private Vector3 lastPosition;
	private bool movementActive = true;
	private ParticleSystem destiny;
	private bool canTravel = true;



	void Start(){
		
		agent = GetComponent<NavMeshAgent>();
		movement = GetComponent<Movement> ();

		destiny = Instantiate (destinyPrefab).GetComponent<ParticleSystem>();
		destiny.transform.position = transform.position;

		position = transform.position;
		rotation = transform.rotation;
		lastPosition = position;

	}

	void Update (){

		TimeMachine.instance.UpdateTime ();

		if(movementActive)
			ReadInputs ();

		UpdateLimit ();

		movement.Move (transform.position-lastPosition,false);
		lastPosition = transform.position;

		TimeMachine.instance.MoveFormers ();

	}

	private void ReadInputs(){
		if (GameManager.instance.currentState == GameState.PLAYING ) {
			if (Input.GetMouseButtonDown (0)) {
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out mouseHit)) {
					agent.SetDestination (mouseHit.point);
					TimeMachine.instance.AddActionAt (mouseHit.point);
					destiny.transform.position = mouseHit.point;
					destiny.Play ();
				}
			}
			if (canTravel) {
				if (Input.GetKeyDown (KeyCode.Space)) {
					TimeMachine.instance.TimeTravel ();
				}
			} else if (Input.GetKeyDown (KeyCode.Space)) {
				balloon.SetActive (true);
				balloon.transform.GetChild (0).GetComponent<Text> ().text = "No debo estar sobre el switch para viajar";
				StartCoroutine (CloseMessage(messageDuration));
			}
		}
	}

	private void UpdateLimit(){
		if (Vector3.Distance (transform.position, agent.destination) < minDistanceToTarget) {
			agent.speed = 0;
			agent.angularSpeed = 0;
			transform.rotation = rotation;
			transform.position = position;
		} else {
			agent.speed = 3.5f;
			agent.angularSpeed = 180;
			rotation = transform.rotation;
			position = transform.position;
		}
	}

	public void SetMovemenActive(bool stade){
		movementActive = stade;
	}

	public void SetDestination(Vector3 point){
		agent.SetDestination (point);
	}
	public Vector3 GetDestination(){
		return agent.destination;
	}
	public void SetCanTravel(bool newCanTravel){
		canTravel = newCanTravel;
	}
	IEnumerator CloseMessage(float timeToWait){
		yield return new WaitForSeconds (timeToWait);
		balloon.transform.GetChild (0).GetComponent<Text> ().text = "";
		balloon.SetActive (false);
	}

}
