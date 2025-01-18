using System;
using System.Collections;
using System.Collections.Generic;
using Codice.CM.SEIDInfo;
using UnityEngine;

namespace PGG
{
    public class NodeInfoAttribute : Attribute
    {
        public string NodeTitle { get; private set; }
        public string MenuItem { get; private set; }

        public bool IsOutputNode { get; private set; }

        public Type OutputType { get; private set; }

        public NodeInfoAttribute(string title, string menuItem = "", bool isOutputNode = false, Type outputType = null)
        {
            NodeTitle = title;
            MenuItem = menuItem;
            IsOutputNode = isOutputNode;

            if (outputType == null)
            {
                OutputType = typeof(float);
            }
            else
            {
                OutputType = outputType;
            }
        }
    }
}