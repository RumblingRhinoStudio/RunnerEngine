using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MyEnemy", menuName = "Toolbox/Building blocks/Enemy", order = 2)]
public class Enemy : ScriptableObject
{
    public GameObject Prefab;
    public float InitialHealth;
    public float InitialDamage;
    public float InitialSpeed;
    public float InitialValue;
    public float ViewingRange;
    public float MinimumDifficulty;
    public DifficultyFormula HealthFormula;
    public DifficultyFormula DamageFormula;
    public DifficultyFormula SpeedFormula;
    public DifficultyFormula ValueFormula;
    public EnemyAI IdleAI;
    public EnemyAI PursuitAI;
    public EnemyBehaviourEvent OnDeathEvent;
    public FloatEvent OnPlayerHitEvent;

    public void SetVariablesForDifficultyLevel(EnemyBehaviour enemy, FloatReference difficulty)
    {
        if (enemy != null)
        {
            enemy.Health = HealthFormula.Calculate(difficulty.Value, InitialHealth);
            enemy.Damage = DamageFormula.Calculate(difficulty.Value, InitialDamage);
            enemy.Speed = SpeedFormula.Calculate(difficulty.Value, InitialSpeed);
            enemy.Value = ValueFormula.Calculate(difficulty.Value, InitialValue);
            enemy.ViewingRange = ViewingRange;
            enemy.AIIdle = IdleAI;
            enemy.AIPursuit = PursuitAI;
            enemy.AICurrent = IdleAI;
            enemy.OnDeathEvent = OnDeathEvent;
            enemy.OnPlayerHitEvent = OnPlayerHitEvent;
        }
    }
}
