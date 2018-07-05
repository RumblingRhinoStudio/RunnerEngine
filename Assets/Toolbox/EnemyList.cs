using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MyEnemyList", menuName = "Building blocks/Enemy List", order = 3)]
public class EnemyList : MonoBehaviour
{
    public Enemy[] Enemies;
}
