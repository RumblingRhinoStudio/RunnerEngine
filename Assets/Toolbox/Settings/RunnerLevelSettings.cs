using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "RunnerLevelSettings", menuName = "Toolbox/Settings/Runner Level Settings", order = 0)]
public class RunnerLevelSettings : ScriptableObject
{
    [Header("Level generation settings")]
    public int LevelMatrixWidth;
    public int LevelMatrixHeight;
    [Tooltip("0 based index.")]
    public int LevelMatrixRoadLane;
    public int RowsLeftBeforeNextPart;
    public int DifficultyMinLength;
    public int DifficultyMaxLength;
    public int MinRowsNoSideRoads;

    [Header("Enemy settings")]
    public int RowsBeforeEnemies;
    public int EnemiesPerLevelMatrix;

    [Header("Variables")]
    public FloatReference Difficulty;

    [Header("Building blocks")]
    public GroundList GroundObjects;
    public RoadList RoadObjects;
    public DividerList DividerObjects;
    public EnemyList EnemyObjects;
}