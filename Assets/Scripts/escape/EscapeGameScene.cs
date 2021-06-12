using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeGameScene : MonoBehaviour {

	[SerializeField] GameUI m_GameUI;
	EscapeGame m_Game;
	public EscapeGame Game { get { return m_Game; } }

	float SELECT_RANGE = 5f;

	private void Awake()
    {
		m_Game = new EscapeGame();
		m_Game.OnGameStarted += HandleOnGameStarted;
		m_Game.OnGameFinished += HandleOnGameFinished;
		m_Game.OnGameOver += HandleOnGameOver;

		m_Game.OnMessageAdded += HandleOnMessageAdded;

		m_Game.OnEntityInteracted += HandleOnEntityInteracted;
		m_Game.OnEntityInspected += HandleOnEntityInspected;
		m_Game.OnEntitySelected += HandleOnEntitySelected;
		m_Game.OnEntityDeselected += HandleOnEntityDeselected;
		m_Game.OnEntityTaken += HandleOnEntityTaken;
		m_Game.OnEntityReleased += HandleOnEntityReleased;

		m_Game.MakeGame();
	}

	void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
			DetectEntity();
        }

		/*if (Input.GetKeyDown (KeyCode.N)) {

			m_Game.SelectNext ();

		} else if (Input.GetKeyDown (KeyCode.Space)) {

			m_Game.Inspect ();

		} else if (Input.GetKeyDown (KeyCode.Return)) {

			m_Game.Interact ();

		} else if (Input.GetKeyDown (KeyCode.R)) {

			m_Game.PutBack ();
		}*/
	}

	void DetectEntity()
    {
		Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
		bool detectEntity = false;
		RaycastHit raycastresult;
		if (Physics.Raycast(ray, out raycastresult))
        {
			if (raycastresult.distance < SELECT_RANGE)
            {
				var entityBehav = raycastresult.collider.GetComponent<EntityBehav>();
				if (entityBehav != null)
                {
					detectEntity = true;
					m_Game.SelectEntity(entityBehav.Entity);
                }
            }
        }
		if (!detectEntity)
        {
			SelectNothing();
        }
		void SelectNothing()
        {
			m_Game.SelectEntity(null);
        }
    }

	#region Message Event Handlers
	void HandleOnMessageAdded(string message) 
	{
		m_GameUI.ShowMessage(message);
	}
	#endregion

	#region Entity Event Handlers
	void HandleOnEntityInteracted(Entity entity) { }
	void HandleOnEntityInspected(Entity entity) { }
	void HandleOnEntitySelected(Entity entity) 
	{
		m_GameUI.SetActionVisible(true);
	}
	void HandleOnEntityDeselected(Entity entity) 
	{
		m_GameUI.SetActionVisible(false);
	}
	void HandleOnEntityTaken(Entity entity) 
	{
		m_GameUI.SetActionVisible(false);
		m_GameUI.UpdateTakenEntities();

	}
	void HandleOnEntityReleased(Entity entity) 
	{
		m_GameUI.UpdateTakenEntities();
		var entityBehav = GameObject.Instantiate(
					Resources.Load<EntityBehav>("prefabs/" + entity.Prefabs));
		entityBehav.transform.localPosition = entity.Position;
		entityBehav.UpdateEntity(entity);

	}
	#endregion

	#region Game Event Handlers
	void HandleOnGameStarted(EscapeGame game)
	{
		foreach (var e in m_Game.Entities)
		{
			// create entities gameobject here
			var entityBehav = GameObject.Instantiate(
					Resources.Load<EntityBehav>("prefabs/" + e.Prefabs));
			entityBehav.transform.localPosition = e.Position;
			entityBehav.UpdateEntity(e);
		}
	}
	void HandleOnGameFinished(EscapeGame game) { }
	void HandleOnGameOver(EscapeGame game) { }
	#endregion
}
