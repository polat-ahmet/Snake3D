using UnityEngine;

namespace Snake3D.Grid
{
    public abstract class CellItem : MonoBehaviour
    {
        public Cell cell;
        public Vector3 itemOffset;
        
        public void setCell(Cell cell)
        {
            this.cell = cell;
        }

        public abstract void TryEat();
    }
}