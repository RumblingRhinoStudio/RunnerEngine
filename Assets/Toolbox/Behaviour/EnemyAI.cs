using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAI : ScriptableObject
{
    public abstract void Initialise(GameObject enemy);

    public abstract void SetDestination(NavMeshAgent agent, Transform transform, float speed, Transform target);
}