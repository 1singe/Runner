using System;

namespace PGG
{
    public class InputAttribute : Attribute
    {
        public bool MultiInput { get; private set; }

        public Type DisplayType { get; private set; }

        public bool IsBindable { get; private set; }

        public bool IsParameter { get; private set; }

        public string DisplayName { get; private set; }

        public InputAttribute(bool multiInput, Type acceptedType, bool isBindable = true, bool isParameter = true, string displayName = "")
        {
            MultiInput = multiInput;
            DisplayType = acceptedType;
            IsBindable = isBindable;
            IsParameter = isParameter;
            DisplayName = displayName;
        }
    }
}