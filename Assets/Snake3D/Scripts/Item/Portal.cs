using UnityEngine;
using Snake3D.Grid;

namespace Snake3D.Item
{
    // public class Portal : CellItem
    // {
    //     public Cell inCell;
    //     public Cell outCell;
    //     void Awake()
    //     {
    //         itemOffsetY = new Vector3(0, -0.9f, 0);
    //         type = ItemType.Portal;
    //     }
    //
    //     public void Init(Cell inCell, Cell outCell)
    //     {
    //         this.inCell = inCell;
    //         this.outCell = outCell;
    //         
    //         foreach (var direction in this.cell.neighbors.Keys)
    //         {
    //             this.inCell.neighbors[direction] = this.outCell;
    //             this.outCell.neighbors[direction] = this.inCell;
    //         }
    //     }
    //     
    //     
    //     public override void Eat(Snake.Snake snake)
    //     {
    //         Debug.Log("Portal");
    //     }
    // }
}