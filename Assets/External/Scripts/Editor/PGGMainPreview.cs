using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace PGG.Editor
{
    public class PGGMainPreview : VisualElement
    {
        private PGGWindow _window;


        private VisualElement _viewer;
        private RenderTexture _renderTexture;

        public PGGMainPreview(PGGWindow window)
        {
            _window = window;
            _renderTexture = AssetDatabase.LoadAssetAtPath<RenderTexture>("Assets/External/Resource/Editor/Textures/GenerationPreview.renderTexture");

            _viewer = new VisualElement() { name = "preview", style = { backgroundSize = new StyleBackgroundSize(new BackgroundSize(BackgroundSizeType.Contain)), backgroundPositionX = new StyleBackgroundPosition(new BackgroundPosition(BackgroundPositionKeyword.Center)), backgroundPositionY = new StyleBackgroundPosition(new BackgroundPosition(BackgroundPositionKeyword.Bottom)), backgroundImage = new StyleBackground(Background.FromRenderTexture(_renderTexture)) } };

            pickingMode = PickingMode.Ignore;
            style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            style.flexWrap = new StyleEnum<Wrap>(Wrap.NoWrap);
            style.alignSelf = new StyleEnum<Align>(Align.Stretch);
            style.alignContent = new StyleEnum<Align>(Align.Stretch);
            style.alignItems = new StyleEnum<Align>(Align.Stretch);
            style.justifyContent = new StyleEnum<Justify>(Justify.FlexEnd);
            style.flexShrink = 1;
            style.flexGrow = 1;
            style.paddingTop = 16f;
            style.paddingRight = 16f;

            VisualElement listView = new VisualElement();
            listView.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Column);
            listView.style.alignSelf = new StyleEnum<Align>(Align.Stretch);
            listView.style.alignContent = new StyleEnum<Align>(Align.Stretch);
            listView.style.alignItems = new StyleEnum<Align>(Align.Stretch);
            listView.style.justifyContent = new StyleEnum<Justify>(Justify.FlexEnd);
            listView.style.width = new StyleLength(new Length(30f, LengthUnit.Percent));
            listView.style.height = new StyleLength(new Length(100f, LengthUnit.Percent));
            listView.pickingMode = PickingMode.Ignore;

            Add(listView);

            //_viewer.style.flexDirection = new StyleEnum<FlexDirection>(FlexDirection.Row);
            _viewer.style.flexShrink = 1;
            _viewer.style.flexGrow = 1;
            _viewer.pickingMode = PickingMode.Ignore;
            //_viewer.style.alignSelf = new StyleEnum<Align>(Align.Stretch);
            //_viewer.style.alignContent = new StyleEnum<Align>(Align.Stretch);
            //_viewer.style.alignItems = new StyleEnum<Align>(Align.Stretch);
            //_viewer.style.justifyContent = new StyleEnum<Justify>(Justify.FlexEnd);
            _viewer.style.maxHeight = new StyleLength(new Length(50f, LengthUnit.Percent));

            Button reloadButton = new Button(() => _window._currentGraph.OnGraphCooked.Invoke());
            reloadButton.text = "Generate Example";
            reloadButton.style.alignSelf = new StyleEnum<Align>(Align.FlexEnd);
            reloadButton.style.alignContent = new StyleEnum<Align>(Align.Stretch);
            reloadButton.style.alignItems = new StyleEnum<Align>(Align.Stretch);
            reloadButton.style.height = 30f;
            reloadButton.style.width = new StyleLength(new Length(100f, LengthUnit.Percent));


            listView.Add(_viewer);
            listView.Add(reloadButton);
        }
    }
}