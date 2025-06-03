using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.XR.XRDisplaySubsystem;

public class SetScreenToGlobalTexture : ScriptableRendererFeature
{
    public enum RenderType
    {
        Color,
        Depth
    }

    [SerializeField] string GlobalTextureName = "_PostProcessing";

    [SerializeField] public RenderPassEvent RenderPass = RenderPassEvent.AfterRenderingPostProcessing;
    [SerializeField] RenderType Type = RenderType.Color;
    class CustomRenderPass : ScriptableRenderPass
    {
        

        //protected RTHandle tempRender;

        protected ScriptableRenderer renderTarget;

        protected RenderType type;

        protected string TexName;

        protected int rtHandleID;

        public CustomRenderPass(string texName, RenderType type)
        {

            TexName = texName;

            rtHandleID = Shader.PropertyToID(TexName);

            //tempRender = RTHandles.Alloc(TexName, name: TexName);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            base.Configure(cmd, cameraTextureDescriptor);

            
        }

        public void SetRenderTarget(ScriptableRenderer target)
        {
            renderTarget = target;
        }

        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in a performant manner.
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor fullscreenDescriptor = renderingData.cameraData.cameraTargetDescriptor;

            fullscreenDescriptor.width = Mathf.Max(1, Screen.width); // <--
            fullscreenDescriptor.height = Mathf.Max(1, Screen.height); // <--

            //fullscreenDescriptor.width = Screen.width; //<--
            //fullscreenDescriptor.height = Screen.height; //<--
            fullscreenDescriptor.msaaSamples = 1;


            cmd.GetTemporaryRT(rtHandleID /*Shader.PropertyToID(tempRender.name)*/, fullscreenDescriptor);
        }


        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (renderingData.cameraData.cameraType != CameraType.Game)
                return;

            if (renderTarget == null) return;

            CommandBuffer cmd = CommandBufferPool.Get();


            switch (this.type)
            {
                case RenderType.Color:

                    cmd.Blit(renderTarget.cameraColorTargetHandle, rtHandleID);
                    break;

                case RenderType.Depth:
                    cmd.Blit(renderTarget.cameraDepthTargetHandle, rtHandleID);

                    break;
            }
            
            
            cmd.SetGlobalTexture(TexName, rtHandleID);

            context.ExecuteCommandBuffer(cmd);

            CommandBufferPool.Release(cmd);

        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(rtHandleID/*Shader.PropertyToID(tempRender.name)*/);

            //if(tempRender != null)
            //{
            //    RTHandles.Release(tempRender);

            //    tempRender = null;
            //}
        }

    }


    CustomRenderPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass(GlobalTextureName, this.Type);
        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPass;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);

        
        m_ScriptablePass.SetRenderTarget(renderer);
    }
}


