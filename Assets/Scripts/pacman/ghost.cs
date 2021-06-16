using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ghost : MonoBehaviour
{
    [SerializeField] GameObject m_Player;
    [SerializeField] NavMeshAgent m_Agent;

    const float WANDER_INTERVAL = 1f;
    const float WANDER_DISTANCE = 5f;
    float m_WanderTimer = -1;

    const float TRACKING_RANGE = 256f;

    private void Awake()
    {
        m_Player = FindObjectOfType<Controller>().gameObject;    
    }

    private void Update()
    {
        if ((m_Player.transform.position - this.transform.position).sqrMagnitude < TRACKING_RANGE)
        {
            m_Agent.SetDestination(m_Player.transform.position);
            m_WanderTimer = -1;
        }
        else
        {
            if (m_WanderTimer == -1 || m_WanderTimer > WANDER_INTERVAL)
            {
                Wander();
            }
            else
            {
                m_WanderTimer += Time.deltaTime;
            }
        }   
    }

    void Wander()
    {
        NavMeshHit navHit;
        Vector3 randomPoint = this.transform.position + Random.insideUnitSphere * WANDER_DISTANCE;
        if (NavMesh.SamplePosition(randomPoint, out navHit, WANDER_DISTANCE, -1))
        {
            m_Agent.SetDestination(navHit.position);
            m_WanderTimer = 0;
        }
        else
        {
            m_WanderTimer = -1;
        }
    }
}
