using Snake3D.Grid;
using UnityEngine;

namespace Snake3D.Item
{
    public class Wall : CellItem
    {
        private void Awake()
        {
            itemOffsetY = new Vector3(0, 0, 0);
            type = ItemType.Wall;
        }

        protected override void Eat(Snake.Snake snake)
        {
            Debug.Log("Wall");
        }
    }
}