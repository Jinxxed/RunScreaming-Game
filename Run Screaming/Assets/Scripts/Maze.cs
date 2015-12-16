using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class Maze : MonoBehaviour
{

    public GameObject goalPrefab;

    public IntVector2 size;

    public MazeCell cellPrefab;

    public FirstPersonController fpcPrefab;

    public float generationStepDelay;

    public MazePassage passagePrefab;
    public MazeWall wallPrefab;

    private MazeCell[,] cells;

    private MazeCell start;

    public IntVector2 RandomCoordinates
    {
        get
        {
            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }

    public bool ContainsCoordinates(IntVector2 coordinate)
    {
        return coordinate.x >= 0 && coordinate.x < size.x && coordinate.z >= 0 && coordinate.z < size.z;
    }

    public MazeCell GetCell(IntVector2 coordinates)
    {
        return cells[coordinates.x, coordinates.z];
    }

    public void Generate()
    {
        cells = new MazeCell[size.x, size.z];
        List<MazeCell> activeCells = new List<MazeCell>();
        DoFirstGenerationStep(activeCells);
        while (activeCells.Count > 0)
        {
            DoNextGenerationStep(activeCells);
        }

        start = this.GetCell(this.RandomCoordinates);

        LinkedList<MazeCell> pathes = new LinkedList<MazeCell>();
        MazeCell goal = calculatePaths(start, pathes);
        while(goal == null)
        {
            pathes.Clear();
            start = this.GetCell(this.RandomCoordinates);
            goal = calculatePaths(start, pathes);
        }

        FirstPersonController fpc = Instantiate(fpcPrefab) as FirstPersonController;
        fpc.transform.localPosition = start.transform.localPosition;
        
        GameObject goalObject = Instantiate(goalPrefab) as GameObject;
        goalObject.transform.localPosition = goal.transform.localPosition;

    }

    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        activeCells.Add(CreateCell(RandomCoordinates));
    }

    public MazeCell calculatePaths(MazeCell activeCell, LinkedList<MazeCell> markedCells)
    {
        if (markedCells.Count > 5 * size.x)
        {
            Debug.Log("Goal Calculated!");
            Debug.Log("calculatePaths contains " + markedCells.Count + 1 + " tiles");
            return activeCell;
        }

        if (markedCells.Contains(activeCell))
            return null;
        markedCells.AddLast(activeCell);

        for (int i = 0; i < MazeDirections.Count; i++)
            if (!(activeCell.GetEdge((MazeDirection)i) is MazeWall))
            {
                LinkedList<MazeCell> path = new LinkedList<MazeCell>(markedCells);
                MazeCell goal = calculatePaths(activeCell.GetEdge((MazeDirection)i).otherCell, path);
                if (goal != null)
                {
                    return goal;
                }
            }
        return null;
    }

    private void DoNextGenerationStep(List<MazeCell> activeCells)
    {
        int currentIndex = activeCells.Count - 1;
        MazeCell currentCell = activeCells[currentIndex];
        if (currentCell.IsFullyInitialized)
        {
            activeCells.RemoveAt(currentIndex);
            return;
        }
        MazeDirection direction = currentCell.RandomUninitializedDirection;
        IntVector2 coordinates = currentCell.coordinates + direction.ToIntVector2();
        if (ContainsCoordinates(coordinates))
        {
            MazeCell neighbor = GetCell(coordinates);
            if (neighbor == null)
            {
                neighbor = CreateCell(coordinates);
                CreatePassage(currentCell, neighbor, direction);
                activeCells.Add(neighbor);
            }
            else {
                CreateWall(currentCell, neighbor, direction);
            }
        }
        else {
            CreateWall(currentCell, null, direction);
        }
    }

    private MazeCell CreateCell(IntVector2 coordinates)
    {
        MazeCell newCell = Instantiate(cellPrefab) as MazeCell;
        cells[coordinates.x, coordinates.z] = newCell;
        newCell.coordinates = coordinates;
        newCell.name = "Maze Cell " + coordinates.x + ", " + coordinates.z;
        newCell.transform.parent = transform;
        newCell.transform.localPosition = new Vector3(coordinates.x - size.x * 0.5f + 0.5f, 0f, coordinates.z - size.z * 0.5f + 0.5f);
        return newCell;
    }

    private void CreatePassage(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazePassage passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(cell, otherCell, direction);
        passage = Instantiate(passagePrefab) as MazePassage;
        passage.Initialize(otherCell, cell, direction.GetOpposite());
    }

    private void CreateWall(MazeCell cell, MazeCell otherCell, MazeDirection direction)
    {
        MazeWall wall = Instantiate(wallPrefab) as MazeWall;
        wall.Initialize(cell, otherCell, direction);
        if (otherCell != null)
        {
            wall = Instantiate(wallPrefab) as MazeWall;
            wall.Initialize(otherCell, cell, direction.GetOpposite());
        }
    }
}