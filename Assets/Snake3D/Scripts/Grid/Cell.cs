using System.Collections.Generic;
using Snake3D.Grid;
using Unity.VisualScripting;
using UnityEngine;
    public class Cell : MonoBehaviour
    {
        [SerializeField] private int x;
        [SerializeField] private int z;

        public Grid grid;
        
        public int X { get => x; }
        public int Z { get => z; }
        
        public List<Cell> Neighbours;

        private CellItem item;
        
        // public Transform itemTransform;
        
        public void Init(int x, int z, Grid grid)
        {
            this.x = x;
            this.z = z;
            item = null;
            this.grid = grid;
            // itemTransform = transform;
            // itemTransform.position += Vector3.up;
        }

        public void SetItem(CellItem item)
        {
            this.item = item;
        }

        public CellItem GetItem()
        {
            return item;
        }
        
        public Cell GetNeighbourWithDirection(Direction direction)
        {
            return grid.GetNeighbourWithDirection(this, direction);
        }
    }