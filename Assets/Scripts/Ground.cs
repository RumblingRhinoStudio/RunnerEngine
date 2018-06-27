using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MyGround", menuName = "Building blocks/Ground", order = 2)]
public class Ground : GroundBase
{
    public bool CanBeRotated;
    public Tuple<int, int> IndexCenter { get; set; }

    public override void SetClassSpecificFields(GroundBlock block, int x, int y)
    {
        if (IndexCenter == null && block.CenterPoint)
        {
            IndexCenter = new Tuple<int, int>(x, y);
        }
    }
}