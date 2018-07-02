using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class LevelManager : MonoBehaviour
{
    public int LevelMatrixWidth;
    public int LevelMatrixHeight;
    [Tooltip("0 based index.")]
    public int LevelMatrixRoadLane;
    public int DifficultyMinLength;
    public int DifficultyMaxLength;


    public GroundList GroundObjects;
    public RoadList RoadObjects;
    public DividerList DividerObjects;

    private List<GameObject> groundObjectsInUse = new List<GameObject>();
    private List<GameObject> roadObjectsInUse = new List<GameObject>();
    private List<GameObject> dividersInUse = new List<GameObject>();
    private Transform playerTransform;
    private bool placingGround = false;
    private int biggestBlockHeight = 0;
    private int currentDifficultySize;
    private int currentDifficultyTargetSize;

    private Transform levelParentTransform;
    private NavMeshSurface levelNavMeshSurface;

    private float lastRowPlacedZ = 0;

    private GroundBlock[,] levelBlocks;

    // Use this for initialization
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        GameObject Level = GameObject.FindGameObjectWithTag("Level");
        levelParentTransform = Level.transform;
        levelNavMeshSurface = Level.GetComponent<NavMeshSurface>();

        foreach (Ground ground in GroundObjects.Grounds)
        {
            ground.Initialise();
            if (ground.Height > biggestBlockHeight || ground.CanBeRotated && ground.Width > biggestBlockHeight)
            {
                biggestBlockHeight = ground.CanBeRotated ? Mathf.Max(ground.Height, ground.Width) : ground.Height;
            }
        }
        foreach (Road road in RoadObjects.Roads)
        {
            road.Initialise();
        }
        foreach (Divider divider in DividerObjects.Dividers)
        {
            divider.Initialise();
        }
        currentDifficultyTargetSize = getNewDifficultyLevelLength();
        placeGround();
    }

    // Update is called once per frame
    void Update()
    {
        placeGround();
        //destroyGround();
    }

    private void placeGround()
    {
        // TODO : Change if to build next part of level sooner
        if (!placingGround && (levelBlocks == null || playerTransform.position.z >= lastRowPlacedZ))
        {
            placingGround = true;
            int currentIterationHeight = Math.Min(LevelMatrixHeight, currentDifficultyTargetSize - currentDifficultySize);
            GroundBlock[,] levelBlocksTemp = new GroundBlock[LevelMatrixWidth, currentIterationHeight];
            int currentMatrixLevel = 0;

            int currentMatrixRoadLevel = currentMatrixLevel;
            float lastPlacedRoadRowZ = lastRowPlacedZ;
            while (levelBlocksTemp[LevelMatrixRoadLane, currentIterationHeight - 1] == null)
            {
                // Figure out what size road we are going for
                int emptySpacesLeft = 0;
                int emptySpacesRight = 0;
                for (int i = LevelMatrixRoadLane - 1; i >= 0; i--)
                {
                    if (levelBlocksTemp[i, currentMatrixRoadLevel] != null) break;

                    emptySpacesLeft++;
                }
                for (int i = LevelMatrixRoadLane + 1; i < LevelMatrixWidth; i++)
                {
                    if (levelBlocksTemp[i, currentMatrixRoadLevel] != null) break;

                    emptySpacesRight++;
                }

                // Find fitting road block and instantiate it TODO: Object pooling?
                Road toPlace = findFittingRoad(emptySpacesLeft, emptySpacesRight, currentIterationHeight - currentMatrixRoadLevel);
                GameObject placedGround = Instantiate(toPlace.Prefab);
                placedGround.transform.parent = levelParentTransform;

                placedGround.transform.position = new Vector3(placedGround.transform.position.x, 0, lastPlacedRoadRowZ + 10);

                roadObjectsInUse.Add(placedGround);

                lastPlacedRoadRowZ = lastPlacedRoadRowZ + (toPlace.IndexAnchorEnd.Item2 + 1) * 10;

                // Fill matrix with newly placed block
                for (int i = 0; i < toPlace.Pieces.GetLength(0); i++)
                {
                    for (int j = 0; j < toPlace.Pieces.GetLength(1); j++)
                    {
                        levelBlocksTemp[LevelMatrixRoadLane - toPlace.IndexAnchorStart.Item1 + i, j + currentMatrixRoadLevel] = toPlace.Pieces[i, j];
                    }
                }

                currentMatrixRoadLevel += toPlace.IndexAnchorEnd.Item2 + 1;
            }

            // Fill out the rest of the matrix
            while (currentMatrixLevel < currentIterationHeight)
            {
                // Find first empty cell
                bool emptyCellFound = false;
                int column = 0;
                int row = currentMatrixLevel;
                // Iterate through all rows
                for (; row < currentIterationHeight; row++)
                {
                    column = 0;
                    // Iterate over each cell in the current row
                    for (; column < LevelMatrixWidth; column++)
                    {
                        if (levelBlocksTemp[column, row] == null)
                        {
                            emptyCellFound = true;
                            break;
                        }
                    }
                    if (emptyCellFound)
                    {
                        break;
                    }
                    else
                    {
                        currentMatrixLevel++;
                    }
                }
                if (!emptyCellFound)
                    break;


                // Find possible empty rectangles to put new block in
                List<Tuple<int, int>> emptyRectangles = new List<Tuple<int, int>>();
                int maxWidth = LevelMatrixWidth - column;
                int maxHeight = 0;
                bool done = false;
                for (int j = row; j < Mathf.Min(biggestBlockHeight + row, currentIterationHeight); j++)
                {
                    int rowWidth = 0;
                    for (int i = column; i < LevelMatrixWidth; i++)
                    {
                        if (levelBlocksTemp[i, j] != null)
                        {
                            if (i == column)
                            {
                                done = true;
                            }
                            if (maxHeight > 0 && maxWidth > rowWidth)
                            {
                                emptyRectangles.Add(new Tuple<int, int>(maxWidth, maxHeight));
                            }
                            if (rowWidth < maxWidth)
                            {
                                maxWidth = rowWidth;
                            }
                            break;
                        }
                        rowWidth++;

                    }
                    if (done) break;
                    maxHeight++;
                }
                // Make sure we have the end of the level as well
                if (!done)
                {
                    emptyRectangles.Add(new Tuple<int, int>(maxWidth, maxHeight));
                }

                // Find a block to fit into one of the empty rectangles
                Ground groundToPlace = findFittingGround(emptyRectangles);
                if (groundToPlace == null)
                    break;

                // Figure out what rotation, if any, we should give the block
                List<int> possibleRotations = new List<int>();
                if (groundToPlace.CanBeRotated)
                {
                    if (emptyRectangles.Any(x => x.Item1 >= groundToPlace.Width && x.Item2 >= groundToPlace.Height))
                    {
                        possibleRotations.Add(0);
                        possibleRotations.Add(180);
                    }
                    if (emptyRectangles.Any(x => x.Item1 >= groundToPlace.Height && x.Item2 >= groundToPlace.Width))
                    {
                        possibleRotations.Add(90);
                        possibleRotations.Add(270);
                    }
                }
                else
                {
                    possibleRotations.Add(0);
                }

                // Instantiate gameObject TODO : Object pooling?
                GameObject placedGround = Instantiate(groundToPlace.Prefab);
                placedGround.transform.parent = levelParentTransform;
                int chosenRotation = possibleRotations[Random.Range(0, possibleRotations.Count)];
                switch (chosenRotation)
                {
                    case 0:
                        placedGround.transform.position = new Vector3((column - LevelMatrixRoadLane + groundToPlace.IndexCenter.Item1) * 10, 0, lastRowPlacedZ + (row + 1) * 10);
                        break;
                    case 90:
                        placedGround.transform.Rotate(Vector3.up, 90);
                        placedGround.transform.position = new Vector3((column - LevelMatrixRoadLane) * 10, 0, lastRowPlacedZ + (row + 1 + groundToPlace.Width - 1 - groundToPlace.IndexCenter.Item1) * 10);
                        break;
                    case 180:
                        placedGround.transform.Rotate(Vector3.up, 180);
                        placedGround.transform.position = new Vector3((column - LevelMatrixRoadLane + groundToPlace.Width - 1 - groundToPlace.IndexCenter.Item1) * 10, 0, lastRowPlacedZ + (row + 1 + groundToPlace.Height - 1) * 10);
                        break;
                    case 270:
                        placedGround.transform.Rotate(Vector3.up, 270);
                        placedGround.transform.position = new Vector3((column - LevelMatrixRoadLane + groundToPlace.Height - 1) * 10, 0, lastRowPlacedZ + (row + 1 + groundToPlace.IndexCenter.Item1) * 10);
                        break;
                }


                // Fill level matrix with new groundblocks
                GroundBlock[,] groundBlocksUsed;
                // TODO : Look into making it more effective
                switch (chosenRotation)
                {
                    case 0:
                        groundBlocksUsed = groundToPlace.Pieces;
                        break;
                    case 90:
                        groundBlocksUsed = Helpers.RotateMatrix90(groundToPlace.Pieces);
                        break;
                    case 180:
                        groundBlocksUsed = Helpers.RotateMatrix180(groundToPlace.Pieces);
                        break;
                    case 270:
                        groundBlocksUsed = Helpers.RotateMatrix270(groundToPlace.Pieces);
                        break;
                    default:
                        groundBlocksUsed = new GroundBlock[0, 0];
                        break;
                }
                for (int i = 0; i < groundBlocksUsed.GetLength(0); i++)
                {
                    for (int j = 0; j < groundBlocksUsed.GetLength(1); j++)
                    {
                        levelBlocksTemp[column + i, row + j] = groundBlocksUsed[i, j];
                    }
                }
            }

            levelBlocks = levelBlocksTemp;
            int levelBlocksHeight = levelBlocks.GetLength(1);
            lastRowPlacedZ += levelBlocksHeight * 10;
            currentDifficultySize += levelBlocksHeight;

            if (currentDifficultySize == currentDifficultyTargetSize)
            {
                placeDivider();
                currentDifficultySize = 0;
                currentDifficultyTargetSize = getNewDifficultyLevelLength();
            }
            levelNavMeshSurface.BuildNavMesh();
            placingGround = false;
        }
    }

    private void placeDivider()
    {
        if (DividerObjects.Dividers.Any())
        {
            Divider dividerToPlace = DividerObjects.Dividers[Random.Range(0, DividerObjects.Dividers.Length)];
            GameObject divider = Instantiate(dividerToPlace.Prefab);
            divider.transform.position = new Vector3(divider.transform.position.x, 0, lastRowPlacedZ + 10);
            lastRowPlacedZ += dividerToPlace.Height * 10;
        }
    }

    private Ground findFittingGround(List<Tuple<int, int>> emptyRectangles)
    {
        Ground[] usableGrounds = GroundObjects.Grounds
                               .Where(x => emptyRectangles
                                           .Any(y => y.Item1 >= x.Width && y.Item2 >= x.Height
                                                  || x.CanBeRotated && y.Item1 >= x.Height && y.Item2 >= x.Width)).ToArray();
        if (!usableGrounds.Any()) return null;

        return usableGrounds[Random.Range(0, usableGrounds.Length)];
    }

    private Road findFittingRoad(int spacesLeft, int spacesRight, int maxHeight)
    {
        Road[] usableRoads = RoadObjects.Roads
                               .Where(x => x.IndexAnchorStart.Item1 <= spacesLeft
                                        && x.Width <= spacesLeft - (spacesLeft - x.IndexAnchorStart.Item1) + 1 + spacesRight
                                        && x.Height <= maxHeight).ToArray();
        if (!usableRoads.Any()) return null;

        return usableRoads[Random.Range(0, usableRoads.Length)];
    }

    private int getNewDifficultyLevelLength()
    {
        return Random.Range(DifficultyMinLength, DifficultyMaxLength);
    }
}