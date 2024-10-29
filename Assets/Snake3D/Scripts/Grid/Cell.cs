using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Snake3D.Item;
using UnityEngine;

namespace Snake3D.Grid
{
    public class Cell : MonoBehaviour
    {
        [SerializeField] private int x;
        [SerializeField] private int z;

        public Grid grid;

        [CanBeNull] private CellItem item;

        private readonly Dictionary<Direction, Cell> neighbors = new();

        public int X => x;
        public int Z => z;

        private Vector3 itemPlacementPosition => transform.localPosition + Vector3.up;

        public void Init(int x, int z, Grid grid)
        {
            this.x = x;
            this.z = z;
            item = null;
            this.grid = grid;
            UpdateNeighbors();
        }

        public void SetItem(CellItem tempItem)
        {
            item = tempItem;
            item?.setCell(this);
        }

        public CellItem GetItem()
        {
            return item;
        }

        public void RemoveItem()
        {
            item?.setCell(null);
            item = null;
        }

        private void UpdateNeighbors()
        {
            // Debug.Log("UpdateNeighbors");
            foreach (Direction direction in Enum.GetValues(typeof(Direction)))
            {
                var cell = grid.GetNeighbourWithDirection(this, direction);
                neighbors[direction] = cell;
            }
        }

        public Cell GetNeighbourWithDirection(Direction direction)
        {
            return neighbors[direction];
        }

        public void PlaceItem(CellItem tempItem)
        {
            tempItem.transform.localPosition = GetItemPlacementPosition(tempItem);
            SetItem(tempItem);
        }

        public Vector3 GetItemPlacementPosition(CellItem tempItem)
        {
            return itemPlacementPosition + tempItem.GetItemOffset();
        }
    }
}