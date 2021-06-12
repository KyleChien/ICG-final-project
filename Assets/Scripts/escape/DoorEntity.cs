using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEntity : OpenableEntity {

	public DoorEntity (EscapeGame game, string name, string keyIdentifier, Vector3 position) :
		base (game, name, keyIdentifier, position) {
		m_Prefabs = "Magic_Chest";
	}
}

public class ExitDoorEntity : DoorEntity {

	public ExitDoorEntity (EscapeGame game, string name, string keyIdentifier, Vector3 position) :
	base (game, name, keyIdentifier, position) {

	}

	protected override void Open () {

		Debug.Log ("It's the right exit.");

		Game.Escape ();
	}
}

public class MonsterDoorEntity : DoorEntity {

	public MonsterDoorEntity (EscapeGame game, string name, string keyIdentifier, Vector3 position) :
	base (game, name, keyIdentifier, position) {
		m_Prefabs = "Death_Chest";
	}

	protected override void Open () {

		Debug.Log ("<color=red>You release a monster.</color>");

		Game.Die ();
	}
}
