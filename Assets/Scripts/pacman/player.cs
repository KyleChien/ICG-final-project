using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{

    const float INVINCIBLE_INTERVAL = 10f;
    float m_InvincibleTimer;

    private void OnCollisionEnter(Collision collision)
    {
        var ghost = collision.collider.gameObject.GetComponent<ghost>();
        if (ghost != null)
        {
            Debug.Log("game over");
        }
    }
}
