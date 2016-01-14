using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.Characters.FirstPerson;

public class Maze : MonoBehaviour
{

    public GameObject goalPrefab;
    private GameObject goalObj;

    public IntVector2 size;

    public MazeCell cellPrefab;

    public FirstPersonController fpcPrefab;
    private FirstPersonController fpcObj;

    public float generationStepDelay;

    public MazePassage passagePrefab;
    public MazeWall wallPrefab;

    public GameObject[] decorationPrefabs;

    private LinkedList<GameObject> deco;

    public Enemy enemyPrefab;

    private Enemy[] enemies;

    private MazeCell[,] cells;

    private MazeCell start;



    public IntVector2 RandomCoordinates
    {
        get
        {
            return new IntVector2(Random.Range(0, size.x), Random.Range(0, size.z));
        }
    }

    public void OnDestroy()
    {
        Destroy(this.goalObj);
        Destroy(this.fpcObj.gameObject);
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

        LinkedList<MazeCell> path = new LinkedList<MazeCell>();
        MazeCell goal = calculatePaths(start, path);
        while (goal == null)
        {
            path.Clear();
            start = this.GetCell(this.RandomCoordinates);
            goal = calculatePaths(start, path);
        }



        goalObj = Instantiate(goalPrefab) as GameObject;
        goalObj.transform.localPosition = goal.transform.localPosition + 0.6f * Vector3.up;

        goal.setGoal();
        goal.gameObject.tag = "Goal";

        /*this.enemies = new Enemy[1];

        LinkedList<MazeCell> path2 = new LinkedList<MazeCell>();
        path2.AddLast(start);

        this.generateEnemy(path2);

        Debug.Log("Spawned Enemy!");*/

        deco = new LinkedList<GameObject>();

        for (int i = 0; i < 80; i++)
        {
            int j = (int)(Random.value * decorationPrefabs.Length * 0.99f);

            MazeCell spwanpoint = this.GetCell(this.RandomCoordinates);

            if (spwanpoint == start)
            {
                continue;
            }
            deco.AddLast(Instantiate(decorationPrefabs[j]) as GameObject);
            deco.Last.Value.transform.localPosition = spwanpoint.transform.localPosition + Random.value * 0.5f * Vector3.left + Random.value * 0.5f * Vector3.back + Random.value * 0.5f * Vector3.forward + Random.value * 0.5f * Vector3.right;
            deco.Last.Value.transform.localRotation = Quaternion.AngleAxis(Random.value * 360, Vector3.up);
        }
    }

    public void prePareForScreenshot()
    {
        foreach (GameObject go in deco)
        {
            go.transform.localScale = go.transform.localScale * 5;
        }

        foreach (Light l in FindObjectsOfType<Light>())
        {
            l.shadows = LightShadows.None;
            if (l.type == LightType.Spot)
            {
                l.intensity = 2.5f;
                l.range = 5f;
            }
        }
    }

    public void prepareForGame()
    {
        foreach (GameObject go in deco)
        {
            go.transform.localScale = go.transform.localScale * 0.2f;
        }

        foreach (Light l in FindObjectsOfType<Light>())
        {
            l.shadows = LightShadows.Hard;
            if (l.type == LightType.Spot)
            {
                l.intensity = 5;
            }
        }
        fpcObj = Instantiate(fpcPrefab) as FirstPersonController;
        fpcObj.transform.localPosition = start.transform.localPosition;

        this.goalObj.GetComponent<MeshRenderer>().enabled = false;
    }



    private int generateEnemy(LinkedList<MazeCell> startPath)
    {
        MazeCell activeCell = startPath.First.Value;



        if (startPath.Count > 10)
        {
            this.enemies[0] = Instantiate(this.enemyPrefab) as Enemy;

            this.enemies[0].setPath(startPath);

            this.enemies[0].gameObject.transform.localPosition = activeCell.gameObject.transform.localPosition + 0.6f * Vector3.up;
            return 0;
        }

        for (int i = 0; i < MazeDirections.Count; i++)
            if (!(activeCell.GetEdge((MazeDirection)i) is MazeWall))
            {
                LinkedList<MazeCell> newPath = new LinkedList<MazeCell>(startPath);

                if (startPath.Contains(activeCell.GetEdge((MazeDirection)i).otherCell))
                {
                    continue;
                }
                newPath.AddFirst(activeCell.GetEdge((MazeDirection)i).otherCell);
                int success = generateEnemy(newPath);
                if (success == 0)
                {
                    return 0;
                }
            }
        return -1;
    }

    private void DoFirstGenerationStep(List<MazeCell> activeCells)
    {
        activeCells.Add(CreateCell(RandomCoordinates));
    }

    public MazeCell calculatePaths(MazeCell activeCell, LinkedList<MazeCell> path)
    {
        if (path.Count > 50)
        {
            Debug.Log("Goal Calculated!");
            Debug.Log("Calculated path contains " + (path.Count + 1) + " tiles");
            return activeCell;
        }

        if (path.Contains(activeCell))
            return null;
        path.AddLast(activeCell);

        for (int i = 0; i < MazeDirections.Count; i++)
            if (!(activeCell.GetEdge((MazeDirection)i) is MazeWall))
            {
                LinkedList<MazeCell> newPath = new LinkedList<MazeCell>(path);
                MazeCell goal = calculatePaths(activeCell.GetEdge((MazeDirection)i).otherCell, newPath);
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
                if (Random.value < 0.85)
                {

                    CreateWall(currentCell, neighbor, direction);
                }
                else
                {
                    Debug.Log("No Wall");
                    CreatePassage(currentCell, neighbor, direction);
                }

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