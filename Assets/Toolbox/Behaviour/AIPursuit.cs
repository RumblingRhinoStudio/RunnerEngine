using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "MyPursuitAI", menuName = "Toolbox/Behaviour/AI Pursuit", order = 0)]
public class AIPursuit : EnemyAI
{
    public override void Initialise(GameObject enemy)
    {
        
    }

    public override void SetDestination(NavMeshAgent agent, Vector3 position, Transform target)
    {
        agent.SetDestination(target.position);
    }
}
