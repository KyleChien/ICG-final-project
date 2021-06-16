using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaperEntity : Entity {

	string m_Content;

	public PaperEntity (EscapeGame game, string name, string content, Vector3 position) :
		base (game, name, position) {
		m_Prefabs = "Silver_Chest";
		m_Content = content;
	}

	public override string Inspect () {

		return "There is something on the paper.";
	}

	public override string Interact (Entity entity = null) {

		return string.Format ("Read the paper:<color=white>{0}</color>", m_Content);
	}
}
