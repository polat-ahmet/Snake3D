using Snake3D.Grid;
using UnityEngine;

namespace Snake3D.Item
{
    public abstract class CellItem : MonoBehaviour
    {
        public Cell cell;
        public Vector3 itemOffsetY;
        public ItemType type;

        public void setCell(Cell cell)
        {
            this.cell = cell;
        }

        protected void Init(Cell cell, ItemType type)
        {
            this.cell = cell;
            this.type = type;
        }


        //Template Method
        public void TryEat(Snake.Snake snake)
        {
            if (canEatBySnake(snake))
            {
                Eat(snake);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log($"Can't eaten by snake: {type}");
                snake.StopMoving();
            }
        }

        private bool canEatBySnake(Snake.Snake snake)
        {
            if (snake.canEatItems.Contains(type)) return true;
            return false;
        }

        protected abstract void Eat(Snake.Snake snake);

        public Vector3 GetItemOffset()
        {
            return itemOffsetY;
        }
    }
}