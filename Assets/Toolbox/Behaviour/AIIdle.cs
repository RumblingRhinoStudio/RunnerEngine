using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "MyIdleAI", menuName = "Toolbox/Behaviour/AI Idle", order = 0)]
public class AIIdle : EnemyAI
{
    public float WanderRadius;
    public float WanderTimer;

    private Transform target;
    private float timer;

    public override void Initialise(GameObject enemy)
    {
    }

    public override void SetDestination(NavMeshAgent agent, Vector3 position, float speed, Transform target)
    {
        timer += Time.deltaTime;

        if (timer >= WanderTimer)
        {
            Vector3 randDirection = Random.insideUnitSphere * WanderRadius;

            randDirection += position;

            NavMeshHit navHit;

            NavMesh.SamplePosition(randDirection, out navHit, WanderRadius, -1);

            agent.SetDestination(navHit.position);
            timer = 0;
        }
    }

}
