using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity {

	public delegate void EntityEvent(Entity e);
	public event EntityEvent OnSelected = (e) => { };
	public event EntityEvent OnDeselected = (e) => { };
	public event EntityEvent OnTaken = (e) => { };

	protected string m_Name;
	public string Name { get { return m_Name; } }

	EscapeGame m_Game;
	public EscapeGame Game { get { return m_Game; } }

	protected string m_Prefabs = "Wooden_Chest";
	public string Prefabs { get { return m_Prefabs; } }

	Vector3 m_Position;
	public Vector3 Position { get { return m_Position; } }

	public Entity (EscapeGame game, string name, Vector3 position) {
		m_Position = position;
		m_Game = game;
		m_Name = name;
	}
		
	public virtual void Inspect () { 

		Debug.Log ("Hmm...nothing special.");
	}

	public virtual void Interact (Entity entity = null) {
	
		Debug.Log ("Nothing happened.");
	}
	public virtual void Select()
    {
		OnSelected(this);
    }
	public virtual void Deselect()
    {
		OnDeselected(this);
    }
	public void Take()
    {
		OnTaken(this);
    }
}