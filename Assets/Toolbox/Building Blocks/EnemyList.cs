using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "MyEnemyList", menuName = "Toolbox/Building blocks/Enemy List", order = 3)]
public class EnemyList : PrefabList
{
    public Enemy[] Enemies;
}
