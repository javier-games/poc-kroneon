using UnityEngine;
using System.Collections;

public class GameTimer : MonoBehaviour {

	//this is how close to being done before it does the bounce animation (.8 means 80% done)
	public float bouncePerc = 0.8f;
	
	//what would you like to do when the time runs out
	public GameObject sendMessageObject;
	public string sendMessage;

	//rotation (must be the same as the Analog Timer CS Object 

	public int x_rotation =89;
	public int y_rotation=0;


	
	//start immediately when the scene runs
	public bool startOnPlay;
	public float secondsRemaining = 16;
	
	//define elements of the clocks\
	public GameObject hand;
	public GameObject activeR;
	public GameObject inacHalf;
	public Renderer activeRightRenderer;
	public Renderer inactiveHalfRenderer;
	
	//private vars
	private float maxTime;
	private float timeLeft;
	private GameObject thisGO;
	private Transform handT;
	private bool timerIsRunning;
	
	//event listeners
	public delegate void TimeIsUpAction();
	public static event TimeIsUpAction TimeIsUp;
	
	void Start () {
	
		thisGO = gameObject;
		handT = hand.transform;
		
		if(startOnPlay == true){
			startTimer(secondsRemaining);
		}
	
	}
	
	public void startTimer(float timerSeconds)
	{
		
		resumeTimer();
		
		maxTime = timerSeconds;
		timeLeft = timerSeconds;
		
		stopTimer();//make sure it's not already playing
		timerIsRunning = true;
		InvokeRepeating("moveTime", 1f, 1f);//start timer
		
	}
	
	//this function runs every second
	private void moveTime()
	{
		
		timeLeft = timeLeft - 1;
		
		determineAngle();
		
	}
	
	private void determineDone()
	{
		
		//check if 0 seconds left
		if(timeLeft == 0){
			
			stopTimer(true);
			
			timerIsRunning = false;
			
			if(sendMessageObject){
				
				sendMessageObject.SendMessage(sendMessage);
				
			}
			
			if(TimeIsUp != null)
				TimeIsUp();
			
		}
		
	}
	
	//add seconds
	public void addSeconds(int timeToAdd)
	{
		
		if(timerIsRunning){
			timeLeft = timeLeft + timeToAdd;
			if(timeLeft > maxTime){
				timeLeft = maxTime;
			}
			determineAngle();
		}
		
	}
	
	//determine the angle of the hand
	private void determineAngle()
	{
		
		float percDone = 1 - timeLeft/maxTime;
		float angle = -percDone * 360;
		if(percDone <= 0)angle = 359;
		
		moveHands(percDone, angle);
		
	}
	
	//move the hands
	private void moveHands(float percDone, float angle)
	{
		
		//iTween.RotateTo(hand,{"rotation":Vector3(0,0,angle),"time":.5,"delay":0, "onComplete":"determineDone", "onCompleteTarget":thisGO, "onUpdate":"moveBgs", "onUpdateTarget":thisGO});
		//iTween.MoveTo(gameObject,iTween.Hash("x",3,"time",4,"delay",1,"onupdate","myUpdateFunction","looptype",iTween.LoopType.pingPong));				
		iTween.RotateTo(hand, iTween.Hash("rotation",new Vector3(x_rotation,y_rotation,angle),"time",.5f,"delay",0, "onComplete","determineDone", "onCompleteTarget",thisGO, "onUpdate","moveBgs", "onUpdateTarget",thisGO));
		
		//iTween.RotateTo(hand, iTween.Hash("rotation",new Vector3(0,0,angle),"time",.5f,"delay",0));
		
		if(percDone >= bouncePerc){
			//iTween.PunchScale(thisGO, {"amount": Vector3(.1,.1,0), "time":.7});
			iTween.PunchScale(thisGO, iTween.Hash ("amount", new Vector3(.1f,.1f,0), "time",.7f));
		}
		
	}
	
	//move the backgrounds so that it looks like it's dark where it's been
	private void moveBgs()
	{
		
		// print the rotation around the y-axis
		float angle = handT.eulerAngles.z;
		
		if(angle >= 180){//first half
			
			activeRightRenderer.sortingOrder = 5;
			inactiveHalfRenderer.sortingOrder = -5;
			activeR.transform.rotation = Quaternion.Euler(x_rotation, y_rotation, angle);
			
		}else{//second half
			
			activeRightRenderer.sortingOrder = -5;
			inactiveHalfRenderer.sortingOrder = 6;
			inacHalf.transform.rotation = Quaternion.Euler(x_rotation, y_rotation, angle + 180);
			
		}
		
	}
	
	
	
	public void pauseTimer()
	{
		
		if(timerIsRunning)
			Time.timeScale = 0;
		
	}
	
	public void resumeTimer()
	{
		
		Time.timeScale = 1;
		
	}
	
	//stop the time and reset
	public void stopTimer()
	{
		
		if(timerIsRunning){
			iTween.Stop(thisGO, true);
			resetGraphics();
			CancelInvoke("moveTime");
		}
		
		resetGraphics();
		
	}
	
	//overload method for stopTimer
	//only plays when it's complete
	public void stopTimer(bool isFinished)
	{
		
		if(timerIsRunning){
			iTween.Stop(thisGO, true);
			if(!isFinished)resetGraphics();
			CancelInvoke("moveTime");
		}
		
	}
	
	//set everything back to how it started
	private void resetGraphics()
	{
		
		activeRightRenderer.sortingOrder = 5;
		inactiveHalfRenderer.sortingOrder = -5;
		inacHalf.transform.eulerAngles = new Vector3(x_rotation,y_rotation,0);
		activeR.transform.eulerAngles = new Vector3(x_rotation,y_rotation,0);
		hand.transform.eulerAngles = new Vector3(x_rotation,y_rotation,359);
		
	}
	


}
