using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PrepareVideo : MonoBehaviour {

	[SerializeField]
	private VideoPlayer videoPlayer;
	[SerializeField]
	private AudioSource source;
	[SerializeField]
	private AudioClip clip;

	void Start () {
		StartCoroutine( PlayVideo ());
	}

	IEnumerator StopVideo(){
		yield return new WaitForSeconds((float)videoPlayer.clip.length);
		videoPlayer.transform.gameObject.SetActive (false);
	}

	IEnumerator PlayVideo(){
		videoPlayer.Prepare();
		while (!videoPlayer.isPrepared)
			yield return null;
		
		videoPlayer.Play();
		source.PlayOneShot (clip);

		while (videoPlayer.isPlaying)
			yield return null;
		
		transform.gameObject.SetActive (false);
	}
}
