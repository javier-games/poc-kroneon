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


		//	Audio
		[SerializeField] AudioClip[] normalSteps;
		[SerializeField] AudioClip jumpSound; 
		[SerializeField] AudioClip crouching;
		private AudioSource audioControl;


		void Start()
		{
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

			//	Normalize if it was not normalized
			if (move.magnitude > 1f || move.magnitude < 1f) move.Normalize();

			//	Rotation Control
			//		Transforms the direction from world-sapce to local-space (Li)
			move = transform.InverseTransformDirection(move);
			//		Getting the error with the axis-x as setpoint
			m_TurnAmount = Mathf.Atan2(move.x, move.z);
			//		Gettin the error in a range of -1 to 1
			m_ForwardAmount = move.z;
			//		Calculating the speed according to the error
			float turnSpeed = Mathf.Lerp(spd_limit_low, spd_limit_top, m_ForwardAmount);
			//		Applying the speed
			transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);

			// send input and other state parameters to the animator
			UpdateAnimator(move);
		}




		void UpdateAnimator(Vector3 move)
		{
			// update the animator parameters
			m_Animator.SetFloat("Speed", m_ForwardAmount, 0.1f, Time.deltaTime);
			m_Animator.SetFloat ("Direction", m_TurnAmount, 0.1f, Time.deltaTime);

		}
			

		public void SoundStep(){
			if (m_ForwardAmount > 0.1) {
				if (audioControl.isPlaying) {
					audioControl.Stop ();
					audioControl.clip = normalSteps[Random.Range(0,normalSteps.Length)];
				}
				audioControl.Play ();
			}
		}

	}
