using System.Collections;
using System.Collections.Generic;
using Snake3D.Game;
using Snake3D.Grid;
using Snake3D.Item;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] private float stepSize = 1f;
    // [SerializeField] private float stepSpeed = 1f;

    [SerializeField] private SnakeBodyPart headPrefab;
    [SerializeField] private SnakeBodyPart bodyPrefab;
    [SerializeField] private SnakeBodyPart tailPrefab;

    private bool alive = true;
    
    public List<SnakeBodyPart> snakeBody = new();
    public SnakeBodyPart snakeHead;
    public SnakeBodyPart snakeTail;

    public Direction direction;

    private void Start()
    {
        
    }

    public void Init(Cell headCell, Cell tailCell)
    {
        AddHead();
        AddTail();
        // snakeHead.setCell(headCell);
        headCell.SetItem(snakeHead);
        // Debug.Log("Snake Head Cell:" + snakeHead.cell.X + " , " + snakeHead.cell.Z);
        // snakeTail.setCell(tailCell);
        tailCell.SetItem(snakeTail);
        direction = Direction.Left;
        Debug.Log("direction:" + direction);
        TickSystem.OnTick += delegate(object sender, TickSystem.OnTickEventArgs args)
        {
            Debug.Log("direction:" + direction);
            
            Cell nextCell = snakeHead.cell.GetNeighbourWithDirection(direction);
            
            if (nextCell == null)
            {
                Debug.Log("Dead");
                alive = false;
            }
            if (alive)
            {
                CellItem nextItem = nextCell.GetItem();
                if (nextItem != null) nextItem.TryEat();
                
                Move();

                
                
                // snakeHead.setCell(nextCell);
                nextCell.SetItem(snakeHead);
                // snakeTail.setCell(snakeHead.cell);
                // snakeHead.cell
            }
            
        };
        
    }
    
    void Update()
    {
        // Yön değişiklikleri için input kontrolleri
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Direction.Down)
        {
            direction = Direction.Up;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Direction.Up)
        {
            direction = Direction.Down;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Direction.Right)
        {
            direction = Direction.Left;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Direction.Left)
        {
            direction = Direction.Right;
        }
    }

    public void grow(int amount)
    {
        
    }

    // public void AddBodyPart()
    // {
    //     if (snakeBody.Count == 0)
    //     {
    //         
    //     }
    //     
    //     if (snakeTail == null)
    //     {
    //         snakeTail = Instantiate(tailPrefab,
    //             snakeHead.transform.position - (snakeHead.transform.forward * stepSize).normalized,
    //             snakeHead.transform.rotation);
    //         snakeTail.name = "Tail";
    //         // snakeBody.Add(snakeTail);
    //         snakeTail.transform.parent = transform;
    //     }
    // }

    public void AddTail()
    {
        if (snakeTail == null)
        {
            snakeTail = Instantiate(tailPrefab,
                snakeHead.transform.position - (snakeHead.transform.forward * stepSize).normalized,
                snakeHead.transform.rotation);
            snakeTail.name = "Tail";
            // snakeBody.Add(snakeTail);
            snakeTail.transform.parent = transform;
        }
    }

    public void AddHead()
    {
        if (snakeHead == null)
        {
            snakeHead = Instantiate(headPrefab, transform);
            snakeHead.name = "Head";
            // snakeBody.Add(snakeHead);
            snakeHead.transform.parent = transform;
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
    public Vector3 getRotationVector()
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

    //TODO move snake movement
    public void Move()
    {
        List<IEnumerator> coroutineList = new List<IEnumerator>();
            
        var lastHeadPosition = snakeHead.transform.localPosition;
        Vector3 endPos = lastHeadPosition + getDirectionVector();
            
        var lastHeadRotation = snakeHead.transform.localEulerAngles;

        lastHeadRotation.x = 0;
        lastHeadRotation.z = 0;
            
        coroutineList.Add(snakeHead.Move(endPos, Quaternion.Euler(getRotationVector())));
            
        var lastBodyPosition = lastHeadPosition;
        var lastBodyRotation = Quaternion.Euler(lastHeadRotation);
            
        for (var i = 0; i < snakeBody.Count; i++)
        {
            var currentBodyPosition = snakeBody[i].transform.localPosition;
            var currentBodyRotation = snakeBody[i].transform.rotation;
                
            coroutineList.Add(snakeBody[i].Move(lastBodyPosition, lastBodyRotation));

            lastBodyPosition = currentBodyPosition;
            lastBodyRotation = currentBodyRotation;
        }
        coroutineList.Add(snakeTail.Move(lastBodyPosition, lastBodyRotation));
            
        foreach (IEnumerator coroutine in coroutineList)
        {
            StartCoroutine(coroutine);
        }
    }

    // private void MoveSnakeEachTick()
    // {
    //     var lastHeadPosition = snakeHead.transform.position;
    //     var lastHeadRotation = snakeHead.transform.localEulerAngles;
    //
    //     lastHeadRotation.x = 0;
    //     lastHeadRotation.z = 0;
    //
    //     snakeHead.transform.position += snakeHead.transform.forward * stepSpeed;
    //
    //     // snakeHead.transform.localPosition = Vector3.Lerp(startPos, endPos, travelPercent);
    //
    //     var lastBodyPosition = lastHeadPosition;
    //     var lastBodyRotation = Quaternion.Euler(lastHeadRotation);
    //
    //     if (snakeBody.Count > 0)
    //         for (var i = 0; i < snakeBody.Count; i++)
    //         {
    //             var currentBodyPosition = snakeBody[i].transform.position;
    //             var currentBodyRotation = snakeBody[i].transform.rotation;
    //
    //             snakeBody[i].transform.position = lastBodyPosition;
    //             snakeBody[i].transform.rotation = lastBodyRotation;
    //
    //             lastBodyPosition = currentBodyPosition;
    //             lastBodyRotation = currentBodyRotation;
    //         }
    //
    //     snakeTail.transform.position = lastBodyPosition;
    //     snakeTail.transform.rotation = lastBodyRotation;
    // }

    // private IEnumerator MoveSnake()
    // {
    //     var elapsedTime = 0f;
    //     while (alive)
    //     {
    //         elapsedTime += Time.deltaTime;
    //         if (elapsedTime >= stepSpeed)
    //         {
    //             // Vector3 startPos = snakeHead.transform.localPosition;
    //             // Vector3 endPos = startPos + snakeHead.transform.forward;
    //             //
    //             // float travelPercent = 0f;
    //             // while (travelPercent < 1f)
    //             // {
    //             //     travelPercent += Time.deltaTime;
    //
    //
    //             // transform.localPosition = Vector3.Lerp(startPos, endPos, travelPercent);
    //
    //             var lastHeadPosition = snakeHead.transform.position;
    //             var lastHeadRotation = snakeHead.transform.localEulerAngles;
    //
    //             lastHeadRotation.x = 0;
    //             lastHeadRotation.z = 0;
    //
    //             snakeHead.transform.position += snakeHead.transform.forward * stepSpeed;
    //
    //             // snakeHead.transform.localPosition = Vector3.Lerp(startPos, endPos, travelPercent);
    //
    //             var lastBodyPosition = lastHeadPosition;
    //             var lastBodyRotation = Quaternion.Euler(lastHeadRotation);
    //
    //             if (snakeBody.Count > 0)
    //                 for (var i = 0; i < snakeBody.Count; i++)
    //                 {
    //                     var currentBodyPosition = snakeBody[i].transform.position;
    //                     var currentBodyRotation = snakeBody[i].transform.rotation;
    //
    //                     snakeBody[i].transform.position = lastBodyPosition;
    //                     snakeBody[i].transform.rotation = lastBodyRotation;
    //
    //                     lastBodyPosition = currentBodyPosition;
    //                     lastBodyRotation = currentBodyRotation;
    //                 }
    //
    //             snakeTail.transform.position = lastBodyPosition;
    //             snakeTail.transform.rotation = lastBodyRotation;
    //
    //             elapsedTime = 0;
    //
    //
    //             //     yield return new WaitForEndOfFrame();
    //             // }
    //         }
    //
    //         yield return null;
    //     }
    // }
}