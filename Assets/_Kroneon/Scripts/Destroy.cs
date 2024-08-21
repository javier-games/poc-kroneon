using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy : MonoBehaviour {

	[SerializeField]
	private string tagTarget = "Player";
	[SerializeField]
	private GameObject[] objectsToDestroy;

	void OnTriggerEnter(Collider other){
		if (other.CompareTag (tagTarget)) {
			for (int i = 0; i < objectsToDestroy.Length; i++) {
				Destroy (objectsToDestroy [i]);
			}
			if (NavSurfaceBaker.instance) {
				NavSurfaceBaker.instance.Bake ();
			}
		}
	}
}
