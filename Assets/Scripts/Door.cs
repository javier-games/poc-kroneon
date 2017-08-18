using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour {

	[SerializeField]
	private Vector3 targetPosition;
	[SerializeField]
	private float speed = 0.2f;
	[SerializeField]
	private float tolerance = 0.05f;
	[SerializeField]
	private bool lockAtFull = false;
	[SerializeField]
	private Switch[] related;
	[SerializeField]
	private AudioClip bakeClip;

	private AudioSource source;
	private Vector3 originalPosition;
	private Vector3 destinyPosition;
	private Vector3 velocity;
	private float amount = 0;
	private float lastAmount = 0;
	private bool bake = false;
	private bool locked = false;

	void Start () {
		source = GetComponent<AudioSource> ();
		originalPosition = transform.position;
		targetPosition = transform.TransformPoint (targetPosition);
		destinyPosition = Vector3.Lerp (originalPosition,targetPosition,amount);
	}

	void Update () {

		if (!locked) {
			amount = 0;
			for (int i = 0; i < related.Length; i++) {
				if (related [i].IsPressed ()) {
					amount += 1f / related.Length;
				}
			}

			if (amount != lastAmount)
				UpdateDestiny ();

			lastAmount = amount;

			if ((transform.position - destinyPosition).magnitude > tolerance)
				transform.position = Vector3.SmoothDamp (transform.position, destinyPosition, ref velocity, speed);
			else if (bake && NavSurfaceBaker.instance) {
				source.PlayOneShot (bakeClip);
				NavSurfaceBaker.instance.Bake ();
				bake = false;
				if (lockAtFull && amount >= 1)
					locked = true;
				
			}
		}


	}

	public void UpdateDestiny(){
		destinyPosition = Vector3.Lerp (originalPosition,targetPosition,amount);
		bake = true;
	}
}