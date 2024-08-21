using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour {

	public static HUD instance;

	[SerializeField]
	private Image	timeLevel;
	[SerializeField]
	private Image	timeChargingLevel;
	[SerializeField]
	private Text	timeChargingText;

	void Awake(){
		instance = this;
	}

	void Start() {
		LevelManager.instance.ChangedTimeEvent += UpdateTimeLevel;
		LevelManager.instance.ChangedTravelCountEvent += UpdateTravelCount;
	}

	private void UpdateTimeLevel(){
		timeLevel.fillAmount = (TimeMachine.instance.GetCurrentTime() + LevelManager.instance.GetGameTime() - LevelManager.instance.GetGameTimeAmount() ) / LevelManager.instance.GetGameTime();
		if (LevelManager.instance.GetTravelCount () > 0f)
			timeChargingLevel.fillAmount = (TimeMachine.instance.GetCurrentTime () / TimeMachine.instance.GetChargingTime ());
		else
			timeChargingLevel.fillAmount = 0f;
	}
	private void UpdateTravelCount(){
		timeChargingText.text = LevelManager.instance.GetTravelCount ().ToString();
	}
}
