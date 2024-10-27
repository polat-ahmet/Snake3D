using System.Collections;
using System.Collections.Generic;
using Snake3D.Game;
using Snake3D.Grid;
using Snake3D.Item;
using Snake3D.Snake;
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

    public Direction startDirection;
    
    public bool lockInput = false;
    
    private Queue<System.Action> growRequests = new Queue<System.Action>();
    private Queue<System.Action> addBodyRequests = new Queue<System.Action>();
    
    private SnakeBodyPartModel createSnakeBodyPartModel = new SnakeBodyPartModel();

    private void Start()
    {
        
    }
    
    public void AddGrowRequest()
    {
        growRequests.Enqueue(() => grow());
    }
    
    public void AddBodyRequest()
    {
        addBodyRequests.Enqueue(() => AddBodyPart());
    }

    public void Init(Cell headCell, Cell tailCell, Direction startDirection)
    {
        AddHead(headCell, startDirection);
        AddTail(tailCell, startDirection);
        
        // snakeHead.direction = startDirection;
        // snakeHead.nextCell = snakeHead.cell.GetNeighbourWithDirection(snakeHead.direction);
        
        TickSystem.OnTick += delegate(object sender, TickSystem.OnTickEventArgs args)
        {
            //TODO move to game manager
            ManuelUpdate();
        };
        
    }

    void ManuelUpdate()
    {
        snakeHead.nextCell = snakeHead.cell.GetNeighbourWithDirection(snakeHead.direction);
            
        if (snakeHead.nextCell == null)
        {
            Debug.Log("Dead");
            alive = false;
        }
        if (alive)
        {
            lockInput = true;
            
            CellItem nextItem = snakeHead.nextCell.GetItem();
            if (nextItem != null) nextItem.TryEat();
                
            // snakeHead.
            if (growRequests.Count > 0)
            {
                var growRequest = growRequests.Dequeue();
                growRequest.Invoke();
            }   
            // Move();
            List<IEnumerator> coroutineList = new List<IEnumerator>();

            SnakeBodyPart bodyPart = snakeHead;
            while (bodyPart != null)
            {
                if (bodyPart.beforePart == null)
                {
                    bodyPart.nextDirection = bodyPart.direction;
                }
                else
                {
                    bodyPart.nextDirection = bodyPart.beforePart.direction;
                }
               
                coroutineList.Add(bodyPart.MoveToNextCell());
                    
                bodyPart = bodyPart.afterPart;
            }
            foreach (IEnumerator coroutine in coroutineList)
            {
                StartCoroutine(coroutine);
            }
            
            if (addBodyRequests.Count > 0)
            {
                var request = addBodyRequests.Dequeue();
                request.Invoke();
            }
            
            lockInput = false;
            
        }
    }
    
    void Update()
    {
        if (!lockInput)
        {
            
            if (Input.GetKeyDown(KeyCode.UpArrow) && snakeHead.direction != Direction.Down)
            {
                snakeHead.direction = Direction.Up;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow) && snakeHead.direction != Direction.Up)
            {
                snakeHead.direction = Direction.Down;
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && snakeHead.direction != Direction.Right)
            {
                snakeHead.direction = Direction.Left;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && snakeHead.direction != Direction.Left)
            {
                snakeHead.direction = Direction.Right;
            }
        }
        
    }

    public void grow()
    {
        snakeTail.isReadyToMove = false;
        SnakeBodyPart lastBodyPart = snakeTail.beforePart;
        Cell cell = lastBodyPart.cell;
        Direction direction = lastBodyPart.direction;
        
        createSnakeBodyPartModel.Init(cell, direction);
        AddBodyRequest();
        // AddBodyPart();
    }

    public void AddBodyPartNextTo(SnakeBodyPart beforePart, SnakeBodyPart afterPart)
    {
        beforePart.afterPart = afterPart;
        afterPart.beforePart = beforePart;
    }

    public void AddBodyPart()
    {
        // if (snakeTail == null)
        // {
        //     Cell cell = snakeHead.cell.GetNeighbourWithDirection(snakeHead.GetReverseDirection());
        //     snakeTail = CreateTailPart(cell, snakeHead.direction);
        //     AddBodyPartNextTo(snakeHead, snakeTail);
        // }
        // else
        // {
            // SnakeBodyPart lastBodyPart = snakeTail.beforePart;
            // Cell cell = lastBodyPart.cell;
            // // lastBodyPart.afterPart = null;
            // Direction direction = lastBodyPart.direction;
            // SnakeBodyPart body = CreateBodyPart(cell, direction);
            // AddBodyPartNextTo(lastBodyPart, body);
            // AddBodyPartNextTo(body, snakeTail);
            // snakeTail.isReadyToMove = false;
            // body.isReadyToMove = false;
            //
            // Debug.Log("Start adding to snake body");
            //
            // yield return WaitForNextTick();
            // Debug.Log("Added");
            // snakeTail.isReadyToMove = true;
            // body.isReadyToMove = true;

            SnakeBodyPart lastBodyPart = snakeTail.beforePart;
            
            Cell cell = createSnakeBodyPartModel.cell;
            Direction direction = createSnakeBodyPartModel.direction;
            
            SnakeBodyPart body = CreateBodyPart(cell, direction);
            AddBodyPartNextTo(lastBodyPart, body);
            AddBodyPartNextTo(body, snakeTail);
            snakeTail.isReadyToMove = true;
            

        // }
    }
    
    private IEnumerator WaitForNextTick()
    {
        bool tickOccurred = false;

        void OnTickHandler(object sender, TickSystem.OnTickEventArgs e)
        {
            Debug.Log("TickOccurred");
            tickOccurred = true;
        }

        TickSystem.OnTick += OnTickHandler;

        // Wait until the tickOccurred is true
        yield return new WaitUntil(() => tickOccurred);

        TickSystem.OnTick -= OnTickHandler;
    }


    public void AddTail(Cell tailCell, Direction direction)
    {
        snakeTail = CreateTailPart(tailCell, direction);
        AddBodyPartNextTo(snakeHead, snakeTail);
    }

    public SnakeBodyPart CreateTailPart(Cell cell, Direction direction)
    {
            SnakeBodyPart body = Instantiate(tailPrefab, transform);
                // Instantiate(tailPrefab,
                // snakeHead.transform.position - (snakeHead.transform.forward * stepSize).normalized,
                // snakeHead.transform.rotation);
            body.name = "Tail";
            // snakeBody.Add(snakeTail);
            body.transform.parent = transform;
            body.transform.localPosition = cell.GetItemPlacementPosition(body);
            body.Init(cell, direction);
            return body;
    }
    public SnakeBodyPart CreateBodyPart(Cell cell, Direction direction)
    {
        SnakeBodyPart body = Instantiate(bodyPrefab, transform);
        // Instantiate(tailPrefab,
        // snakeHead.transform.position - (snakeHead.transform.forward * stepSize).normalized,
        // snakeHead.transform.rotation);
        body.name = "Body";
        // snakeBody.Add(snakeTail);
        body.transform.parent = transform;
        body.transform.localPosition = cell.GetItemPlacementPosition(body);
        body.Init(cell, direction);
        return body;
    }

    public void AddHead(Cell headCell, Direction direction)
    {
        if (snakeHead == null)
        {
            //TODO remove object pool
            snakeHead = Instantiate(headPrefab, transform);
            snakeHead.name = "Head";
            
            snakeHead.transform.parent = transform;
            
            snakeHead.transform.localPosition = headCell.GetItemPlacementPosition(snakeHead);
            snakeHead.Init(headCell, direction);
        }
    }

    

    //TODO move snake movement
    // public void Move()
    // {
    //     List<IEnumerator> coroutineList = new List<IEnumerator>();
    //         
    //     var lastHeadPosition = snakeHead.transform.localPosition;
    //     Vector3 endPos = lastHeadPosition + getDirectionVector();
    //     
    //     var lastHeadRotation = snakeHead.transform.localEulerAngles;
    //
    //     lastHeadRotation.x = 0;
    //     lastHeadRotation.z = 0;
    //         
    //     coroutineList.Add(snakeHead.Move(endPos, Quaternion.Euler(getRotationVector())));
    //         
    //     // var lastBodyPosition = lastHeadPosition;
    //     // var lastBodyRotation = Quaternion.Euler(lastHeadRotation);
    //     //     
    //     // for (var i = 0; i < snakeBody.Count; i++)
    //     // {
    //     //     var currentBodyPosition = snakeBody[i].transform.localPosition;
    //     //     var currentBodyRotation = snakeBody[i].transform.rotation;
    //     //         
    //     //     coroutineList.Add(snakeBody[i].Move(lastBodyPosition, lastBodyRotation));
    //     //
    //     //     lastBodyPosition = currentBodyPosition;
    //     //     lastBodyRotation = currentBodyRotation;
    //     // }
    //     // coroutineList.Add(snakeTail.Move(lastBodyPosition, lastBodyRotation));
    //         
    //     foreach (IEnumerator coroutine in coroutineList)
    //     {
    //         StartCoroutine(coroutine);
    //     }
    // }

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