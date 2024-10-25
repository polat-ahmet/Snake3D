using UnityEngine;

namespace Snake3D.Grid
{
    public class CellItem : MonoBehaviour
    {
        public Cell cell;
        
        public void setCell(Cell cell)
        {
            this.cell = cell;
        }

    }
}