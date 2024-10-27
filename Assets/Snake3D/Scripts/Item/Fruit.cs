using Snake3D.Grid;
using UnityEngine;

namespace Snake3D.Item
{
    public class Fruit : CellItem
    {
        public static event System.Action OnFruitEaten;
        void Awake()
        {
            itemOffset = new Vector3(0, -.25f, 0);
        }
         
        public override void TryEat()
        {
            Debug.Log("Yummyyy");
            OnFruitEaten?.Invoke();
            
            Destroy(this.gameObject);
        }
    }
}