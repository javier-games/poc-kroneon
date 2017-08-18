using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public static UIManager instance;

	[SerializeField]
	private GameObject hudPanel;
	[SerializeField]
	private GameObject pausePanel;
	[SerializeField]
	private GameObject gameOverPanel;
	[SerializeField]
	private GameObject startPanel;
	[SerializeField]
	private GameObject winPanel;
	[SerializeField]
	private string menuSceneName;
	[SerializeField]
	private string nextLevel;



	void Awake(){
		instance = this;
	}
	void Start () {
		GameManager.instance.ChangeStateEvent += ShowPanel;
		ReStart ();
	}



	//	Enable Methods
	void ClearUI(){
		hudPanel.SetActive (false);
		pausePanel.SetActive (false);
		gameOverPanel.SetActive (false);
		startPanel.SetActive (false);
		winPanel.SetActive (false);
	}
	public  void StartGame(){
		GameManager.instance.ChangeToNewState (GameState.PLAYING);
	}
	public void Pause(){
		GameManager.instance.ChangeToNewState (GameState.PAUSE);
	}
	public void Continue(){
		GameManager.instance.ChangeToNewState (GameState.PLAYING);
	}
	public void GameOver(){
		GameManager.instance.ChangeToNewState (GameState.GAME_OVER);
	}
	public void Exit(){
		GameManager.instance.ChangeToNewState (GameState.EXIT);
	}
	public void ReStart(){
		GameManager.instance.ChangeToNewState (GameState.START);
	}



	public void ShowPanel(){
		switch (GameManager.instance.currentState) {

		case GameState.START:
			ClearUI ();
			startPanel.SetActive (true);
			Time.timeScale = 0;
			StartCoroutine (LoadScene(300f,menuSceneName));
			break;

		case GameState.CUTSCENE:
			ClearUI ();
			break;

		case GameState.PLAYING:
			StopAllCoroutines ();
			ClearUI ();
			hudPanel.SetActive (true);
			Time.timeScale = 1;
			break;

		case GameState.PAUSE:
			ClearUI ();
			pausePanel.SetActive (true);
			Time.timeScale = 0;
			break;

		case GameState.CONTINUE:
			break;

		case GameState.GAME_OVER:
			ClearUI ();
			gameOverPanel.SetActive (true);
			StartCoroutine (LoadScene(4f,SceneManager.GetActiveScene().name));
			break;

		case GameState.EXIT:
			Time.timeScale = 1;
			//This should be an other scene.
			StartCoroutine (LoadScene(0f,SceneManager.GetActiveScene().name));
			break;

		case GameState.RESTART:
			StartCoroutine (LoadScene(0f,SceneManager.GetActiveScene().name));
			break;

		case GameState.WIN:
			ClearUI ();
			winPanel.SetActive (true);
			StartCoroutine (LoadScene(4f,nextLevel));
			break;
		}
	}
	public IEnumerator LoadScene(float timeToWait,string sceneName){
		yield return StartCoroutine(CoroutineUtil.WaitForRealSeconds(timeToWait));
		SceneManager.LoadScene (sceneName,LoadSceneMode.Single);
	}
}