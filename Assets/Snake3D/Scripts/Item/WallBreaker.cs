using Snake3D.Grid;
using UnityEngine;

namespace Snake3D.Item
{
    public class WallBreaker : CellItem
    {
        private void Awake()
        {
            itemOffsetY = new Vector3(0, -.25f, 0);
            type = ItemType.Fruit;
        }

        protected override void Eat(Snake.Snake snake)
        {
            snake.canEatItems.Add(ItemType.Wall);
        }
    }
}