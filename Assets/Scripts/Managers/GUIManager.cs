using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GUIState{SPLASH, SPONSORS, TITLE, MENU, CREDITS, CONTROLS, LOADING, VIDEO}
public class GUIManager : MonoBehaviour {



	public static GUIManager instance;
	public GUIState currentState;

	public delegate void ChangeState();
	public ChangeState ChangeStateEvent;

	[SerializeField]
	private Image transition;
	[SerializeField]
	private GameObject splashPanel;
	[SerializeField]
	private GameObject sponsorsPanel;
	[SerializeField]
	private GameObject titlePanel;
	[SerializeField]
	private GameObject menuPanel;
	[SerializeField]
	private GameObject creditsPanel;
	[SerializeField]
	private GameObject controlsPanel;
	[SerializeField]
	private GameObject videoPanel;
	[SerializeField]
	private string firstLavelName;
	[SerializeField]
	private float splashDuration;
	[SerializeField][Range(0f,1f)]
	private float transitionPercent;
	[SerializeField]
	private float videoWaitingTime;
	[SerializeField]
	private float speed = 0.01f;

	private float speedTransition;


	void Awake(){
		instance = this;
		ChangeStateEvent += ShowPanel;
	}

	void Start(){
		transition.gameObject.SetActive (true);
		transition.color = Color.white;
		ClearUI ();
		speedTransition = speed;
		ChangeToNewState (GUIState.SPLASH);
		transition.color = new Color (transition.color.r,transition.color.g,transition.color.b,1f);
	}

	public void ChangeToNewState(GUIState newState){
		currentState = newState;
		ChangeStateEvent ();
	}



	void ClearUI(){
		splashPanel.SetActive (false);
		sponsorsPanel.SetActive (false);
		titlePanel.SetActive (false);
		menuPanel.SetActive (false);
		creditsPanel.SetActive (false);
		controlsPanel.SetActive (false);
		videoPanel.SetActive (false);
	}
	public void Splash(){
		GUIManager.instance.ChangeToNewState (GUIState.SPLASH);
	}
	public void Title(){
		GUIManager.instance.ChangeToNewState (GUIState.TITLE);
	}
	public void Credits(){
		GUIManager.instance.ChangeToNewState (GUIState.CREDITS);
	}
	public void Controls(){
		GUIManager.instance.ChangeToNewState (GUIState.CONTROLS);
	}
	public void Menu(){
		GUIManager.instance.ChangeToNewState (GUIState.MENU);
	}
	public void StartGame(){
		GUIManager.instance.ChangeToNewState (GUIState.LOADING);
	}


	IEnumerator SetSense(float timeToWait){
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(timeToWait));
		speedTransition = speedTransition * -1f;
	}
	IEnumerator LoadScene(float timeToWait,string levelName){
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(timeToWait));
		SceneManager.LoadScene (levelName,LoadSceneMode.Single);
	}
	IEnumerator ScheduleState(float timeToWait, GUIState state){
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(timeToWait));
		ChangeToNewState (state);
	}



	public void ShowPanel(){
		switch (currentState) {

		case GUIState.SPLASH:
			speedTransition = speed;

			transition.color = new Color (transition.color.r,transition.color.g,transition.color.b,1f);
			ClearUI ();
			speedTransition = speedTransition * -1f;
			splashPanel.SetActive (true);
			StartCoroutine (ScheduleState (splashDuration, GUIState.SPONSORS));
			StartCoroutine (SetSense (splashDuration*(1f-transitionPercent)));
			break;

		case GUIState.SPONSORS:
			speedTransition = speedTransition * -1f;
			ClearUI ();
			sponsorsPanel.SetActive (true);
			StartCoroutine ( ScheduleState(splashDuration,GUIState.TITLE) );
			StartCoroutine (SetSense (splashDuration*(1f-transitionPercent)));
			break;

		case GUIState.TITLE:
			speedTransition = speedTransition * -1f;
			ClearUI ();
			titlePanel.SetActive (true);
			StartCoroutine ( ScheduleState(splashDuration*1.2f,GUIState.MENU) );
			StartCoroutine (SetSense (splashDuration*1.2f*(1f-transitionPercent)));
			break;

		case GUIState.MENU:
			ClearUI ();
			speedTransition = speedTransition * -1f;
			menuPanel.SetActive (true);
			StartCoroutine (ScheduleState(videoWaitingTime,GUIState.VIDEO));
			break;

		case GUIState.CONTROLS:
			ClearUI ();
			controlsPanel.SetActive (true);
			//StopAllCoroutines ();
			break;

		case GUIState.CREDITS:
			ClearUI ();
			creditsPanel.SetActive (true);
			//StopAllCoroutines ();
			break;

		case GUIState.VIDEO:
			ClearUI ();
			videoPanel.SetActive (true);
			StopAllCoroutines ();
			break;

		case GUIState.LOADING:
			ClearUI ();
			transition.color = new Color (transition.color.r,transition.color.g,transition.color.b,0f);
			speedTransition = speed;
			StartCoroutine (LoadScene(3f,firstLavelName));
			break;

		}
	}


	void Update(){
		if (Input.GetKeyDown (KeyCode.R) && (currentState == GUIState.VIDEO)) {
			//speedTransition = -speed;
			//transition.color = new Color (transition.color.r,transition.color.g,transition.color.b,1f);
			Splash ();
		}
		transition.color = new Color (transition.color.r,transition.color.g,transition.color.b,transition.color.a+speedTransition);
	}

}

public static class CoroutineUtil{
	public static IEnumerator WaitForRealSeconds(float time){
		float start = Time.realtimeSinceStartup;
		while (Time.realtimeSinceStartup < start + time){
			yield return null;
		}
	}
}