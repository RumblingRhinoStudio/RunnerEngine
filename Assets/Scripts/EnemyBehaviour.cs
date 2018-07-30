using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [ReadOnly]
    public float Health;
    [ReadOnly]
    public float Damage;
    [ReadOnly]
    public float Speed;
    [ReadOnly]
    public float ViewingRange;

    public EnemyAI AIIdle { get; set; }
    public EnemyAI AIPursuit { get; set; }
    public EnemyAI AICurrent { get; set; }
    
    private float timer;
    private NavMeshAgent agent;

    public void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
    }

    public void Update()
    {
        AICurrent.SetDestination(agent, transform.position);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            // Die
            Destroy(gameObject);
        }
    }

    public bool PlayerInRange(Transform player)
    {
        return false;
    }

    public void ChangeToPursuit()
    {
        AICurrent = AIPursuit;
    }
}