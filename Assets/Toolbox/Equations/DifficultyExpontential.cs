using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "MyExponentialDifficulty", menuName = "Toolbox/Equations/Difficulty Exponential", order = 0)]
public class DifficultyExponential : DifficultyFormula
{
    [ReadOnly]
    public string Description = "y = x^(A*difficulty) + B*x*difficulty + C*difficulty";
    public float A;
    public float B;
    public float C;

    public override float Calculate(float difficulty, float initial)
    {
        return Mathf.Pow(initial, A * difficulty) + B * initial * difficulty + C * difficulty;
    }
}