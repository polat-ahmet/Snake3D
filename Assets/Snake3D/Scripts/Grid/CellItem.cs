using Snake3D.Item;
using UnityEngine;
using Snake3D.Snake;

namespace Snake3D.Grid
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
        
        public void Init(Cell cell, ItemType type)
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
            }
            else
            {
                Debug.Log($"Can't eaten by snake: {type}");
                snake.alive = false;
            }
        }

        public bool canEatBySnake(Snake.Snake snake)
        {
            if (snake.canEatItems.Contains(type)) return true;
            return false;
        }
        
        public abstract void Eat(Snake.Snake snake);

        public Vector3 GetItemOffset()
        {
            return this.itemOffsetY;
        }
    }
}