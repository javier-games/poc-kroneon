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
	[SerializeField]
	private float videoWaitingTime;
	[SerializeField][Range(0f,0.5f)]
	private float transitionPercent;
	[SerializeField]
	private float transitionStepTime = 0.1f;

	private float speedTransition;
	private Color tempColor;


	void Awake(){
		instance = this;
		ChangeStateEvent += ShowPanel;
	}

	void Start(){

		tempColor = transition.color;

		ClearUI ();

		ChangeToNewState (GUIState.SPLASH);

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

	IEnumerator StartTransition(float duration, float sense, bool setSense){
		
		if (sense > 0f) 
			yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds( duration*(1f-transitionPercent) ));
		
		transition.gameObject.SetActive (true);
		tempColor = Color.white;

		if (sense > 0f) 
			tempColor.a = 0f;
		
		transition.color = tempColor;
		StartCoroutine (Transition (2f*sense*transitionStepTime/(duration*transitionPercent)) );

		if (sense < 0f && setSense)
			StartCoroutine ( StartTransition(duration,1f,false) );

	}
	IEnumerator Transition(float increment){
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(transitionStepTime));
		tempColor.a += increment;
		transition.color = tempColor;
		if ( (increment > 0f && tempColor.a < 1f) || (increment < 0f && tempColor.a > 0f)){
			StartCoroutine (Transition (increment));
		}
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
			
			ClearUI ();
			splashPanel.SetActive (true);
			StartCoroutine (ScheduleState (splashDuration, GUIState.SPONSORS));
			StartCoroutine ( StartTransition(splashDuration,-1f,true) );

			break;

		case GUIState.SPONSORS:
			
			ClearUI ();
			sponsorsPanel.SetActive (true);
			StartCoroutine ( ScheduleState (splashDuration,GUIState.TITLE) );
			StartCoroutine ( StartTransition(splashDuration,-1f,true) );

			break;

		case GUIState.TITLE:
			
			ClearUI ();
			titlePanel.SetActive (true);
			StartCoroutine ( ScheduleState (splashDuration,GUIState.MENU) );
			StartCoroutine ( StartTransition(1.2f*splashDuration,-1f,true) );

			break;

		case GUIState.MENU:
			
			ClearUI ();
			menuPanel.SetActive (true);
			StartCoroutine (ScheduleState (videoWaitingTime, GUIState.VIDEO));
			StartCoroutine (StartTransition (splashDuration, -1f, false));

			break;

		case GUIState.CONTROLS:
			
			ClearUI ();
			controlsPanel.SetActive (true);

			break;

		case GUIState.CREDITS:
			
			ClearUI ();
			creditsPanel.SetActive (true);

			break;

		case GUIState.VIDEO:
			
			ClearUI ();
			videoPanel.SetActive (true);
			StopAllCoroutines ();

			break;

		case GUIState.LOADING:
			
			ClearUI ();
			StartCoroutine (LoadScene(3f,firstLavelName));

			break;
		}
	}

	void Update(){
		if (Input.GetKeyDown (KeyCode.R) && (currentState == GUIState.VIDEO || currentState == GUIState.CREDITS || currentState == GUIState.CONTROLS)) {
			Menu ();
		}
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