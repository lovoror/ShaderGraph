/*
 *  @CreatedTime 2021年12月20日 17:37:04
 *  @FileName TestBlurPass.cs
 *  @Version 1.00
 *  @Author  Jacob.zhang
 *
*/

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Scenes.Blur.Script
{
    public class TestBlurPass : ScriptableRenderPass
    {
        static readonly string k_RenderTag = "Render TestBlur Effects";
        //Shader.PropertyToID拿到我们的Shader在Pass里面的参数的赋予ID
        static readonly int MainTexId = Shader.PropertyToID("_MainTex");
        static readonly int TempTargetId = Shader.PropertyToID("_TempTargetTestBlur");
        static readonly int FocusPowerId = Shader.PropertyToID("_FocusPower");
        static readonly int FocusDetailId = Shader.PropertyToID("_FocusDetail");
        static readonly int FocusScreenPositionId = Shader.PropertyToID("_FocusScreenPosition");
        static readonly int ReferenceResolutionXId = Shader.PropertyToID("_ReferenceResolutionX");
        TestBlur testBlur;
        Material testBlurMaterial;
        RenderTargetIdentifier currentTarget;
        
        public TestBlurPass(RenderPassEvent evt)
        {
            //接着，我们先进行构造函数的赋予，主要为了定义我们的shader，以及把这个shader汇入到我们新建的材质中，如果没有shader则进行报错：
            renderPassEvent = evt;
            var shader = Shader.Find("PostEffect/Blur");
            if (shader == null)
            {
                Debug.LogError("Shader not found.");
                return;
            }
            testBlurMaterial = CoreUtils.CreateEngineMaterial(shader);
        }
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            if (testBlurMaterial == null)
            {
                Debug.LogError("Material not created.");
                return;
            }
            
            if (!renderingData.cameraData.postProcessEnabled) return;
            //主要的目的在于判断材质是否创造成功，以及将其汇入到我们的Commandbuffer中
            var stack = VolumeManager.instance.stack;
            testBlur = stack.GetComponent<TestBlur>();
            if (testBlur == null) { return; }
            if (!testBlur.IsActive()) { return; }

            var cmd = CommandBufferPool.Get(k_RenderTag);
            Render(cmd, ref renderingData);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
        
        public void Setup(in RenderTargetIdentifier currentTarget)
        {
            this.currentTarget = currentTarget;
        }

        void Render(CommandBuffer cmd, ref RenderingData renderingData)
        {
            ref var cameraData = ref renderingData.cameraData;
            var source = currentTarget;
            int destination = TempTargetId;
            //宽高缩放比，主要为了呈现出类似我的世界的像素块模糊
            var w = (int)(cameraData.camera.scaledPixelWidth / testBlur.downSample.value);
            var h = (int)(cameraData.camera.scaledPixelHeight / testBlur.downSample.value);
            testBlurMaterial.SetFloat(FocusPowerId, testBlur.BiurRadius.value);
            
            //从渲染来源获得我们的当前摄像机的图片，将其进行RT生成得到我们需要的图片
            int shaderPass = 0;
            cmd.SetGlobalTexture(MainTexId, source);
            cmd.GetTemporaryRT(destination, w, h, 0, FilterMode.Point, RenderTextureFormat.Default);
            
            //导入我们的来源到目的地上面
            cmd.Blit(source, destination);
            for (int i = 0; i < testBlur.Iteration.value; i++)
            {
                cmd.GetTemporaryRT(destination, w / 2, h / 2, 0, FilterMode.Point, RenderTextureFormat.Default);
                cmd.Blit(destination, source, testBlurMaterial, shaderPass);
                cmd.Blit(source, destination);
                cmd.Blit(destination, source, testBlurMaterial, shaderPass + 1);
                cmd.Blit(source, destination);
            }
            for (int i = 0; i < testBlur.Iteration.value; i++)
            {
                cmd.GetTemporaryRT(destination, w * 2, h * 2, 0, FilterMode.Point, RenderTextureFormat.Default);
                cmd.Blit(destination, source, testBlurMaterial, shaderPass);
                cmd.Blit(source, destination);
                cmd.Blit(destination, source, testBlurMaterial, shaderPass + 1);
                cmd.Blit(source, destination);
            }

            cmd.Blit(destination, destination, testBlurMaterial, 0);
        }
    }
}