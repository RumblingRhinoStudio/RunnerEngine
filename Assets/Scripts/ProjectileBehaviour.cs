using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehaviour : MonoBehaviour
{
    public float ProjectileSpeed = 10;
    public float Damage = 1;
    public bool PierceEnemy = false;
    public Transform Target;
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
            transform.Translate(currentTarget.normalized * ProjectileSpeed * Time.deltaTime, Space.World);
            transform.LookAt(Target);
        }
        else
        {
            transform.Translate(currentTarget.normalized * ProjectileSpeed * Time.deltaTime, Space.World);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyBehaviour enemy = other.GetComponent<EnemyBehaviour>();
            enemy.Damage(Damage);
            if (!PierceEnemy)
            {
                Destroy(gameObject);
            }
        }
    }
}
