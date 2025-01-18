using System;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.UIElements;

namespace PGG.Editor
{
    public class PGGPreview : VisualElement
    {
        public const int TEXREZ = 64;
        private SerializedObject _serializedObject;
        private GraphAsset _graphAsset;

        public Texture2D PreviewTexture = new Texture2D(TEXREZ, TEXREZ, TextureFormat.ARGB32, false);
        private Image _previewImage;

        public PGGPreview(SerializedObject obj)
        {
            _serializedObject = obj;
            _graphAsset = (GraphAsset)_serializedObject.targetObject;

            style.position = new StyleEnum<Position>(Position.Relative);
            style.alignSelf = new StyleEnum<Align>(Align.Center);

            ScrollView scrollView = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
            scrollView.horizontalScrollerVisibility = ScrollerVisibility.Hidden;
            scrollView.verticalScrollerVisibility = ScrollerVisibility.Hidden;

            Add(scrollView);

            _previewImage = new Image { name = "preview", image = PreviewTexture, scaleMode = ScaleMode.ScaleToFit };

            scrollView.Add(_previewImage);
        }

        public void WriteToImage(MinMaxHeightMap heightMap)
        {
            Color[] byteArray = new Color[TEXREZ * TEXREZ];
            float min = heightMap.Min;
            float max = heightMap.Max;
            int i = 0;
            for (int y = 0; y < TEXREZ; y++)
            {
                for (int x = 0; x < TEXREZ; x++)
                {
                    var value = heightMap.HeightMap[x, y];
                    var norm = Mathf.InverseLerp(min, max, value);
                    byteArray[i++] = new Color(norm, norm, norm);
                }
            }

            if (PreviewTexture)
            {
                PreviewTexture.SetPixels(byteArray);
                PreviewTexture.Apply();
            }
        }
    }
}