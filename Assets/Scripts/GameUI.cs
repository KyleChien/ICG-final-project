using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] EscapeGameScene m_GameScene;
    [SerializeField] Text m_MessageText;
    [SerializeField] GameObject m_Actions;
    [SerializeField] Dropdown m_TakenEntitiesDropdown;

    private void Start()
    {
        UpdateTakenEntities();
    }

    public void ShowMessage(string msg) 
    {
        m_MessageText.text = msg;
    }
    public void SetActionVisible (bool visible) 
    {
        m_Actions.SetActive(visible);
    }
    public void Interact () 
    {
        m_GameScene.Game.Interact();
    }
    public void Inspect () 
    {
        m_GameScene.Game.Inspect();
    }
    public void UpdateTakenEntities()
    {
        m_TakenEntitiesDropdown.ClearOptions();
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();

        foreach(var e in m_GameScene.Game.TakenEntities)
        {
            options.Add(new Dropdown.OptionData() { text = e.Name });
        }

        m_TakenEntitiesDropdown.options = options;
        m_TakenEntitiesDropdown.gameObject.SetActive(options.Count > 0);
    }

    public void UseEntity()
    {
        var entity = m_GameScene.Game.TakenEntities[m_TakenEntitiesDropdown.value];
        m_GameScene.Game.Interact(entity);
    }
    public void PutbackEntity()
    {
        var entity = m_GameScene.Game.TakenEntities[m_TakenEntitiesDropdown.value];
        m_GameScene.Game.PutBack(entity);
    }
}
