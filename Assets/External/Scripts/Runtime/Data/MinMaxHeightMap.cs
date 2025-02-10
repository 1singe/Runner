using System;
using Unity.Collections;

namespace PGG
{
    public struct MinMaxHeightMap
    {
        public int Resolution;
        public float[,] HeightMap;
        public float Min;
        public float Max;

        public MinMaxHeightMap(int resolution)
        {
            Resolution = resolution;
            HeightMap = new float[Resolution, Resolution];
            Min = 5000f;
            Max = -5000f;
        }
    }
}