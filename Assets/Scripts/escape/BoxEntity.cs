using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxEntity : OpenableEntity {

	Entity m_Content;
	bool m_Closed = true;

	public BoxEntity (EscapeGame game, string name, Entity content, string keyIdentifier, Vector3 position) :
		base (game, name, keyIdentifier, position) {
		m_Prefabs = "Silver_Chest";
		m_Content = content;
	}

	public override string Inspect () { 

		if (m_Closed) {

			return "A closed box.";

		} else {

			if (m_Content == null) {

				return "An empty box.";

			} else {

				m_Content.Inspect ();
				return "Something inside the box:\n";
			}
		}
	}

	public override string Interact (Entity entity = null) {

		if (m_Closed) {

			m_Closed = false;
			return "The box is opened.";

		} else {

			if (m_Content == null) {

				base.Interact (entity);
				return "";

			} else {

				m_Content.Interact(entity);
				return "Something inside the box, interact with it:\n";
			}
		}
	}
}