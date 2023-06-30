using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Core.Effects.Rendering
{
    public class SceneTransitionRendererFeature : ScriptableRendererFeature
    {
        public Material material;

        private SceneTransitionPass renderPass = null;

        public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.cameraType == CameraType.Game)
            {
                //Calling ConfigureInput with the ScriptableRenderPassInput.Color argument ensures that the opaque texture is available to the Render Pass
                renderPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
                renderPass.ConfigureInput(ScriptableRenderPassInput.Color);
                renderPass.SetTarget(renderer.cameraColorTarget);
                renderer.EnqueuePass(renderPass);
            }
        }

        public override void Create()
        {
            renderPass = new SceneTransitionPass(material);
        }
    }
}