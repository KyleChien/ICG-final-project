using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    [SerializeField] bool m_Invincible = false;
    public bool IsInvincible { get { return m_Invincible; } }

    const float INVINCIBLE_INTERVAL = 10f;
    float m_InvincibleTimer;

    private void OnCollisionEnter(Collision collision)
    {
        var pill = collision.collider.gameObject.GetComponent<pill>();

        if (pill != null)
        {
            Destroy(pill.gameObject);
            m_Invincible = true;
            this.GetComponent<MeshRenderer>().material.color = Color.yellow;
            m_InvincibleTimer = 0;
        }
        var ghost = collision.collider.gameObject.GetComponent<ghost>();
        if (ghost != null)
        {
            if (m_Invincible)
            {
                GameObject.Destroy(ghost.gameObject);
            }
            else
            {
                Debug.Log("game over");
                //UnityEditor.EditorApplication.isPlaying = false;
            }
        }
    }

    private void Update()
    {
        if (m_Invincible)
        {
            if (m_InvincibleTimer > INVINCIBLE_INTERVAL)
            {
                m_Invincible = false;
                this.GetComponent<MeshRenderer>().material.color =
                    new Color(67f / 255f, 267f / 255f, 59f / 255f);
            }
            m_InvincibleTimer += Time.deltaTime;
        }
    }
}
