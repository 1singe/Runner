using System;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace PGG.Editor
{
    public class PGGPortPropertyContainer : VisualElement
    {
        public Port Port;
        public PropertyField PropertyField;

        public PGGPortPropertyContainer()
        {
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            style.alignSelf = new StyleEnum<Align>(Align.Stretch);
            style.alignItems = new StyleEnum<Align>(Align.Stretch);
            style.flexBasis = new StyleLength(Length.Auto());
            style.flexWrap = new StyleEnum<Wrap>(Wrap.NoWrap);
            style.alignContent = new StyleEnum<Align>(Align.Stretch);
        }
    }
}