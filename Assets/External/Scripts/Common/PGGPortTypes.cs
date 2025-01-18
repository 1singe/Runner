using System;
using System.Collections.Generic;
using UnityEngine;

public class PGGPortTypes
{
    public static readonly Dictionary<Type, Color> Colors = new Dictionary<Type, Color>
    {
        { typeof(float), Color.blue },
        { typeof(Vector2), Color.green },
        { typeof(NoisePort), Color.red },
        { typeof(int), Color.yellow },
    };

    public class BasePort
    {
        public virtual Type InterpretAsType { get; protected set; }
    }

    public class NoisePort : BasePort
    {
        public override Type InterpretAsType { get; protected set; } = typeof(float);
    }
}