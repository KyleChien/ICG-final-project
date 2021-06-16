using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeGame : MonoBehaviour
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

    // generate enetities
    MazeGenerator m_MazeGenerator;
    List<string> gen_history = new List<string>();
    float gen_posx;
    float gen_posz;

    // monster
    GameObject monster;

    public EscapeGame(MazeGenerator maze_gen, GameObject monster_tmp)
    {
        m_MazeGenerator = maze_gen;
        monster = monster_tmp;
    }

    Vector3 GetRandomPos()
    {
        // check generation point
        int available_gen_pos = m_MazeGenerator.columns / 2 * m_MazeGenerator.rows / 2;
        if (m_Entities.Count > available_gen_pos)
            throw new System.ArgumentOutOfRangeException("entity amount larger than available genration positions !");
            
        do
        {
            gen_posx = Random.Range(m_MazeGenerator.columns / 2, m_MazeGenerator.columns) * m_MazeGenerator.blockWidth;
            gen_posz = Random.Range(m_MazeGenerator.rows / 2, m_MazeGenerator.rows) * m_MazeGenerator.blockHeight;
        }
        while (gen_history.Contains(string.Format("{0}_{1}", gen_posx, gen_posz)));

        gen_history.Add(string.Format("{0}_{1}", gen_posx, gen_posz));
        return new Vector3(gen_posx, 1f, gen_posz);
    }

    public void MakeGame()
    {
        m_Entities.Add(new Entity(this, "Basketball", GetRandomPos()));
        m_Entities.Add(new Entity(this, "Chair", GetRandomPos()));
        m_Entities.Add(new Entity(this, "Cup", GetRandomPos()));
        m_Entities.Add(new KeyEntity(this, "Key A", "123", GetRandomPos()));
        m_Entities.Add(new KeyEntity(this, "Key B", "124", GetRandomPos()));
        m_Entities.Add(new DoorEntity(this, "Door C", null, GetRandomPos()));
        m_Entities.Add(new DoorEntity(this, "Door D", null, GetRandomPos()));
        m_Entities.Add(new MonsterDoorEntity(this, "Door A", "123", GetRandomPos()));
        m_Entities.Add(new ExitDoorEntity(this, "Door B", "124", GetRandomPos()));
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
            string msg = m_SelectedEntity.Inspect();
            OnMessageAdded(string.Format("Inspect item <color=white>{0}</color>, \n{1}", m_SelectedEntity.Name, msg));
        }
        else
        {
            OnMessageAdded("You have to select a item first.");
        }
    }

    public void Interact(Entity entity = null)
    {

        if (m_SelectedEntity is KeyEntity)
        {
            string name = m_SelectedEntity.Name;
            m_SelectedEntity.Interact(entity);
            OnMessageAdded(string.Format("Interact with item <color=white>{0}</color>\n", name));
        }
        else if (m_SelectedEntity != null)
        {
            string msg = m_SelectedEntity.Interact(entity);
            OnMessageAdded(string.Format("Interact with item <color=white>{0}</color>, \n{1}", m_SelectedEntity.Name, msg));
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

        OnMessageAdded(string.Format("Take item <color=white>{0}</color>.", entity.Name));
        m_Entities.Remove(entity);
        m_TakenEntities.Add(entity);
        entity.Take();
        OnEntityTaken(entity);
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
        OnGameFinished(this);
    }

    public void ReleaseMonster()
    {
        Instantiate(monster, GetRandomPos(), Quaternion.identity);
    }

    public void Die()
    {
        OnMessageAdded("<color=red>You died !!!</color>\n" +
                        " returning in 3 seconds...");
        OnGameOver(this);
    }

}
