using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeGame
{

    public delegate void EscapeMessageEvent(string message);
    public delegate void EscapeGameEvent(EscapeGame game);
    public delegate void EscapeGameEntityEvent(Entity entity);

    public event EscapeMessageEvent OnMessageAdded = (m) => { };

    public event EscapeGameEntityEvent OnEntitySelected = (e) => { };
    public event EscapeGameEntityEvent OnEntityDeselected = (e) => { };
    public event EscapeGameEntityEvent OnEntityInspected = (e) => { };
    public event EscapeGameEntityEvent OnEntityInteracted = (e) => { };
    public event EscapeGameEntityEvent OnEntityTaken = (e) => { };
    public event EscapeGameEntityEvent OnEntityReleased = (e) => { };

    public event EscapeGameEvent OnGameStarted = (g) => { };
    public event EscapeGameEvent OnGameOver = (g) => { };
    public event EscapeGameEvent OnGameFinished = (g) => { };


    List<Entity> m_Entities = new List<Entity>();
    public List<Entity> Entities { get { return m_Entities; } }


    int m_SelectedIndex = -1;
    Entity m_SelectedEntity = null;

    List<Entity> m_TakenEntities = new List<Entity>();
    public List<Entity> TakenEntities { get { return m_TakenEntities; } }

    Entity m_TakenEntity = null;
    public Entity TakenEntity { get { return m_TakenEntity; } }

    MazeGenerator m_MazeGenerator;

    public EscapeGame(MazeGenerator maze_gen)
    {
        m_MazeGenerator = maze_gen;
        Debug.Log("You are in a locked room. Do something to escape!");
        Debug.Log("Press 'N' to select item; " +
            "'R' to putback taken item; " +
            "'Space' to inspect selected item; " +
            "'Enter' to interact with the selected item.");
    }

    void Finish()
    {
        Debug.Log("Thanks for playing the game!");
        UnityEditor.EditorApplication.isPlaying = false;
    }

    Vector3 GetRandomPos()
    {
        float gen_posx = Random.Range(m_MazeGenerator.columns / 2, m_MazeGenerator.columns) * m_MazeGenerator.blockWidth;
        float gen_posz = Random.Range(m_MazeGenerator.rows / 2, m_MazeGenerator.rows) * m_MazeGenerator.blockHeight;
        return new Vector3(gen_posx, 1f, gen_posz);
    }

    public void MakeGame()
    {
        m_Entities.Add(new Entity(this, "Basketball", GetRandomPos()));
        m_Entities.Add(new Entity(this, "Chair", GetRandomPos()));
        m_Entities.Add(new Entity(this, "Cup", GetRandomPos()));
        m_Entities.Add(new KeyEntity(this, "Key A", "123", GetRandomPos()));
        m_Entities.Add(new KeyEntity(this, "Key B", "124", GetRandomPos()));
        m_Entities.Add(new DoorEntity(this, "Door A", null, GetRandomPos()));
        m_Entities.Add(new DoorEntity(this, "Door B", null, GetRandomPos()));
        m_Entities.Add(new MonsterDoorEntity(this, "Door C", "123", GetRandomPos()));
        m_Entities.Add(new ExitDoorEntity(this, "Door D", "124", GetRandomPos()));
        m_Entities.Add(new BoxEntity(this, "Box A", null, null, GetRandomPos()));
        m_Entities.Add(new BoxEntity(this, "Box B", new KeyEntity(this, "Key C", "125", GetRandomPos()), null, GetRandomPos()));
        m_Entities.Add(new PaperEntity(this, "Paper A", "Find a key to escape the room.", GetRandomPos()));

        OnGameStarted(this);
    }

    public void SelectEntity(Entity entity)
    {
        if (m_SelectedEntity == entity)
        {
            return;
        }
        if (m_SelectedEntity != null)
        {
            m_SelectedEntity.Deselect();
            OnEntityDeselected(m_SelectedEntity);

            OnMessageAdded(string.Format("<color=white>{0}</color> has been deselected.", m_SelectedEntity.Name));
            m_SelectedEntity = null;
        }
        if (entity != null)
        {
            m_SelectedEntity = entity;
            m_SelectedEntity.Select();
            OnEntitySelected(m_SelectedEntity);

            OnMessageAdded(string.Format("<color=white>{0}</color> has been selected.", m_SelectedEntity.Name));
        }
    }

    public void Inspect()
    {

        if (m_SelectedEntity != null)
        {

            OnMessageAdded(string.Format("Inspect item <color=white>{0}</color>", m_SelectedEntity.Name));
            m_SelectedEntity.Inspect();

        }
        else
        {
            OnMessageAdded("You have to select a item first.");
        }
    }

    public void Interact(Entity entity = null)
    {

        if (m_SelectedEntity != null)
        {
            OnMessageAdded(string.Format("Interact with item <color=white>{0}</color>", m_SelectedEntity.Name));
            m_SelectedEntity.Interact(entity);
        }
        else
        {
            OnMessageAdded("You have to select a item first.");
        }
    }

    public void SelectNext()
    {

        if (m_Entities.Count == 0)
        {

            Deselect();
            OnMessageAdded("There is nothing in this room.");
            return;
        }

        if (++m_SelectedIndex >= m_Entities.Count)
        {

            m_SelectedIndex = 0;
        }

        m_SelectedEntity = m_Entities[m_SelectedIndex];

        OnMessageAdded(string.Format("<color=white>{0}</color> has been selected.", m_SelectedEntity.Name));
    }

    public void Take(Entity entity)
    {
        if (m_SelectedEntity == entity)
        {
            m_SelectedEntity.Deselect();
            m_SelectedEntity = null;
        }

        OnMessageAdded(string.Format("Take item <color=white>{0}</color>, press 'R' to put back.", entity.Name));
        m_Entities.Remove(entity);
        m_TakenEntities.Add(entity);
        entity.Take();
        OnEntityTaken(entity);


        /*if (m_TakenEntity != null)
        {

            Debug.Log(string.Format("You already take item <color=white>{0}</color>", m_TakenEntity.Name));

        }
        else
        {

            Debug.Log(string.Format("Take item <color=white>{0}</color>, press 'R' to put back.", entity.Name));

            m_TakenEntity = entity;
            m_Entities.Remove(entity);

            Deselect();
        }*/
    }

    public void PutBack(Entity entity)
    {
        OnMessageAdded(string.Format("Put item <color=white>{0}</color> back.", entity.Name));
        m_TakenEntities.Remove(entity);
        m_Entities.Add(entity);

        OnEntityReleased(entity);
    }

    void Deselect()
    {

        m_SelectedIndex = -1;
        m_SelectedEntity = null;
    }

    public void Escape()
    {

        OnMessageAdded("<color=green>Congrats! You escape the room!</color>");
        Finish();
    }

    public void Die()
    {

        OnMessageAdded("<color=red>Oops! You died.</color>");
        Finish();
    }
}
