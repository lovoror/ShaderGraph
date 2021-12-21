﻿/*
 *  @CreatedTime 2021年12月20日 17:25:55
 *  @FileName TestBlur.cs
 *  @Version 1.00
 *  @Author  Jacob.zhang
 *
*/

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

    public class TestBlur : VolumeComponent,IPostProcessComponent
    {
        [Range(0f, 100f), Tooltip("模糊强度")]
        public FloatParameter BiurRadius = new FloatParameter(0f);

        [Range(0, 10), Tooltip("模糊质量")]
        public IntParameter Iteration = new IntParameter(5);

        [Range(1, 10), Tooltip("模糊深度")]
        public FloatParameter downSample = new FloatParameter(0f);

        public bool IsActive() => downSample.value > 0f;

        public bool IsTileCompatible() => false;
       
    }
