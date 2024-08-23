using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]

public class Movement : MonoBehaviour{

	[SerializeField] float spd_limit_low = 180;
	[SerializeField] float spd_limit_top = 360;

	Rigidbody m_Rigidbody;
	Animator m_Animator;

	const float k_Half = 0.5f;
	float m_TurnAmount;
	float m_ForwardAmount;

	[SerializeField] AudioClip[] normalSteps;
	private AudioSource audioControl;

	void Start(){
		m_Animator = GetComponent<Animator>();
		m_Rigidbody = GetComponent<Rigidbody>();

		m_Rigidbody.constraints = 
			RigidbodyConstraints.FreezeRotationX | 
			RigidbodyConstraints.FreezeRotationY | 
			RigidbodyConstraints.FreezeRotationZ;

		audioControl = GetComponent<AudioSource> ();

		audioControl.clip = normalSteps [0];
	}

	public void Move(Vector3 move, bool crouch){
		
		if (move.magnitude > 1f || move.magnitude < 1f) move.Normalize();

		move = transform.InverseTransformDirection(move);
		m_TurnAmount = Mathf.Atan2(move.x, move.z);
		m_ForwardAmount = move.z;
		float turnSpeed = Mathf.Lerp(spd_limit_low, spd_limit_top, m_ForwardAmount);
		transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);

		UpdateAnimator(move);
	}

	void UpdateAnimator(Vector3 move){
		m_Animator.SetFloat("Speed", m_ForwardAmount, 0.1f, Time.deltaTime);
		m_Animator.SetFloat ("Direction", m_TurnAmount, 0.1f, Time.deltaTime);
	}

	public void SoundStep(){
			audioControl.PlayOneShot (normalSteps[Random.Range(0,normalSteps.Length)]);
	}
}
