using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PrepareVideo : MonoBehaviour {

	[SerializeField]
	private VideoPlayer videoPlayer;

	void Start () {
		videoPlayer.Prepare ();
		StartCoroutine( PlayVideo ());
	}
		IEnumerator PlayVideo(){
		videoPlayer.Prepare();
		Debug.Log ("no play");
		while (!videoPlayer.isPrepared)
			yield return null;
		
		videoPlayer.Play();
		Debug.Log ("play");
		while (videoPlayer.isPlaying)
			yield return null;
		transform.gameObject.SetActive (false);
		Debug.Log ("stop");
	}
}
