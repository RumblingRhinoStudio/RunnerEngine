using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [ReadOnly]
    public float Health;
    [ReadOnly]
    public float Damage;
    [ReadOnly]
    public float Speed;

    public void TakeDamage(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            // Die
            Destroy(gameObject);
        }
    }
}
