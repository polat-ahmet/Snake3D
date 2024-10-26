using System.Collections;
using System.Collections.Generic;
using Snake3D.Game;
using Snake3D.Grid;
using UnityEngine;

public class SnakeBodyPart : CellItem
{
    public SnakeBodyPart beforePart;
    public SnakeBodyPart afterPart;
    
    public Cell beforeCell;
    private Direction direction;

    public override void TryEat()
    {
        Debug.Log("You can't eat this body!");
    }

    public IEnumerator Move(Vector3 position, Quaternion rotation )
    {
        var elapsedTime = 0f;
    
        Vector3 startPos = transform.localPosition;
        Vector3 endPos = position;
        
        transform.rotation = rotation;
        
        while (elapsedTime < TickSystem.tickTimerMax)
        {
           
            elapsedTime += Time.deltaTime;
            transform.localPosition = Vector3.Lerp(startPos, endPos, elapsedTime/TickSystem.tickTimerMax);
            
            yield return new WaitForEndOfFrame();
        }
        
    }

    // public IEnumerator MoveToNextCell()
    // {
    //     var elapsedTime = 0f;
    //     
    //     Vector3 startPos = transform.localPosition;
    //     Vector3 endPos = beforeCell.transform.localPosition;
    //     
    // }
}
