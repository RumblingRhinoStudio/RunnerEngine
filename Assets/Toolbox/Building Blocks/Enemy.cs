using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "MyEnemy", menuName = "Toolbox/Building blocks/Enemy", order = 2)]
public class Enemy : ScriptableObject
{
    public GameObject Prefab;
    public float InitialHealth;
    public float InitialDamage;
    public float InitialSpeed;
    public DifficultyFormula HealthFormula;
    public DifficultyFormula DamageFormula;
    public DifficultyFormula SpeedFormula;

    public void SetVariablesForDifficultyLevel(EnemyBehaviour enemy, FloatReference difficulty)
    {
        if (enemy != null)
        {
            enemy.Health = HealthFormula.Calculate(difficulty.Value, InitialHealth);
            enemy.Damage = DamageFormula.Calculate(difficulty.Value, InitialDamage);
            enemy.Speed = SpeedFormula.Calculate(difficulty.Value, InitialSpeed);
        }
    }
}
