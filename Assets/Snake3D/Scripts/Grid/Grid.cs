using System.Collections.Generic;
using Snake3D.Game;
using Snake3D.Item;
using Snake3D.Sound;
using TMPro;
using UnityEngine;

namespace Snake3D.Grid
{
    public class Grid : MonoBehaviour
    {
        public int gridWidth;
        public int gridHeight;
        public Cell[] gridCellPrefab;

        [SerializeField] private Snake.Snake snakePrefab;
        [SerializeField] private CellItem applePrefab;
        [SerializeField] private CellItem poisonPrefab;
        [SerializeField] private CellItem bigFruitPrefab;
        [SerializeField] private CellItem wallPrefab;
        [SerializeField] private CellItem wallBreakerPrefab;

        [Header("Wall Cells")] public List<CellData> wallCells = new();

        [Header("Snake Cells")] public CellData snakeHeadStartPosition = new(0, 1);

        public CellData snakeTailStartPosition = new(0, 0);
        public Direction snakeStartDirection = Direction.Left;

        public float tickTimer;

        [SerializeField] public int goalFruit = 5;
        public int collectedFruit;
        [SerializeField] private TMP_Text goalText;

        [SerializeField] private GameObject levelDialog;
        [SerializeField] private TMP_Text levelDialogTitle;

        [SerializeField] private SoundManager soundManager;
        [SerializeField] public AudioClip eatClip;
        [SerializeField] public AudioClip deadClip;
        private Cell[,] grid;

        private Snake.Snake snake;

        private void Start()
        {
            Init();
        }


        private void OnEnable()
        {
            Fruit.OnFruitEaten += delegate(object sender, Fruit.OnFruitEatenArgs args)
            {
                HandleFruitEaten(args.amount, args.fruit);
            };

            Snake.Snake.OnDead += HandleSnakeDead;
        }

        private void OnDisable()
        {
            Snake.Snake.OnDead -= HandleSnakeDead;
        }

        public void Init()
        {
            DestroyAllChildGameObjects();


            levelDialog.SetActive(false);
            levelDialogTitle.text = "You Lose!";


            grid = new Cell[gridWidth, gridHeight];

            GenerateGrid();
            CenterGrid();

            TickSystem.Init();
            TickSystem.tickTimerMax = tickTimer;

            collectedFruit = 0;
            UpdateGoalText();

            InitializeSnake();

            CreateWallItem();

            createItemOnRandomCell(applePrefab);

            createItemOnRandomCell(poisonPrefab);

            createItemOnRandomCell(wallBreakerPrefab);
        }

        private void DestroyAllChildGameObjects()
        {
            foreach (Transform child in transform) Destroy(child.gameObject);
        }

        private void CreateWallItem()
        {
            foreach (var cell in wallCells) CreateItem(grid[cell.X, cell.Z], wallPrefab);
        }

        private void InitializeSnake()
        {
            snake = Instantiate(snakePrefab, transform);
            snake.name = "Snake";

            snake.Init(grid[snakeHeadStartPosition.X, snakeHeadStartPosition.Z],
                grid[snakeTailStartPosition.X, snakeTailStartPosition.Z], snakeStartDirection);
        }

        public void OnUpButtonPressed()
        {
            snake.ChangeDirection(Direction.Up);
        }

        public void OnDownButtonPressed()
        {
            snake.ChangeDirection(Direction.Down);
        }

        public void OnLeftButtonPressed()
        {
            snake.ChangeDirection(Direction.Left);
        }

        public void OnRightButtonPressed()
        {
            snake.ChangeDirection(Direction.Right);
        }

        private void HandleFruitEaten(int amount, CellItem fruit)
        {
            soundManager.PlaySound(eatClip);
            collectedFruit += amount;

            if (collectedFruit < 0)
            {
                snake.StopMoving();
                collectedFruit = 0;
            }

            UpdateGoalText();

            if (collectedFruit >= goalFruit)
            {
                Debug.Log("You Win!");
                levelDialogTitle.text = "You Win!";
                snake.StopMoving();
            }
            else
            {
                snake.AddGrowRequest();
                createItemOnRandomCell(fruit);
            }
        }

        private void HandleSnakeDead()
        {
            soundManager.PlaySound(deadClip);
            levelDialog.gameObject.SetActive(true);
        }

        private void UpdateGoalText()
        {
            goalText.text = collectedFruit + "/" + goalFruit;
        }

        private void GenerateGrid()
        {
            var cells = new GameObject("CellsParent");
            cells.transform.parent = transform;

            for (var x = 0; x < gridWidth; x++)
            for (var z = 0; z < gridHeight; z++)
            {
                var cell = Instantiate(gridCellPrefab[(x + z) % 2], new Vector3(x, 0, z), Quaternion.identity);
                cell.name = "Cell [" + x + "," + z + "]";
                cell.transform.parent = cells.transform;
                
                grid[x, z] = cell;
            }

            for (var x = 0; x < gridWidth; x++)
            for (var z = 0; z < gridHeight; z++)
                grid[x, z].Init(x, z, this);
            cells.transform.localPosition = Vector3.zero;
        }


        private void CreateItem(Cell cell, CellItem item)
        {
            if (IsCellEmpty(cell))
            {
                var itemObject = Instantiate(item, transform);
                itemObject.name = "Apple";

                cell.PlaceItem(itemObject);
            }
        }

        private void createItemOnRandomCell(CellItem item)
        {
            if (IsThereAnyEmptyCell())
            {
                var cell = GetRandomEmptyCell();
                CreateItem(cell, item);
            }
            else
            {
                Debug.Log("There's no empty cell");
            }
        }

        private Cell GetRandomEmptyCell()
        {
            var randomCell = grid[Random.Range(0, gridWidth), Random.Range(0, gridHeight)];
            while (!IsCellEmpty(randomCell)) randomCell = grid[Random.Range(0, gridWidth), Random.Range(0, gridHeight)];
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
                if (IsCellEmpty(grid[x, z]))
                    return true;
            return false;
        }


        private void CenterGrid()
        {
            // Calculate the center based on grid size
            var centerPosition = new Vector3((gridWidth - 1) / 2.0f, 0, (gridHeight - 1) / 2.0f);
            transform.position = -centerPosition; // Move the grid so it's centered
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
}