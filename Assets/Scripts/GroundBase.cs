using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public abstract class GroundBase : ScriptableObject
{ 
    public GameObject Prefab;
    public Transform Transform { get; set; }

    public GroundBlock[,] Pieces { get; set; }
    public int Height { get; set; }
    public int Width { get; set; }

    public virtual void Initialise()
    {
        Transform = Prefab.transform;

        GroundBlock[] children = Prefab.GetComponentsInChildren<GroundBlock>();
        Tuple<float, float>[] positions = children.Select(x => new Tuple<float, float>(x.transform.position.x, x.transform.position.z)).ToArray();
        float xmin = positions.Min(x => x.Item1);
        float xmax = positions.Max(x => x.Item1);
        float zmin = positions.Min(x => x.Item2);
        float zmax = positions.Max(x => x.Item2);

        Width = Mathf.Abs((int)(xmax - xmin) / 10) + 1;
        Height = Mathf.Abs((int)(zmax - zmin) / 10) + 1;
        Pieces = new GroundBlock[Width, Height];
        foreach (GroundBlock child in children)
        {
            int x = Mathf.Abs((int)(child.transform.position.x - xmin) / 10);
            int y = Mathf.Abs((int)(child.transform.position.z - zmin) / 10);
            Pieces[x, y] = child;
            SetClassSpecificFields(child, x, y); 
        }
    }

    public abstract void SetClassSpecificFields(GroundBlock block, int x, int y);
}
