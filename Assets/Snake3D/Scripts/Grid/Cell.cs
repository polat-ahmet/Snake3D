using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Snake3D.Grid;
using Unity.VisualScripting;
using UnityEngine;
using Grid = Snake3D.Grid.Grid;

public class Cell : MonoBehaviour
    {
        [SerializeField] private int x;
        [SerializeField] private int z;

        public CellType type;
        
        public Grid grid;
        
        public int X { get => x; }
        public int Z { get => z; }
        
        public Dictionary<Direction, Cell> neighbors = new Dictionary<Direction, Cell>();

        public Vector3 itemPlacementPosition
        {
            get
            {
                return transform.localPosition + Vector3.up;
            }
        }

        [CanBeNull] private CellItem item;
        
        public void Init(int x, int z, Grid grid)
        {
            type = CellType.Normal;
            this.x = x;
            this.z = z;
            item = null;
            this.grid = grid;
            UpdateNeighbors();
        }
        
        public void SetItem(CellItem item)
        {
            this.item = item;
            this.item?.setCell(this);
        }

        public CellItem GetItem()
        {
            return item;
        }

        public void RemoveItem()
        {
            this.item?.setCell(null);
            item = null;
        }

        public void UpdateNeighbors()
        {
            // Debug.Log("UpdateNeighbors");
            foreach(Direction direction in Enum.GetValues(typeof(Direction)))
            {
                Cell cell = grid.GetNeighbourWithDirection(this, direction);
                neighbors[direction] = cell;
            }
        }
        public Cell GetNeighbourWithDirection(Direction direction)
        {
            return neighbors[direction];
        }

        public void PlaceItem(CellItem item)
        {
            item.transform.localPosition = GetItemPlacementPosition(item);
            SetItem(item);
        }

        public Vector3 GetItemPlacementPosition(CellItem item)
        {
            return itemPlacementPosition + item.GetItemOffset();
        }
    }