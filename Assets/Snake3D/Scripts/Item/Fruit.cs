using Snake3D.Grid;
using UnityEngine;

namespace Snake3D.Item
{
    public class Fruit : CellItem
    {
        public static event System.Action OnFruitEaten;
        void Awake()
        {
            itemOffsetY = new Vector3(0, -.25f, 0);
            type = ItemType.Fruit;
        }
        
        public override void Eat(Snake.Snake snake)
        {
            Debug.Log("Yummyyy");
            OnFruitEaten?.Invoke();
            
           
        }
    }
}