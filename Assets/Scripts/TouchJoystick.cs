using UnityEngine;
using System.Collections;

public class TouchJoystick : MonoBehaviour {

	public Texture2D bg;
	public Texture2D vectorIndicator;
	public float precision = 5;
	
	bool invoked = false;
	float radius;
	
	Vector2 basePos = Vector2.zero;
	Vector2 currentPos = Vector2.zero;
	Vector2 offset = Vector2.zero;
	
	float precisionMultiplier;
	Vector2 direction = Vector2.zero;
	
	
	
	void Start() {
		radius = bg.width / 2;
		precisionMultiplier = precision / radius;
	}
	
	

	void Update() {
		Vector2 pos;
		if (Input.GetMouseButton(0) || Input.touchCount > 0) {
			if (Input.touchCount > 0) {
				pos = Input.GetTouch(0).position;
			} else {
				pos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
			}
			if (!invoked) {
				InitJoystick(pos);
			}
			UpdateJoystick(pos);
		} else {
			DisableJoystick();
		}
	}
	
	
	
	void InitJoystick(Vector2 pos) {
		invoked = true;
		basePos = new Vector2(
			Mathf.Min (Mathf.Max(radius, pos.x), Screen.width - radius),
			Mathf.Min (Mathf.Max(radius, pos.y), Screen.height - radius)
		);
	}
	
	Vector2 RestrictJoystickMagnitude(Vector2 pos) {
		offset = pos - basePos;
		Vector2 restricted = basePos + Vector2.ClampMagnitude(offset, radius);
		return restricted;
	}
	
	Vector2 CalibrateDirection(Vector2 direction) {
		return new Vector2(
			Mathf.FloorToInt((direction.x + .5f) * precisionMultiplier),
			Mathf.FloorToInt((direction.y + .5f) * precisionMultiplier)
		);
	}
	
	void UpdateJoystick(Vector2 pos) {
		currentPos = RestrictJoystickMagnitude(pos);
		direction = CalibrateDirection(basePos - currentPos);
	}
	
	void DisableJoystick() {
		invoked = false;
		direction = Vector2.zero;
	}
	
	
	
	void DebugDisplayJoystickData() {
		Rect labelPos = new Rect(0, 50, Screen.width, Screen.height);
		string content;
		content = "\nDirection: " + direction.x.ToString() + " × " + direction.y.ToString();
		if (!invoked) {
			content += "\njoystick dismissed";
		}
		GUI.Label(labelPos, content);
	}
	
	void DebugDisplayScreenSize() {
		Rect pos = new Rect(0,0,300, 50);
		string content = "Screen size: " + Screen.width + " × " + Screen.height;
		GUI.Label(pos, content);
	}
	
	
		
	void OnGUI() {
		DebugDisplayScreenSize();
		DebugDisplayJoystickData();
		
		if (invoked) {
			Rect bgPos = new Rect(basePos.x - bg.width/2, Screen.height - basePos.y - bg.height/2, bg.width, bg.height);
			GUI.DrawTexture(bgPos, bg);
			
			Rect vectorPos = new Rect(currentPos.x - vectorIndicator.width/2, Screen.height - currentPos.y - vectorIndicator.height/2, vectorIndicator.width, vectorIndicator.height);
			GUI.DrawTexture(vectorPos, vectorIndicator);
		}
	}
}