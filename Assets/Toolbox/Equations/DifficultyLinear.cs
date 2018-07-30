using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "MyLinearDifficulty", menuName = "Toolbox/Equations/Difficulty Linear", order = 0)]
public class DifficultyLinear : DifficultyFormula
{
    [ReadOnly]
    public string Description = "y = A*x*difficulty + B*difficulty + C*x";
    public float A;
    public float B;
    public float C;

    public override float Calculate(float difficulty, float initial)
    {
        return A * initial * difficulty + B * difficulty + C * initial;
    }
}
