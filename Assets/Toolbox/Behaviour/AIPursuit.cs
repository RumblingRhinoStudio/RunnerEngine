using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "MyPursuitAI", menuName = "Toolbox/Behaviour/AI Pursuit", order = 0)]
public class AIPursuit : EnemyAI
{
    public override void Initialise(GameObject enemy)
    {
        throw new System.NotImplementedException();
    }

    public override void SetDestination(NavMeshAgent agent, Vector3 position)
    {
        throw new System.NotImplementedException();
    }
}
