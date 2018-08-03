using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "RunnerPlayerWeapon", menuName = "Toolbox/Weapons/Runner Player Weapon", order = 0)]
public class RunnerPlayerWeapon : ScriptableObject
{
    public FloatReference Speed;
    public FloatReference Damage;
    public bool StickToTarget;
    public bool PierceEnemy;
    public GameObject Prefab;
}