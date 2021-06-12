using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCameraEntity : MonoBehaviour {

	const float VELOCITY = 10f;

	Vector3 m_MousePosition;

	float m_HorizontalAngle = 0;
	float m_VerticalAngle = 0;

	// Update is called once per frame
	void Update () {

		if (Input.GetMouseButtonDown (0)) {
		
			m_MousePosition = Input.mousePosition;

		} else if (Input.GetMouseButton (0)) {
			
			Vector3 mouseDeltaPosition = m_MousePosition - Input.mousePosition;

			m_HorizontalAngle -= mouseDeltaPosition.x * 0.1f;
			m_VerticalAngle = Mathf.Clamp (m_VerticalAngle + mouseDeltaPosition.y * 0.1f, -89f, 89f);

			this.transform.localEulerAngles = new Vector3 (m_VerticalAngle, m_HorizontalAngle, 0f);

			m_MousePosition = Input.mousePosition;
		}
			
		Vector3 movement = Vector3.zero;

		if (Input.GetKey (KeyCode.W)) {
		
			// Move forward
			movement += Vector3.forward;

		} else if (Input.GetKey (KeyCode.S)) {

			// Move backward
			movement += Vector3.back;
		} 

		if (Input.GetKey (KeyCode.A)) {

			// Parallel left
			movement += Vector3.left;

		} else if (Input.GetKey (KeyCode.D)) {

			// Parallel right
			movement += Vector3.right;
		}

		if (Input.GetKey (KeyCode.E)) {

			// Vertical up
			movement += Vector3.up;

		} else if (Input.GetKey (KeyCode.Q)) {

			// Vertical down
			movement += Vector3.down;
		} 

		if (movement != Vector3.zero) {
		
			movement.Normalize ();
			this.transform.Translate (movement * VELOCITY * Time.deltaTime);
		}
	}
}
