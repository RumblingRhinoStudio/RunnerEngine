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

    public LevelManager LevelManager { get; set; }

    private NavMeshAgent agent;
    private Transform target;

    public void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = Speed;
    }

    public void Update()
    {
        AICurrent.SetDestination(agent, transform.position, target);
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            // Die hard
            Destroy(gameObject);
            LevelManager.RemoveKilledEnemy(this);
        }
    }

    public bool PlayerInRange(Transform player)
    {
        return Vector3.Distance(player.position, transform.position) < ViewingRange;
    }

    public void ChangeToPursuit(Transform target)
    {
        this.target = target;
        AICurrent = AIPursuit;
    }
}