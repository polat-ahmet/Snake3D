using System;
using System.Collections.Generic;

namespace Snake3D.Game
{
    [Serializable]
    public class LevelConfig
    {
        public int rows;
        public int columns;
        public List<InitialItem> initialItems;
        public InitialSnake initialSnake;

        [Serializable]
        public class InitialItem
        {
            public string itemType; // Example: "Fruit"
            public int positionX;
            public int positionY;
        }
        
        [Serializable]
        public class InitialSnake
        {
            public int headPositionX;
            public int headPositionY;
            public int tailPositionX;
            public int tailPositionY;
            public string startDirection;
        }
    }
}