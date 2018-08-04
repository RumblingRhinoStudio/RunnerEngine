using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "MyPursuitAI", menuName = "Toolbox/Behaviour/AI Pursuit", order = 0)]
public class AIPursuit : EnemyAI
{
    public FloatReference PlayerSpeed;

    public override void Initialise(GameObject enemy)
    {
        
    }

    public override void SetDestination(NavMeshAgent agent, Vector3 position, float speed, Transform target)
    {
        agent.SetDestination(target.position);
    }
}
