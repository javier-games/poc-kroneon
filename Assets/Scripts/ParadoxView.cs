using UnityEngine;

public class ParadoxView : MonoBehaviour {
    void OnTriggerEnter(Collider other){
		if ((other.tag == "Player" || other.tag == "Respawn" ) && transform.parent.gameObject != other.gameObject){
			GameManager.instance.ChangeToNewState (GameState.GAME_OVER);
        }
    }
}
