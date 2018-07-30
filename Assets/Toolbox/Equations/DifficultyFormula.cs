using UnityEngine;
using System.Collections;

public abstract class DifficultyFormula : ScriptableObject
{
    public abstract float Calculate(float difficulty, float initial);
}
