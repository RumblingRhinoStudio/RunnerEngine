﻿using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "MyRoad", menuName = "Building blocks/Road", order = 1)]
public class Road : GroundBase
{
    public Tuple<int, int> IndexAnchorStart { get; set; }
    public Tuple<int, int> IndexAnchorEnd { get; set; }

    public override void SetClassSpecificFields(GroundBlock block, int x, int y)
    {
        if (IndexAnchorStart == null || IndexAnchorEnd == null)
        {
            if (block.RoadAnchorStart)
            {
                IndexAnchorStart = new Tuple<int, int>(x, y);
            }
            if (block.RoadAnchorEnd)
            {
                IndexAnchorEnd = new Tuple<int, int>(x, y);
            }
        }
    }
    
}