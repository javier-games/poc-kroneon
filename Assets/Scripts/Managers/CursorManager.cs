using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour {


	[SerializeField]
	private Texture2D cursorIdle;
	[SerializeField]
	private Texture2D cursorDown;
	[SerializeField]
	private Vector2 hotSpot = Vector2.zero;
	[SerializeField]
	private CursorMode cursorMode = CursorMode.Auto;

	void Start(){
		Cursor.SetCursor (cursorIdle, hotSpot, cursorMode);
	}

	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			Cursor.SetCursor (cursorDown, hotSpot, cursorMode);
		} else {
			Cursor.SetCursor (cursorIdle, hotSpot, cursorMode);
		}
	}
}
