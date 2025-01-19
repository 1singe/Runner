using System.Collections.Generic;
using UnityEngine;

namespace PGG
{
    [NodeInfo("Cellular Noise (Voronoi)", "Sample/Voronoi", false, typeof(PGGPortTypes.NoisePort))]
    public class Voronoi : NoiseBase
    {
        [SerializeField] [Input(false, typeof(FastNoise.CellularDistanceFunction), false, true, "Distance Function")]
        public FastNoise.CellularDistanceFunction DistanceFunction = FastNoise.CellularDistanceFunction.Euclidean;

        [SerializeField] [Input(false, typeof(FastNoise.CellularReturnType), false, true, "Return Type")]
        public FastNoise.CellularReturnType ReturnType = FastNoise.CellularReturnType.CellValue;

        public override void Init()
        {
            base.Init();
            _noise.SetNoiseType(FastNoise.NoiseType.Cellular);
            _noise.SetCellularDistanceFunction(DistanceFunction);
            _noise.SetCellularReturnType(ReturnType);
        }

        public override void BakeInit(ref List<string> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines.Add("noise" + _id + ".SetNoiseType(SFastNoise.NoiseType.Cellular);");
            InitLines.Add("noise" + _id + ".SetCellularDistanceFunction( + " + DistanceFunction + ");");
            InitLines.Add("noise" + _id + ".SetCellularReturnType(" + ReturnType + ");");
        }
    }
}