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
		m_Prefabs = "Magic_Chest";
	}

	protected override string Open () {
		Game.Escape();
		return "<color=green>Congrats! You escape the room!</color>\n" +
				"returning in 3 seconds...";
	}
}

public class MonsterDoorEntity : DoorEntity {

	public MonsterDoorEntity (EscapeGame game, string name, string keyIdentifier, Vector3 position) :
	base (game, name, keyIdentifier, position) {
		m_Prefabs = "Death_Chest";
	}

	protected override string Open () {
		Game.ReleaseMonster();
		return "<color=red> You release a monster. Be careful !!!</color>";
	}
}
