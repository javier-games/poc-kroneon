using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Movement))]
public class FormerController : MonoBehaviour {
	
	[SerializeField]
	private float minDistanceToTarget = 0.2f;
	[SerializeField]
	private Transform spotLight;

	private NavMeshAgent	agent;
	private Quaternion		rotation;
	private Vector3			position;
	private Movement		movement;
	private Vector3			lastPosition;
	private bool			movementActive = true;

	void Start(){
		agent = GetComponent<NavMeshAgent>();
		movement = GetComponent<Movement> ();

		position = transform.position;
		rotation = transform.rotation;
		lastPosition = position;
	}

	void Update (){
		UpdateLimit ();
		movement.Move (transform.position-lastPosition,false);
		lastPosition = transform.position;
	}

	public void SetDestination(Vector3 newPoint){
		if(movementActive)
			agent.SetDestination (newPoint);
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

	public void EnableSpotLight(){
		spotLight.gameObject.SetActive (true);
	}
	public void DisableSpotLight(){
		spotLight.gameObject.SetActive (false);
	}
}