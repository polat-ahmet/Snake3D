using System.Collections;
using System.Collections.Generic;
using Snake3D.Game;
using Snake3D.Grid;
using Snake3D.Item;
using UnityEngine;

namespace Snake3D.Snake
{
    public class Snake : MonoBehaviour
    {
        [SerializeField] private float stepSize = 1f;
        // [SerializeField] private float stepSpeed = 1f;

        [SerializeField] private SnakeBodyPart headPrefab;
        [SerializeField] private SnakeBodyPart bodyPrefab;
        [SerializeField] private SnakeBodyPart tailPrefab;

        public bool alive = true;
    
        public List<SnakeBodyPart> snakeBody = new();
        public SnakeBodyPart snakeHead;
        public SnakeBodyPart snakeTail;

        public Direction startDirection;
    
        public bool lockInput = false;
    
        private Queue<System.Action> growRequests = new Queue<System.Action>();
        private Queue<System.Action> addBodyRequests = new Queue<System.Action>();
    
        private SnakeBodyPartModel createSnakeBodyPartModel = new SnakeBodyPartModel();
    
        public HashSet<ItemType> canEatItems = new HashSet<ItemType>();
        
        public static event System.Action OnDead;
    
        public void AddGrowRequest()
        {
            growRequests.Enqueue(() => Grow());
        }
    
        public void AddBodyRequest()
        {
            addBodyRequests.Enqueue(() => AddBodyPart());
        }

        public void Init(Cell headCell, Cell tailCell, Direction startDirection)
        {
            canEatItems.Add(ItemType.Fruit);
            
            AddHead(headCell, startDirection);
            AddTail(tailCell, startDirection);
        
            TickSystem.OnTick += delegate(object sender, TickSystem.OnTickEventArgs args)
            {
                //TODO move to game manager
                ManuelUpdate();
            };
        
        }

        
    
        void ManuelUpdate()
        {
            LockInput();
        
            snakeHead.nextCell = snakeHead.cell.GetNeighbourWithDirection(snakeHead.direction);
            
            if (snakeHead.nextCell == null)
            {
                StopMoving();
            }
            else
            {
                CellItem nextItem = snakeHead.nextCell.GetItem();
        
                if (nextItem != null)
                {
                    nextItem.TryEat(this);
                }
            }
            
            
            if (alive)
            {
                // snakeHead.
                ProcessGrowRequest();   
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
            
                ProcessAddBodyRequest();
            
                UnlockInput();
            
            }
        }

        private void UnlockInput()
        {
            lockInput = false;
        }

        private void LockInput()
        {
            lockInput = true;
        }

        private void ProcessGrowRequest()
        {
            if (growRequests.Count > 0)
            {
                var growRequest = growRequests.Dequeue();
                growRequest.Invoke();
            }
        }

        private void ProcessAddBodyRequest()
        {
            if (addBodyRequests.Count > 0)
            {
                var request = addBodyRequests.Dequeue();
                request.Invoke();
            }
        }
        
        public void StopMoving()
        {
            if (alive)
            {
                alive = false;
                OnDead?.Invoke();
            }
        }
    
        public void StartMoving()
        {
            alive = true;
        }

        public void ChangeDirection(Direction newDirection)
        {
            if (!lockInput)
            {
                if (newDirection == Direction.Up && snakeHead.direction != Direction.Down)
                {
                    snakeHead.direction = Direction.Up;
                }
                else if (newDirection == Direction.Down && snakeHead.direction != Direction.Up)
                {
                    snakeHead.direction = Direction.Down;
                }
                else if (newDirection == Direction.Left && snakeHead.direction != Direction.Right)
                {
                    snakeHead.direction = Direction.Left;
                }
                else if (newDirection == Direction.Right && snakeHead.direction != Direction.Left)
                {
                    snakeHead.direction = Direction.Right;
                }

                LockInput();
            }
        }

        private void Grow()
        {
            snakeTail.isReadyToMove = false;
            SnakeBodyPart lastBodyPart = snakeTail.beforePart;
            Cell cell = lastBodyPart.cell;
            Direction direction = lastBodyPart.direction;
        
            createSnakeBodyPartModel.Init(cell, direction);
            AddBodyRequest();
        }

        private void AddBodyPartNextTo(SnakeBodyPart beforePart, SnakeBodyPart afterPart)
        {
            beforePart.afterPart = afterPart;
            afterPart.beforePart = beforePart;
        }

        private void AddBodyPart()
        {
            SnakeBodyPart lastBodyPart = snakeTail.beforePart;
            
            Cell cell = createSnakeBodyPartModel.cell;
            Direction direction = createSnakeBodyPartModel.direction;
            
            SnakeBodyPart body = CreateBodyPart(cell, direction, bodyPrefab, "Body");
            AddBodyPartNextTo(lastBodyPart, body);
            AddBodyPartNextTo(body, snakeTail);
            snakeTail.isReadyToMove = true;
            
        }

        private void AddTail(Cell tailCell, Direction direction)
        {
            snakeTail = CreateBodyPart(tailCell, direction, tailPrefab, "Tail");
            AddBodyPartNextTo(snakeHead, snakeTail);
        }
        
        private void AddHead(Cell headCell, Direction direction)
        {
            if (snakeHead == null)
            {
                snakeHead = CreateBodyPart(headCell, direction, headPrefab, "Head");
            }
        }

        private SnakeBodyPart CreateBodyPart(Cell cell, Direction direction, SnakeBodyPart prefab, string name)
        {
            SnakeBodyPart body = Instantiate(prefab, transform);
            body.name = name;
            body.transform.parent = transform;
            body.transform.localPosition = cell.GetItemPlacementPosition(body);
            body.Init(cell, direction);
            return body;
        }

        
    
    }
}