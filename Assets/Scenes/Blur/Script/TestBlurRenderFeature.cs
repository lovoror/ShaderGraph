/*
 *  @CreatedTime 2021年12月20日 17:27:06
 *  @FileName TestBlurRenderFeature.cs
 *  @Version 1.00
 *  @Author  Jacob.zhang
 *
*/

using Scenes.Blur.Script;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class TestBlurRenderFeature : ScriptableRendererFeature
{
 
  TestBlurPass testBlurPass;

  public override void Create()
  {
   testBlurPass = new TestBlurPass(RenderPassEvent.BeforeRenderingPostProcessing);
  }

  public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
  {
   testBlurPass.Setup(renderer.cameraColorTarget);
   renderer.EnqueuePass(testBlurPass);
  }
}
