using UnityEngine;
using System.Collections;

public class GameOfLife : MonoBehaviour
{
    #region Variables

    [Header("Grid Settings")]
    [SerializeField] private int gridWidth = 50;
    [SerializeField] private int gridHeight = 50;
    [SerializeField] private float cellSize = 0.5f;
    
    [Header("Simulation Settings")]
    [SerializeField] private float updateInterval = 0.1f;
    [SerializeField] private bool autoStart = true;
    
    [Header("Cell Prefab")]
    [SerializeField] private GameObject cellPrefab;
    
    [Header("Cell Colors")]
    [SerializeField] private Color aliveColor = Color.white;
    [SerializeField] private Color deadColor = Color.black;
    
    private bool[,] currentGrid;
    private bool[,] nextGrid;
    private Cell[,] cellObjects;
    private bool isRunning = false;
    private Coroutine simulationCoroutine;

    #endregion

    void Start()
    {
        InitializeGrid();
        CreateCellObjects();
        
        if (autoStart)
        {
            StartSimulation();
        }
    }

    void InitializeGrid()
    {
        currentGrid = new bool[gridWidth, gridHeight];
        nextGrid = new bool[gridWidth, gridHeight];
        cellObjects = new Cell[gridWidth, gridHeight];
        
        // random initial state
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                currentGrid[x, y] = Random.value > 0.7f;
            }
        }
    }

    void CreateCellObjects()
    {
        Vector3 startPos = transform.position;
        startPos.x -= (gridWidth * cellSize) / 2f;
        startPos.y -= (gridHeight * cellSize) / 2f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                Vector3 position = startPos + new Vector3(x * cellSize, y * cellSize, 0);
                GameObject cellObj = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                cellObj.name = $"Cell_{x}_{y}";
                
                Cell cell = cellObj.GetComponent<Cell>();
                if (cell == null)
                {
                    cell = cellObj.AddComponent<Cell>();
                }
                
                cellObjects[x, y] = cell;
                cell.SetColors(aliveColor, deadColor);
                cell.SetState(currentGrid[x, y]);
            }
        }
    }

    public void StartSimulation()
    {
        if (!isRunning)
        {
            isRunning = true;
            simulationCoroutine = StartCoroutine(SimulationLoop());
        }
    }

    public void StopSimulation()
    {
        if (isRunning)
        {
            isRunning = false;
            if (simulationCoroutine != null)
            {
                StopCoroutine(simulationCoroutine);
            }
        }
    }

    IEnumerator SimulationLoop()
    {
        while (isRunning)
        {
            UpdateGrid();
            yield return new WaitForSeconds(updateInterval);
        }
    }

    void UpdateGrid()
    {
        // check for alternate rules
        AlternateRules alternateRules = GetComponent<AlternateRules>();
        
        // calculate next generation
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                int neighbors = CountNeighbors(x, y);
                bool isAlive = currentGrid[x, y];
                
                // use alternate rules if available, otherwise use default rules
                if (alternateRules != null && alternateRules.enabled)
                {
                    nextGrid[x, y] = alternateRules.EvaluateCell(isAlive, neighbors);
                }
                else
                {
                    // default rules (B3/S23)
                    if (isAlive)
                    {
                        nextGrid[x, y] = (neighbors == 2 || neighbors == 3);
                    }
                    else
                    {
                        nextGrid[x, y] = (neighbors == 3);
                    }
                }
            }
        }

        // update current grid and visuals
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                currentGrid[x, y] = nextGrid[x, y];
                cellObjects[x, y].SetState(currentGrid[x, y]);
            }
        }
    }

    int CountNeighbors(int x, int y)
    {
        int count = 0;
        
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                if (i == 0 && j == 0) continue;
                
                int nx = x + i;
                int ny = y + j;
                
                // wrap around edges
                if (nx < 0) nx = gridWidth - 1;
                if (nx >= gridWidth) nx = 0;
                if (ny < 0) ny = gridHeight - 1;
                if (ny >= gridHeight) ny = 0;
                
                if (currentGrid[nx, ny])
                {
                    count++;
                }
            }
        }
        
        return count;
    }

    void Update()
    {
        // toggle simulation with Space 
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isRunning)
            {
                StopSimulation();
            }
            else
            {
                StartSimulation();
            }
        }
        
        // step one generation with S key
        if (Input.GetKeyDown(KeyCode.S))
        {
            UpdateGrid();
        }
    }
}
