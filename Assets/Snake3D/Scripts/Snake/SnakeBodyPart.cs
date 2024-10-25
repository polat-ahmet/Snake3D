using System.Collections;
using System.Collections.Generic;
using Snake3D.Game;
using Snake3D.Grid;
using UnityEngine;

public class SnakeBodyPart : CellItem
{
    // next
    // before
    // prefab

    private Direction direction;

   
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
}
