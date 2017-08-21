using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class NavSurfaceBaker : MonoBehaviour{
    
	public  static NavSurfaceBaker instance;

	private NavMeshSurface[] navMesh;

	void Awake(){
		instance = this;
	}

	void Start (){
        navMesh = GetComponents<NavMeshSurface>();
	}
	
	public void Bake(){
		for (int i = 0; i < navMesh.Length; i++){
			navMesh [i].Bake ();
		}
	}
}
