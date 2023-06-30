using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Core.Effects.Rendering
{
    internal class SceneTransitionPass : ScriptableRenderPass
    {
        private new ProfilingSampler profilingSampler = new ("SceneTransition");
        private Material material;
        private RenderTargetIdentifier cameraColorTarget;

        public SceneTransitionPass(Material material)
        {
            this.material = material;
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public void SetTarget(RenderTargetIdentifier colorHandle)
        {
            cameraColorTarget = colorHandle;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            ConfigureTarget(new RenderTargetIdentifier(cameraColorTarget, 0, CubemapFace.Unknown, -1));
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var camera = renderingData.cameraData.camera;
            if (camera.cameraType != CameraType.Game)
                return;

            if (material == null)
                return;

            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, profilingSampler))
            {
                //material.SetFloat("_Intensity", intensity);
                cmd.SetRenderTarget(new RenderTargetIdentifier(cameraColorTarget, 0, CubemapFace.Unknown, -1));
                //The RenderingUtils.fullscreenMesh argument specifies that the mesh to draw is a quad.
                cmd.DrawMesh(RenderingUtils.fullscreenMesh, Matrix4x4.identity, material);
            }
            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            CommandBufferPool.Release(cmd);
        }
    }
}