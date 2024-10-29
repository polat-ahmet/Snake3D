using System;

namespace Snake3D.Grid
{
    [Serializable]
    public class CellData
    {
        public int X;
        public int Z;

        public CellData(int x, int z)
        {
            X = x;
            Z = z;
        }
    }
}