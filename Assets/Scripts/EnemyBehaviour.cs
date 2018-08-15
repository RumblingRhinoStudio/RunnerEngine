using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyBehaviour : MonoBehaviour
{
    [ReadOnly]
    public float Health;
    [ReadOnly]
    public float Damage;
    [ReadOnly]
    public float Speed;
    [ReadOnly]
    public float Value;
    [ReadOnly]
    public float ViewingRange;

    public EnemyAI AIIdle { get; set; }
    public EnemyAI AIPursuit { get; set; }
    public EnemyAI AICurrent { get; set; }

    public EnemyBehaviourEvent OnDeathEvent { get; set; }
    public FloatEvent OnPlayerHitEvent { get; set; }

    private NavMeshAgent agent;
    private Transform target;

    public void Start()
    {
        agent = gameObject.GetComponent<NavMeshAgent>();
        agent.speed = Speed;
    }

    public void Update()
    {
        AICurrent.SetDestination(agent, transform, Speed, target);
    }

    public bool TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            // Die hard
            die();
            return true;
        }
        return false;
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            OnPlayerHitEvent.Invoke(Damage);
            die();
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

    private void die()
    {
        Destroy(gameObject);
        OnDeathEvent.Invoke(this);
    }
}