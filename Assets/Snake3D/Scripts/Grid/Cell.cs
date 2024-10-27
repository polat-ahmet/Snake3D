using System.Collections.Generic;
using JetBrains.Annotations;
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

        public Vector3 itemPlacementPosition
        {
            get
            {
                return transform.localPosition + Vector3.up;
            }
        }
        
        // public List<Cell> Neighbours;

        [CanBeNull] private CellItem item;
        
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
        
        public Cell GetNeighbourWithDirection(Direction direction)
        {
            return grid.GetNeighbourWithDirection(this, direction);
        }

        public void PlaceItem(CellItem item)
        {
            item.transform.localPosition = GetItemPlacementPosition(item);
            SetItem(item);
        }

        public Vector3 GetItemPlacementPosition(CellItem item)
        {
            return itemPlacementPosition + item.itemOffset;
        }
    }