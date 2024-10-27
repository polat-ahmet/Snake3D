using Snake3D.Game;
using Snake3D.Grid;
using Snake3D.Item;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    public Cell[] gridCellPrefab;
    [SerializeField] private Snake snakePrefab;
    [SerializeField] private GameObject applePrefab;
    
    public Snake snake;

    public Vector3 gridOffset = Vector3.zero;
    public Vector3 itemOffset = Vector3.up;

    public float tickTimer;
    private Cell[,] grid;

    private void Start()
    {
        grid = new Cell[gridWidth, gridHeight];
        
        GenerateGrid();
        CenterGrid();

        TickSystem.Init();
        TickSystem.tickTimerMax = tickTimer;
        
        
        snake = Instantiate(snakePrefab, transform);
        snake.name = "Snake";
        // snakeBody.Add(snakeTail);
        // snake.transform.position += Vector3.forward;
        // snake.snakeHead.setCell(grid[1,0]);
        // snake.snakeTail.setCell(grid[0,0]);
        snake.Init(grid[0,1], grid[0,0], Direction.Left);
        
        
        
        TickSystem.OnTick += delegate(object sender, TickSystem.OnTickEventArgs args)
        {
            // Debug.Log("Tick:" + args.tick);
            // Debug.Log("Tick:" + args.tick);
            // snake.Move();
        };
    }
    
    void OnEnable()
    {
        Fruit.OnFruitEaten += HandleFruitEaten;
    }

    void OnDisable()
    {
        Fruit.OnFruitEaten -= HandleFruitEaten;
    }
    
    private void HandleFruitEaten()
    {
        snake.AddGrowRequest();
        createItemOnRandomCell(applePrefab);
    }

    private void GenerateGrid()
    {
        for (var x = 0; x < gridWidth; x++)
        for (var z = 0; z < gridHeight; z++)
        {
            var cell = Instantiate(gridCellPrefab[(x + z) % 2], new Vector3(x, 0, z), Quaternion.identity);
            cell.name = "Cell [" + x + "," + z + "]";
            cell.transform.parent = transform;

            cell.Init(x, z, this);
            grid[x, z] = cell;
        }
        // createItem(grid[gridWidth-1,gridHeight-1], applePrefab);
        // createItemOnRandomCell(applePrefab);
        createItem(grid[3,3], applePrefab);
    }
    

    public void createItem(Cell cell, GameObject item)
    {
        if (IsCellEmpty(cell))
        {
            GameObject itemObject = Instantiate(item, transform);
            itemObject.name = "Apple";

            cell.PlaceItem(itemObject.GetComponent<CellItem>());

        }
    }
    
    public void createItemOnRandomCell(GameObject item)
    {
        if (IsThereAnyEmptyCell())
        {
            Cell cell = getRandomEmptyCell();
            createItem(cell, item);
        }
        else
        {
            Debug.Log("There's no empty cell");
        }
   
    }

    private Cell getRandomEmptyCell()
    {
        Cell randomCell = grid[Random.Range(0, gridWidth), Random.Range(0, gridHeight)];
        while (!IsCellEmpty(randomCell))
        {
            randomCell = grid[Random.Range(0, gridWidth), Random.Range(0, gridHeight)];
        }
        return randomCell;
    }

    private bool IsCellEmpty(Cell cell)
    {
        return cell.GetItem() == null;
    }
    private bool IsThereAnyEmptyCell()
    {
        for (var x = 0; x < gridWidth; x++)
        for (var z = 0; z < gridHeight; z++)
        {
            if (IsCellEmpty(grid[x, z]))
            {
                return true;
            }
        }
        return false;
    }
    

    private void CenterGrid()
    {
        // Calculate the center based on grid size
        var centerPosition = new Vector3((gridWidth - 1) / 2.0f, 0, (gridHeight - 1) / 2.0f);
        transform.position = -centerPosition + gridOffset; // Move the grid so it's centered
    }
    
    public Cell GetNeighbourWithDirection(Cell cell, Direction direction)
    {
        var x = cell.X;
        var z = cell.Z;
        switch (direction)
        {
            case Direction.Up:
                x += 1;
                break;
       
            case Direction.Right:
                z -= 1;
                break;
      
            case Direction.Down:
                x -= 1;
                break;
         
            case Direction.Left:
                z += 1;
                break;
            
        }

        if (x >= gridWidth || x < 0 || z >= gridHeight || z < 0) return null;

        return grid[x, z];
    }
}