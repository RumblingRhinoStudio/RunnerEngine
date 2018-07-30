using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

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

        Pieces = FillPieces(Prefab, true);
    }

    public virtual GroundBlock[,] FillPieces(GameObject prefab, bool initialise = false)
    {
        GroundBlock[] children = prefab.GetComponentsInChildren<GroundBlock>();
        Tuple<float, float>[] positions = children.Select(x => new Tuple<float, float>(x.transform.position.x, x.transform.position.z)).ToArray();
        float xmin = positions.Min(x => x.Item1);
        float xmax = positions.Max(x => x.Item1);
        float zmin = positions.Min(x => x.Item2);
        float zmax = positions.Max(x => x.Item2);

        int width = Mathf.Abs((int)(xmax - xmin) / 10) + 1;
        int height = Mathf.Abs((int)(zmax - zmin) / 10) + 1;
        GroundBlock[,] pieces = new GroundBlock[width, height];
        foreach (GroundBlock child in children)
        {
            int x = Mathf.Abs((int)(child.transform.position.x - xmin) / 10);
            int y = Mathf.Abs((int)(child.transform.position.z - zmin) / 10);
            pieces[x, y] = child;
            if (initialise)
            {
                SetClassSpecificFields(child, x, y);
            }
        }
        if (initialise)
        {
            Width = width;
            Height = height;
        }
        return pieces;
    }

    public abstract void SetClassSpecificFields(GroundBlock block, int x, int y);
}
