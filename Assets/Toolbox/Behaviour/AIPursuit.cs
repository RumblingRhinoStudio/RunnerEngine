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

    public override void SetDestination(NavMeshAgent agent, Transform transform, float speed, Transform target)
    {
        Vector3 point = CalculateInterceptPoint(target.position, (target.position + (Vector3.forward * PlayerSpeed.Value * Time.deltaTime)) - target.position, transform.position, speed * Time.deltaTime);
        agent.SetDestination(point + Vector3.forward);
    }


    public Vector3 CalculateInterceptPoint(Vector3 targetPosition, Vector3 targetVelocity, Vector3 position, float speed)
    {
        float a = Mathf.Pow(targetVelocity.x, 2) + Mathf.Pow(targetVelocity.z, 2) - Mathf.Pow(speed, 2);
        float b = 2 * targetPosition.x * targetVelocity.x + 2 * targetPosition.z * targetVelocity.z;
        float c = Mathf.Pow(targetPosition.x, 2) + Mathf.Pow(targetPosition.z, 2);

        float d1 = (-b + Mathf.Sqrt(Mathf.Pow(b, 2) - 4 * a * c)) / 2 * a;
        float d2 = (-b - Mathf.Sqrt(Mathf.Pow(b, 2) - 4 * a * c)) / 2 * a;

        if (d1 > 0 && d2 > 0)
        {
            return targetPosition + Vector3.forward * Mathf.Min(d1, d2) * PlayerSpeed.Value * Time.deltaTime;
        }
        else if (d1 > 0)
        {
            return targetPosition + Vector3.forward * d1 * PlayerSpeed.Value * Time.deltaTime;
        }
        else if (d2 > 0)
        {
            return targetPosition + Vector3.forward * d2 * PlayerSpeed.Value * Time.deltaTime;
        }
        else
        {
            return targetPosition;
        }
    }
}
