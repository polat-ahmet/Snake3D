using System;
using Snake3D.Grid;
using UnityEngine;

namespace Snake3D.Item
{
    public class PoisonFruit : Fruit
    {
        void Awake()
        {
            itemOffsetY = new Vector3(0, -.25f, 0);
            type = ItemType.Fruit;
            amount = -1;
        }
    }
}