using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static SetScreenToGlobalTexture;

public class ShaderToScreen : ScriptableRendererFeature
{
    [SerializeField] RenderPassEvent RenderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    [SerializeField] int Priority = -1;
    
    [SerializeField] Material ScreenMaterial;

    private void OnValidate()
    {
        

        if(m_ScriptablePass != null)
        {
            m_ScriptablePass.SetMatierial(ScreenMaterial);
            m_ScriptablePass.SetPriority(Priority);
            m_ScriptablePass.renderPassEvent = RenderPassEvent;
        }
    }
    class CustomRenderPass : ScriptableRenderPass
    {


        protected RTHandle tempRender;

        protected ScriptableRenderer renderTarget;

        protected RenderType type;

        protected string TexName;

        protected int prio;

        protected Material material;

        public CustomRenderPass()
        {
            

            TexName = "ScreenTexture";

            tempRender = RTHandles.Alloc(TexName, name: TexName);
        }

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            base.Configure(cmd, cameraTextureDescriptor);
            
        }

        public void SetRenderTarget(ScriptableRenderer target)
        {
            renderTarget = target;
        }

        public void SetMatierial(Material material)
        {
            this.material = material;
        }

        public void SetPriority(int priority)
        {
            prio = priority;
        }

        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in a performant manner.
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            

            RenderTextureDescriptor fullscreenDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            
            


            cmd.GetTemporaryRT(Shader.PropertyToID(tempRender.name), fullscreenDescriptor);

        }


        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if(renderingData.cameraData.cameraType != CameraType.Game) return;

            if (material != null)
            {
                CommandBuffer cmd = CommandBufferPool.Get();

                cmd.Blit(renderingData.cameraData.targetTexture, Shader.PropertyToID(tempRender.name));



                material.mainTexture = renderingData.cameraData.targetTexture;
                cmd.Blit(Shader.PropertyToID(tempRender.name), renderingData.cameraData.renderer.cameraColorTargetHandle, material, prio);
                    //Blitter.BlitCameraTexture(cmd, tempRender, renderTarget.cameraColorTargetHandle);

                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                cmd.Release();
            }

        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            
            cmd.ReleaseTemporaryRT(Shader.PropertyToID(tempRender.name));
        }
    }

    CustomRenderPass m_ScriptablePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new CustomRenderPass();
        

        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent;
        
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
        m_ScriptablePass.SetRenderTarget(renderer);
        m_ScriptablePass.SetMatierial(ScreenMaterial);
        m_ScriptablePass.SetPriority(Priority);
    }


}


