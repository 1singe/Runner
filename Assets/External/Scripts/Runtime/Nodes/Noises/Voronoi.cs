using System.Collections.Generic;
using System.Globalization;
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

        public override float ProcessSelf(float x, float y)
        {
            return _noise.GetCellular(x, y);
        }

        public override string BakeProcess(string Input)
        {
            return "Noise" + _id + ".GetCellular(x + " + Offset.x.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "f, y + " + Offset.y.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "f) * " + Amplitude.ToString(CultureInfo.InvariantCulture).Replace(',', '.') + "f";
        }

        public override void BakeInit(ref Dictionary<string, List<string>> InitLines)
        {
            base.BakeInit(ref InitLines);
            InitLines[_id].Add("noise" + _id + ".SetNoiseType(FastNoise.NoiseType.Cellular);");
            InitLines[_id].Add("noise" + _id + ".SetCellularDistanceFunction(FastNoise.CellularDistanceFunction." + DistanceFunction + ");");
            InitLines[_id].Add("noise" + _id + ".SetCellularReturnType(FastNoise.CellularReturnType." + ReturnType + ");");
        }
    }
}