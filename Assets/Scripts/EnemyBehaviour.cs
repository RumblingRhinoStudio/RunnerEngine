using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public float Life = 1;
    public float Speed = 10;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Damage(float damage)
    {
        Life -= damage;
        if (Life <= 0)
        {
            // Die
            Destroy(gameObject);
        }
    }
}
