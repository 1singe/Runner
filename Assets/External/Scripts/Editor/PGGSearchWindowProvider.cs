using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using Attribute = System.Attribute;

namespace PGG.Editor
{
    public struct SearchContextElement
    {
        public object Target { get; private set; }
        public string Title { get; private set; }

        public SearchContextElement(object target, string title)
        {
            Target = target;
            Title = title;
        }
    }

    public class PGGSearchWindowProvider : ScriptableObject, ISearchWindowProvider
    {
        public PGGView View;
        public VisualElement target;

        public static List<SearchContextElement> Elements;

        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> tree = new List<SearchTreeEntry>();
            tree.Add(new SearchTreeGroupEntry(new GUIContent("Nodes"), 0));

            Elements = new List<SearchContextElement>();

            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblies)
            {
                foreach (Type type in assembly.GetTypes())
                {
                    if (type.CustomAttributes.ToList() != null)
                    {
                        Attribute attribute = type.GetCustomAttribute(typeof(NodeInfoAttribute));
                        if (attribute != null)
                        {
                            NodeInfoAttribute nodeAttribute = (NodeInfoAttribute)attribute;
                            if (nodeAttribute.IsOutputNode)
                                continue;
                            var node = Activator.CreateInstance(type);
                            if (string.IsNullOrEmpty(nodeAttribute.MenuItem))
                            {
                                continue;
                            }

                            Elements.Add(new SearchContextElement(node, nodeAttribute.MenuItem));
                        }
                    }
                }
            }

            Elements.Sort((a, b) =>
            {
                string[] aSplit = a.Title.Split('/');
                string[] bSplit = b.Title.Split('/');

                for (int i = 0; i < aSplit.Length; i++)
                {
                    if (i >= bSplit.Length)
                    {
                        return 1;
                    }

                    int value = String.Compare(aSplit[i], bSplit[i], StringComparison.Ordinal);

                    if (value != 0)
                    {
                        if (aSplit.Length != bSplit.Length && (i == aSplit.Length - 1 || i == bSplit.Length - 1))
                            return aSplit.Length < bSplit.Length ? 1 : -1;
                        return value;
                    }
                }

                return 0;
            });

            List<string> groups = new List<string>();

            foreach (SearchContextElement element in Elements)
            {
                string[] entryTitle = element.Title.Split('/');

                string groupName = "";

                for (int i = 0; i < entryTitle.Length - 1; i++)
                {
                    groupName += entryTitle[i];
                    if (!groups.Contains(groupName))
                    {
                        tree.Add(new SearchTreeGroupEntry(new GUIContent(entryTitle[i]), i + 1));
                        groups.Add(groupName);
                    }

                    groupName += "/";
                }

                SearchTreeEntry entry = new SearchTreeEntry(new GUIContent(entryTitle.Last()));
                entry.level = entryTitle.Length;
                entry.userData = new SearchContextElement(element.Target, element.Title);
                tree.Add(entry);
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            var windowMousePosition = View.ChangeCoordinatesTo(View, context.screenMousePosition - View.Window.position.position);

            var viewMousePosition = View.contentViewContainer.WorldToLocal(windowMousePosition);

            SearchContextElement element = (SearchContextElement)SearchTreeEntry.userData;

            Node node = (Node)element.Target;
            node.SetPosition(new Rect(viewMousePosition, new Vector2()));

            View.Add(node);

            return true;
        }
    }
}