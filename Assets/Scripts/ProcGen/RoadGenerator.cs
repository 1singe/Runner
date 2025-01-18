using UnityEngine;

struct RoadGeneratorParameter
{
    public Vector2 StartPosition;
    
}
public class RoadGenerator : MonoBehaviour
{
    public Vector2 StartPosition;
    public float MaxStartAngle;

    [Header("Turns")] public AnimationCurve TurnAngleDistributionCurve;
    public float MaxTurnHalfAngle = 90f;
}
