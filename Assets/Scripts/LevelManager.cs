using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Random = UnityEngine.Random;
using UnityEngine.AI;
using System.Text;

public class LevelManager : MonoBehaviour
{
    public RunnerLevelSettings Settings;

    private List<GameObject> groundObjectsInUse = new List<GameObject>();
    private List<GameObject> roadObjectsInUse = new List<GameObject>();
    private List<GameObject> dividersInUse = new List<GameObject>();
    private List<GameObject> enemiesInUse = new List<GameObject>();
    private List<EnemyBehaviour> idleEnemies = new List<EnemyBehaviour>();
    private Transform playerTransform;
    private bool placingGround = false;
    private int biggestBlockHeight = 0;
    private int currentDifficultySize;
    private int currentDifficultyTargetSize;

    private Transform levelParentTransform;
    private NavMeshSurface levelNavMeshSurface;

    private float lastRowPlacedZ = 0;

    private List<float> difficultyChangeDistances = new List<float>();

    private GroundBlock[,] levelBlocks;
    
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player")?.transform;
        GameObject Level = GameObject.FindGameObjectWithTag("Level");
        levelParentTransform = Level.transform;
        levelNavMeshSurface = Level.GetComponent<NavMeshSurface>();

        foreach (Ground ground in Settings.GroundObjects.Grounds)
        {
            ground.Initialise();
            if (ground.Height > biggestBlockHeight || ground.CanBeRotated && ground.Width > biggestBlockHeight)
            {
                biggestBlockHeight = ground.CanBeRotated ? Mathf.Max(ground.Height, ground.Width) : ground.Height;
            }
        }
        foreach (Road road in Settings.RoadObjects.Roads)
        {
            road.Initialise();
        }
        foreach (Divider divider in Settings.DividerObjects.Dividers)
        {
            divider.Initialise();
        }
        currentDifficultyTargetSize = getNewDifficultyLevelLength();
        placeGround();
    }
    
    void Update()
    {
        checkForDifficultyIncrease();
        placeGround();
        if (idleEnemies.Any())
        {
            for (int i = idleEnemies.Count - 1; i >= 0; i--)
            {
                EnemyBehaviour enemy = idleEnemies[i];
                if (enemy == null || enemy.gameObject == null)
                {
                    idleEnemies.RemoveAt(i);
                }
                else if (enemy.PlayerInRange(playerTransform))
                {
                    enemy.ChangeToPursuit(playerTransform);
                    idleEnemies.RemoveAt(i);
                }
            }
        }
        //destroyGround();
    }

    private void placeGround()
    {
        if (!placingGround && (levelBlocks == null || playerTransform.position.z >= lastRowPlacedZ - (Settings.RowsLeftBeforeNextPart * 10)))
        {
            placingGround = true;
            int currentIterationHeight = Math.Min(Settings.LevelMatrixHeight, currentDifficultyTargetSize - currentDifficultySize);
            GroundBlock[,] levelBlocksTemp = new GroundBlock[Settings.LevelMatrixWidth, currentIterationHeight];
            int currentMatrixLevel = 0;

            int currentMatrixRoadLevel = currentMatrixLevel;
            float lastPlacedRoadRowZ = lastRowPlacedZ;
            int rowsSinceLastSideRoad = 0;
            while (levelBlocksTemp[Settings.LevelMatrixRoadLane, currentIterationHeight - 1] == null)
            {
                // Figure out what size road we are going for
                int emptySpacesLeft = 0;
                int emptySpacesRight = 0;
                for (int i = Settings.LevelMatrixRoadLane - 1; i >= 0; i--)
                {
                    if (levelBlocksTemp[i, currentMatrixRoadLevel] != null) break;

                    emptySpacesLeft++;
                }
                for (int i = Settings.LevelMatrixRoadLane + 1; i < Settings.LevelMatrixWidth; i++)
                {
                    if (levelBlocksTemp[i, currentMatrixRoadLevel] != null) break;

                    emptySpacesRight++;
                }

                // Find fitting road block and instantiate it TODO: Object pooling?
                Road toPlace = findFittingRoad(emptySpacesLeft, emptySpacesRight, currentIterationHeight - currentMatrixRoadLevel, rowsSinceLastSideRoad);
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
                        levelBlocksTemp[Settings.LevelMatrixRoadLane - toPlace.IndexAnchorStart.Item1 + i, j + currentMatrixRoadLevel] = toPlace.Pieces[i, j];
                    }
                }

                if (toPlace.Width > 1)
                {
                    rowsSinceLastSideRoad = 0;
                }
                else
                {
                    rowsSinceLastSideRoad += toPlace.Height;
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
                    for (; column < Settings.LevelMatrixWidth; column++)
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
                int maxWidth = Settings.LevelMatrixWidth - column;
                int maxHeight = 0;
                bool done = false;
                for (int j = row; j < Mathf.Min(biggestBlockHeight + row, currentIterationHeight); j++)
                {
                    int rowWidth = 0;
                    for (int i = column; i < Settings.LevelMatrixWidth; i++)
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
                        placedGround.transform.position = new Vector3((column - Settings.LevelMatrixRoadLane + groundToPlace.IndexCenter.Item1) * 10, 0, lastRowPlacedZ + (row + 1) * 10);
                        break;
                    case 90:
                        placedGround.transform.Rotate(Vector3.up, 90);
                        placedGround.transform.position = new Vector3((column - Settings.LevelMatrixRoadLane) * 10, 0, lastRowPlacedZ + (row + 1 + groundToPlace.Width - 1 - groundToPlace.IndexCenter.Item1) * 10);
                        break;
                    case 180:
                        placedGround.transform.Rotate(Vector3.up, 180);
                        placedGround.transform.position = new Vector3((column - Settings.LevelMatrixRoadLane + groundToPlace.Width - 1 - groundToPlace.IndexCenter.Item1) * 10, 0, lastRowPlacedZ + (row + 1 + groundToPlace.Height - 1) * 10);
                        break;
                    case 270:
                        placedGround.transform.Rotate(Vector3.up, 270);
                        placedGround.transform.position = new Vector3((column - Settings.LevelMatrixRoadLane + groundToPlace.Height - 1) * 10, 0, lastRowPlacedZ + (row + 1 + groundToPlace.IndexCenter.Item1) * 10);
                        break;
                }

                GroundBlock[,] groundBlocksUsed = groundToPlace.FillPieces(placedGround);
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

            //Remember to build NavMesh before creating enemies to be able to use Nav Mesh Agent
            levelNavMeshSurface.BuildNavMesh();

            // Place enemies before calculating the currentDifficultySize
            if (Settings.EnemyObjects.Enemies.Any())
            {
                placeEnemies();
            }

            currentDifficultySize += levelBlocksHeight;

            if (currentDifficultySize == currentDifficultyTargetSize)
            {
                placeDivider();
                currentDifficultySize = 0;
                currentDifficultyTargetSize = getNewDifficultyLevelLength();
            }


            placingGround = false;

        }
    }

    private void placeEnemies()
    {
        List<GroundBlock> possibleEnemyLocations = new List<GroundBlock>();

        // Columns
        for (int i = 0; i < levelBlocks.GetLength(0); i++)
        {
            // Rows
            for (int j = Math.Max(0, Settings.RowsBeforeEnemies - currentDifficultySize); j < levelBlocks.GetLength(1); j++)
            {
                GroundBlock currentBlock = levelBlocks[i, j];
                if (currentBlock.CanHoldEnemy)
                {
                    possibleEnemyLocations.Add(currentBlock);
                }
            }
        }

        for (int i = 0; i < Settings.EnemiesPerLevelMatrix; i++)
        {
            if (possibleEnemyLocations.Any())
            {
                GroundBlock enemyBlock = possibleEnemyLocations[Random.Range(0, possibleEnemyLocations.Count)];
                possibleEnemyLocations.Remove(enemyBlock);
                Enemy enemyToPlace = findFittingEnemy();
                GameObject enemy = Instantiate(enemyToPlace.Prefab);
                NavMeshAgent agent = enemy.GetComponent<NavMeshAgent>();
                agent.enabled = false;
                EnemyBehaviour enemyBehaviour = enemy.GetComponent<EnemyBehaviour>();
                enemyToPlace.SetVariablesForDifficultyLevel(enemyBehaviour, Settings.Difficulty);
                enemy.transform.position = new Vector3(enemyBlock.transform.position.x, enemy.transform.position.y, enemyBlock.transform.position.z);
                idleEnemies.Add(enemyBehaviour);
                agent.enabled = true;
            }
            else
            {
                break;
            }
        }
    }

    private void placeDivider()
    {
        if (Settings.DividerObjects.Dividers.Any())
        {
            Divider dividerToPlace = Settings.DividerObjects.Dividers[Random.Range(0, Settings.DividerObjects.Dividers.Length)];
            GameObject divider = Instantiate(dividerToPlace.Prefab);
            divider.transform.position = new Vector3(divider.transform.position.x, 0, lastRowPlacedZ + 10);

            // Make sure we can see when the difficulty should change
            difficultyChangeDistances.Add(lastRowPlacedZ + 10f);
            lastRowPlacedZ += dividerToPlace.Height * 10;
        }
    }

    private Enemy findFittingEnemy()
    {
        Enemy[] usableEnemies = Settings.EnemyObjects.Enemies.Where(x => x.MinimumDifficulty < Settings.Difficulty.Value).ToArray();
        return usableEnemies[Random.Range(0, usableEnemies.Length)];
    }

    private Ground findFittingGround(List<Tuple<int, int>> emptyRectangles)
    {
        Ground[] usableGrounds = Settings.GroundObjects.Grounds
                               .Where(x => emptyRectangles
                                           .Any(y => y.Item1 >= x.Width && y.Item2 >= x.Height
                                                  || x.CanBeRotated && y.Item1 >= x.Height && y.Item2 >= x.Width)).ToArray();
        if (!usableGrounds.Any()) return null;

        return usableGrounds[Random.Range(0, usableGrounds.Length)];
    }

    private Road findFittingRoad(int spacesLeft, int spacesRight, int maxHeight, int rowsSinceLastSideRoad)
    {
        Road[] usableRoads = Settings.RoadObjects.Roads
                               .Where(x => x.IndexAnchorStart.Item1 <= spacesLeft
                                        && x.Width <= (rowsSinceLastSideRoad >= Settings.MinRowsNoSideRoads ? spacesLeft - (spacesLeft - x.IndexAnchorStart.Item1) + 1 + spacesRight : 1)
                                        && x.Height <= maxHeight).ToArray();
        if (!usableRoads.Any()) return null;

        return usableRoads[Random.Range(0, usableRoads.Length)];
    }

    private int getNewDifficultyLevelLength()
    {
        return Random.Range(Settings.DifficultyMinLength, Settings.DifficultyMaxLength);
    }

    private void checkForDifficultyIncrease()
    {
        if (difficultyChangeDistances.Any(x => playerTransform.position.z > x))
        {
            Settings.DifficultyIncreaseEvent.Raise(1);
            difficultyChangeDistances.RemoveAll(x => x < playerTransform.position.z);
        }
    }


    #region EventListener Functions

    public void RemoveEnemyFromIdle(EnemyBehaviour enemy)
    {
        if (idleEnemies.Contains(enemy))
        {
            idleEnemies.Remove(enemy);
        }
    }

    public void IncreaseDifficulty(float increaseBy)
    {
        if (!Settings.Difficulty.UseConstant)
        {
            Settings.Difficulty.Variable.Value += increaseBy;
        }
    }

    #endregion
}