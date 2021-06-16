using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyEntity : Entity {

	string m_KeyIdentifier;

	public string Identifier { get { return m_KeyIdentifier; } }

	public KeyEntity (EscapeGame game, string name, string keyIdentifier, Vector3 position) : 
		base (game, name, position) {
		m_Prefabs = "Golden_Chest";
		m_KeyIdentifier = keyIdentifier;
	}

	public override string Inspect () {

		return "A key for something. Maybe can be used later.";
	}

	public override string Interact (Entity entity = null) {

		Game.Take (this);
		return "";		// dummy, never return
	}
}
