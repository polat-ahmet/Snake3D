using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Snake3D.Game;
using Snake3D.Grid;
using UnityEngine;

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
        itemOffset = new Vector3(0, -.25f, 0);
    }

    public void Init(Cell cell, Direction direction)
    {
        cell.SetItem(this);
        this.direction = direction;
        nextCell = cell.GetNeighbourWithDirection(direction);
    }
    public override void TryEat()
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
        
            transform.rotation =  Quaternion.Euler(getRotationVector(direction));
        
            nextCell = cell.GetNeighbourWithDirection(direction);
            
            direction = nextDirection;

            var elapsedTime = 0f;
            while (elapsedTime < TickSystem.tickTimerMax)
            {
           
                elapsedTime += Time.deltaTime;
                
                transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime/TickSystem.tickTimerMax);
            
                yield return new WaitForEndOfFrame();
            }
            
        }
    }
    
    //TODO move snake movement
    public Vector3 getDirectionVector()
    {
        switch (direction)
        {
            case Direction.Left:
                return Vector3.forward;
            case Direction.Right:
                return Vector3.back;
            case Direction.Up:
                return Vector3.right;
            case Direction.Down:
                return Vector3.left;
            default:
                return Vector3.zero;
        }
    }
    
    //TODO move snake movement
    public Vector3 getRotationVector(Direction direction)
    {
        switch (direction)
        {
            case Direction.Left:
                return new Vector3(0, 0, 0);
            case Direction.Right:
                return new Vector3(0, 180, 0);
            case Direction.Up:
                return new Vector3(0, 90, 0);
            case Direction.Down:
                return new Vector3(0, 270, 0);
            default:
                return Vector3.zero;
        }
    }
    public Direction GetReverseDirection()
    {
        switch (direction)
        {
            case Direction.Left:
                return Direction.Right;
            case Direction.Right:
                return Direction.Left;
            case Direction.Up:
                return Direction.Down;
            case Direction.Down:
                return Direction.Up;
            default:
                return Direction.Left;
        }
    }
}
