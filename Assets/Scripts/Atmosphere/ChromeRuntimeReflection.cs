using System;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;

namespace Atmosphere
{
    public class ChromeRuntimeReflection : MonoBehaviour
    {
        public ReflectionProbe ReflectionProbe;
        public RenderTexture RuntimeRenderTexture;
        public Material Chrome;

        private void Start()
        {
            AssignRenderTexture();
        }

        private void AssignRenderTexture()
        {
            if (ReflectionProbe != null)
            {
                RenderTextureDescriptor Desc = new RenderTextureDescriptor(200, 200);
                Desc.dimension = TextureDimension.Cube;
                Desc.msaaSamples = 2;
                Desc.colorFormat = RenderTextureFormat.DefaultHDR;
                Desc.graphicsFormat = GraphicsFormat.B8G8R8A8_UNorm;
                Desc.stencilFormat = GraphicsFormat.None;
                Desc.useDynamicScale = true;
                Desc.shadowSamplingMode = ShadowSamplingMode.CompareDepths;

                ReflectionProbe.realtimeTexture = RuntimeRenderTexture = new RenderTexture(Desc);
                Chrome.SetTexture("_Cubemap", RuntimeRenderTexture);
            }
        }
    }
}