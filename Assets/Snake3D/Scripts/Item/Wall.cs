using Snake3D.Grid;
using UnityEngine;

namespace Snake3D.Item
{
    public class Wall : CellItem
    {
        void Awake()
        {
            itemOffsetY = new Vector3(0, 0, 0);
            type = ItemType.Wall;
        }
        
        public override void Eat(Snake.Snake snake)
        {
            Debug.Log("Wall");
        }
    }
}