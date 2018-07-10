using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "MyEnemy", menuName = "Toolbox/Building blocks/Enemy", order = 2)]
public class Enemy : ScriptableObject
{
    public GameObject Prefab;
    public float InitialHealth;
    public float InitialDamage;
    public float InitialSpeed;

    public void SetVariablesForDifficultyLevel(EnemyBehaviour enemy, FloatReference difficulty, DifficultyFormula healthFormula, DifficultyFormula damageFormula, DifficultyFormula speedFormula)
    {
        if (enemy != null)
        {
            enemy.Health = healthFormula.Calculate(difficulty.Value, InitialHealth);
            enemy.Damage = damageFormula.Calculate(difficulty.Value, InitialDamage);
            enemy.Speed = speedFormula.Calculate(difficulty.Value, InitialSpeed);
        }
    }
}
