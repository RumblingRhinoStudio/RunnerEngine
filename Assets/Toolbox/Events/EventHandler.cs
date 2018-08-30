using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public FloatVariable Currency;
    public FloatVariable Difficulty;

    public void UpdateCurrencyOnEnemyDeath(EnemyBehaviour enemy)
    {
        if (enemy != null)
        {
            Currency.ApplyChange(enemy.Value);
        }
    }

    public void ChangeCurrency(float change)
    {
        Currency.ApplyChange(change);
    }

    public void ChangeDifficulty(float change)
    {
        Difficulty.ApplyChange(change);
    }

    public void ResetDifficulty()
    {
        Difficulty.ApplyChange(-Difficulty.Value);
    }
}
