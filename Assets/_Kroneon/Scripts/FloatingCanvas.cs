using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCanvas : MonoBehaviour {

	[SerializeField] private Vector3 offset;

	void Update () {
		transform.LookAt (Camera.main.transform.forward + transform.parent.transform.position + offset);
		transform.position = transform.parent.transform.position + offset;
	}
}
