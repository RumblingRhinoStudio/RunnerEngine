using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    [ReadOnly]
    public float Speed = 10;
    [ReadOnly]
    public float Damage = 1;
    [ReadOnly]
    public bool PierceEnemy = false;
    [ReadOnly]
    public Transform Target;
    [ReadOnly]
    public bool stickToTarget;
    private Vector3 currentTarget;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Target != null)
        {
            currentTarget = Target.position - transform.position;
            transform.Translate(currentTarget.normalized * Speed * Time.deltaTime, Space.World);
            transform.LookAt(Target);
        }
        else
        {
            transform.Translate(currentTarget.normalized * Speed * Time.deltaTime, Space.World);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBehaviour enemy = other.GetComponent<EnemyBehaviour>();
            bool enemyDead = enemy.TakeDamage(Damage);
            if (!PierceEnemy && !stickToTarget)
            {
                Destroy(gameObject);
            }
            else if (PierceEnemy)
            {
                Target = null;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
