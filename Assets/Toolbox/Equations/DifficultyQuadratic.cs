using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "MyQuadraticDifficulty", menuName = "Toolbox/Equations/Difficulty Quadratic", order = 0)]
public class DifficultyQuadratic : DifficultyFormula
{
    [ReadOnly]
    public string Description = "y = A*difficulty*x^2 + B*x*difficulty + C*difficulty";
    public float A;
    public float B;
    public float C;

    public override float Calculate(float difficulty, float initial)
    {
        return A * difficulty * Mathf.Pow(initial, 2) + B * initial * difficulty + C * difficulty;
    }
}