using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenableEntity : Entity {
	
	string m_KeyIdentifier;

	public OpenableEntity (EscapeGame game, string name, string keyIdentifier, Vector3 position) :
		base (game, name, position) {

		m_KeyIdentifier = keyIdentifier;
	}

	public override string Inspect () {

		if (string.IsNullOrEmpty (m_KeyIdentifier)) {

			base.Inspect ();
			return "";
		}

		return "Use the right key to open this.";
	}

	public override string Interact (Entity entity = null) {

		if (string.IsNullOrEmpty (m_KeyIdentifier)) {

			string msg = Open ();
			return msg;
		}

		KeyEntity key = entity as KeyEntity;

		if (key != null) 
		{
			if (key.Identifier == m_KeyIdentifier) 
			{
				string msg = Open ();
				return msg;
			}
			else 
			{
				return string.Format ("This item cannot be opened by the key <color=white>{0}</color>", key.Name);
			}
		} 
		else 
		{
			return "You need a key to open it.";
		}
	}

	protected virtual string Open () { 

		return "Succeed to open the item, but nothing happened.";
	}
}
