using UnityEngine;
using System.Collections;

public static class Helpers 
{
    public static GroundBlock[,] RotateMatrix90(GroundBlock[,] oldMatrix)
    {
        int newDimensionX = oldMatrix.GetLength(1);
        int newDimensionY = oldMatrix.GetLength(0);
        GroundBlock[,] newMatrix = new GroundBlock[newDimensionX, newDimensionY];

        for (int newX = 0; newX < newDimensionX; newX++)
        {
            for (int newY = 0; newY < newDimensionY; newY++)
            {
                newMatrix[newX, newY] = oldMatrix[newDimensionY - 1 - newY, newX];
            }
        }
        return newMatrix;
    }

    public static GroundBlock[,] RotateMatrix180(GroundBlock[,] oldMatrix)
    {
        int newDimensionX = oldMatrix.GetLength(0);
        int newDimensionY = oldMatrix.GetLength(1);
        GroundBlock[,] newMatrix = new GroundBlock[newDimensionX, newDimensionY];

        for (int newX = 0; newX < newDimensionX; newX++)
        {
            for (int newY = 0; newY < newDimensionY; newY++)
            {
                newMatrix[newX, newY] = oldMatrix[newDimensionX - 1 - newX, newDimensionY - 1 - newY];
            }
        }
        return newMatrix;
    }

    public static GroundBlock[,] RotateMatrix270(GroundBlock[,] oldMatrix)
    {
        int newDimensionX = oldMatrix.GetLength(1);
        int newDimensionY = oldMatrix.GetLength(0);
        GroundBlock[,] newMatrix = new GroundBlock[newDimensionX, newDimensionY];

        for (int newX = 0; newX < newDimensionX; newX++)
        {
            for (int newY = 0; newY < newDimensionY; newY++)
            {
                newMatrix[newX, newY] = oldMatrix[newY, newDimensionX - 1 - newX];
            }
        }
        return newMatrix;
    }
}
