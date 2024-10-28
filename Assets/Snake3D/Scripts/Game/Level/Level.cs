using UnityEngine;

namespace Snake3D.Game
{
    public class Level : MonoBehaviour
    {
        public TextAsset levelConfigJson;
        private Grid.Grid grid;
        private LevelConfig levelConfig;
        
        public Cell[] gridCellPrefab;
        [SerializeField] private Snake.Snake snakePrefab;
        [SerializeField] private GameObject applePrefab;
        [SerializeField] private GameObject wallPrefab;

        private void Start()
        {
            // Load the level configuration from the JSON file
            // LoadLevelConfig();
            // // Set up the Grid based on the level configuration
            // SetupGrid();
            var gridObject = new GameObject("GridObject");

            // 2. Yeni nesneye Grid MonoBehaviour'u ekle
            Grid.Grid gridComponent = gridObject.AddComponent<Grid.Grid>();
            // gridComponent.Init(10, 10, gridCellPrefab, snakePrefab, applePrefab, wallPrefab);
        }

        private void LoadLevelConfig()
        {
            if (levelConfigJson != null)
                levelConfig = JsonUtility.FromJson<LevelConfig>(levelConfigJson.text);
            else
                Debug.LogError("Level config JSON is not assigned.");
        }

        private void SetupGrid()
        {
            if (levelConfig == null)
            {
                Debug.LogError("Level config is not loaded.");
                return;
            }

            // grid = FindObjectOfType<Grid>();

            if (grid == null) Debug.LogError("Grid not found.");

            // Initialize the Grid's size
            // grid.GenerateGrid(levelConfig.rows, levelConfig.columns);

            // Place initial items
            // foreach (var item in levelConfig.initialItems)
            // {
            //     // Cell cell = grid.GetCell(item.positionX, item.positionY);
            //     // Assuming you have a method to create an item based on item type
            //     GameObject itemPrefab = GetItemPrefab(item.itemType);
            //     if (itemPrefab != null && cell != null)
            //     {
            //         grid.createItem(cell, itemPrefab);
            //     }
            // }

            // Center the Grid
            // grid.CenterGrid();
        }

        private GameObject GetItemPrefab(string itemType)
        {
            // This method should return the prefab for the specified itemType
            // Implement this according to your resource loading strategy
            return Resources.Load<GameObject>($"Prefabs/{itemType}");
        }
    }
}