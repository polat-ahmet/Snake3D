using JetBrains.Annotations;

namespace Snake3D.Snake
{
    public class SnakeBodyPartModel
    {
        public Cell nextCell;
        public Cell cell;
        public Direction direction;
        public Direction nextDirection;
        
        public void Init(Cell cell, Direction direction)
        {
            this.cell = cell;
            this.direction = direction;
            nextCell = cell.GetNeighbourWithDirection(direction);
        }
    }
}