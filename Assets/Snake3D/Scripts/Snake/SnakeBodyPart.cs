using System.Collections;
using JetBrains.Annotations;
using Snake3D.Game;
using Snake3D.Grid;
using Snake3D.Item;
using UnityEngine;

namespace Snake3D.Snake
{
    public class SnakeBodyPart : CellItem
    {
        [CanBeNull] public SnakeBodyPart beforePart;
        [CanBeNull] public SnakeBodyPart afterPart;
    
        public Cell nextCell;
        public Direction direction;
        public Direction nextDirection;

        public bool isReadyToMove = true;
        void Awake()
        {
            itemOffsetY = new Vector3(0, -.25f, 0);
        }
        
        public void Init(Cell cell, Direction direction)
        {
            base.Init(cell, ItemType.SnakeBodyPart);
            this.direction = direction;
            nextCell = cell.GetNeighbourWithDirection(direction);
        }
        public override void Eat(Snake snake)
        {
            Debug.Log("You can't eat your body!");
        }
    
        public IEnumerator MoveToNextCell()
        {
            if (isReadyToMove)
            {
                nextCell = cell.GetNeighbourWithDirection(direction);
        
                cell.RemoveItem();
                nextCell.SetItem(this);
        
                Vector3 startPos = transform.localPosition;
                Vector3 endPos = nextCell.GetItemPlacementPosition(this);
        
                transform.rotation =  Quaternion.Euler(DirectionUtil.GetRotationVector(direction));
        
                nextCell = cell.GetNeighbourWithDirection(direction);
                direction = nextDirection;

                yield return InterpolateSnakeMovement(startPos, endPos);
            }
        }

        private IEnumerator InterpolateSnakeMovement(Vector3 startPos, Vector3 endPos)
        {
            var elapsedTime = 0f;
            while (elapsedTime < TickSystem.tickTimerMax)
            {
           
                elapsedTime += Time.deltaTime;
                
                transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime/TickSystem.tickTimerMax);
            
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
