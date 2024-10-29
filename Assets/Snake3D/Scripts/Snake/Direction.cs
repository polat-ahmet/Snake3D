using UnityEngine;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public static class DirectionUtil
{
    public static Vector3 GetDirectionVector(Direction direction)
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

    public static Vector3 GetRotationVector(Direction direction)
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

    public static Direction GetReverseDirection(Direction direction)
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