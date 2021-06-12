using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBehav : MonoBehaviour
{
    MeshRenderer m_Renderer;
    Animator anim;

    Entity m_Entity;
    public Entity Entity { get { return m_Entity; } }

    private void Start()
    {
        m_Renderer = this.GetComponent<MeshRenderer>();
        anim = this.GetComponent<Animator>();
    }
    public void UpdateEntity(Entity entity)
    {
        m_Entity = entity;
        m_Entity.OnSelected += HandleOnSelected;
        m_Entity.OnDeselected += HandleOnDeselected;
        m_Entity.OnTaken += HandleOnTaken;
    }

    private void OnDestroy()
    {
        if (m_Entity != null)
        {
            m_Entity.OnSelected -= HandleOnSelected;
            m_Entity.OnDeselected -= HandleOnDeselected;
            m_Entity.OnTaken -= HandleOnTaken;
        }
    }

    #region Event Handlers
    void HandleOnSelected(Entity e) 
    {
        //m_Renderer.material.color = Color.yellow; 
        anim.SetBool("open", true);
    }
    void HandleOnDeselected(Entity e) 
    { 
        //m_Renderer.material.color = Color.white;
        anim.SetBool("open", false);
    }
    void HandleOnTaken(Entity e) 
    {
        GameObject.Destroy(this.gameObject);
    }
    #endregion
}
