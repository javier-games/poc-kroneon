using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class NavSurfaceBuilder : MonoBehaviour{
    private NavMeshSurface[] navMesh;
	
	void Start (){
        navMesh = GetComponents<NavMeshSurface>();
		for (int i = 0; i < navMesh.Length; i++)
			navMesh [i].Bake ();
	}
	
	public void Bake(){
		for (int i = 0; i < navMesh.Length; i++){
			navMesh [i].Bake ();
		}
	}
}
