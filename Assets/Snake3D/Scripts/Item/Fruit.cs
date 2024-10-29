using System;
using Snake3D.Grid;
using UnityEngine;

namespace Snake3D.Item
{
    public class Fruit : CellItem
    {
        public class OnFruitEatenArgs : EventArgs
        {
            public int amount;
            public CellItem fruit;
        }
        
        public static event EventHandler<OnFruitEatenArgs> OnFruitEaten;
        
        public int amount;
        void Awake()
        {
            itemOffsetY = new Vector3(0, -.25f, 0);
            type = ItemType.Fruit;
            amount = 1;
        }
        
        public override void Eat(Snake.Snake snake)
        {
            Debug.Log("Yummyyy");
            if(OnFruitEaten != null) OnFruitEaten(this, new OnFruitEatenArgs { amount = this.amount, fruit = this });
        }
    }
}