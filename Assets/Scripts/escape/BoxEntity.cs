using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEntity : OpenableEntity {

	Entity m_Content;
	bool m_Closed = true;

	public BoxEntity (EscapeGame game, string name, Entity content, string keyIdentifier, Vector3 position) :
		base (game, name, keyIdentifier, position) {

		m_Content = content;
	}

	public override void Inspect () { 

		if (m_Closed) {

			Debug.Log ("A closed box.");

		} else {

			if (m_Content == null) {

				Debug.Log ("An empty box.");

			} else {

				Debug.Log ("Something inside the box:\n");
				m_Content.Inspect ();
			}
		}
	}

	public override void Interact (Entity entity = null) {

		if (m_Closed) {

			m_Closed = false;
			Debug.Log ("The box is opened.");

		} else {

			if (m_Content == null) {

				base.Interact (entity); 

			} else {

				Debug.Log ("Something inside the box, interact with it:\n");
				m_Content.Interact (entity);
			}
		}
	}
}