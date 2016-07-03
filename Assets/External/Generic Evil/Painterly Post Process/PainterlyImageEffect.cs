// Painterly Post Process
// Copyright 2014 Generic Evil Business Ltd.
// http://www.genericevil.com

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[RequireComponent (typeof(Camera))]
[AddComponentMenu("Image Effects/Generic Evil/Painterly")]
public class PainterlyImageEffect : PostProcessBase
{
    [Range(0, 1.5f)]
	public float intensity = 1.1f;
    [Range(0, 100f)]
    public float curveWidth = 20f;
	private Material overlayMaterial;

    [HideInInspector]
    public Vector2[] Offests;
    [HideInInspector]
    public int OffsetCount =6;

    [Range(0, 360)]
    public float angle;
    [Range(0, 5)]
    public float pixelOffset = 1;

    public int PassCount = 3;
    public SampleCount SampleCount = SampleCount.Four;
    public SampleMode SampleMode = SampleMode.Randomize;
    public CombineMode CombineMode = CombineMode.Min;
    public Grading Gradeing = Grading.Grade;

    public bool AnimateSamples;
    [Range(5, 30)]
    public int SamplesFramerate = 10;

    public bool AnimateStrokeAngle;
    public StorkeAnimationAngleMode StorkeAnimationAngleMode;
    [Range(0, 1)]
    public float StrokeRotateAmmount;
    [Range(5, 30)]
    public float StrokeJitterFramerate = 10;

    private float sampleTimer;
    public float strokeAngleTimer;

    public bool EnableDepthFade = true;
    [Range(0, 1f)]
    public float depthStart;
    [Range(0, 1f)]
    public float depthEnd;


    void OnEnable()
    {
        camera.depthTextureMode |= DepthTextureMode.Depth;
    }

    void Update()
    {
        if (AnimateSamples)
        {
            sampleTimer += Time.deltaTime;

            if (sampleTimer >= (1f / SamplesFramerate))
            {
                if (SampleCount == global::SampleCount.Three)
                    SampleCount = global::SampleCount.Four;
                else if (SampleCount == global::SampleCount.Six)
                    SampleCount = global::SampleCount.Five;
                else
                    SampleCount = Random.value > 0.5f ? (SampleCount)((int)SampleCount - 1) : (SampleCount)((int)SampleCount + 1);
                sampleTimer = 0;
            }
        }

        if (EnableDepthFade)
        {
            camera.depthTextureMode = DepthTextureMode.Depth;
        }
        else
        {
            camera.depthTextureMode = DepthTextureMode.None;
        }

        if (AnimateStrokeAngle)
        {
            switch (StorkeAnimationAngleMode)
            {
                case StorkeAnimationAngleMode.RotateLeft:
                    angle += (StrokeRotateAmmount * 360) * Time.deltaTime;
                    break;
                case StorkeAnimationAngleMode.RotateRight:
                    angle -= (StrokeRotateAmmount * 360) * Time.deltaTime;
                    break;
                case StorkeAnimationAngleMode.Jitter:
                    strokeAngleTimer += Time.deltaTime;

                    if (strokeAngleTimer >= (1f / StrokeJitterFramerate))
                    {
                        var a = StrokeRotateAmmount * 360f;
                        angle += Random.value > 0.5f ? a : -a;
                        strokeAngleTimer = 0;
                    }
                    break;
            }
        }
    }

    void UpdateOffsets()
    {
        Offests = new Vector2[OffsetCount];
        float step = 360f / OffsetCount;

        var pixelSize = new Vector2((1f / Screen.width) * pixelOffset, (1f / Screen.height) * pixelOffset);
        for (int i = 0; i < OffsetCount; i++)
        {
            Vector2 a = new Vector2(1, 0);
            var e = (step * i);
            Quaternion rot = Quaternion.Euler(0, 0, e + angle);
            
            Offests[i] = (rot * a);// *mul;
            Offests[i] = new Vector2(Offests[i].x * pixelSize.x, Offests[i].y * pixelSize.y);
        }

        overlayMaterial.SetVector("_Offset1", Offests[0]);
        overlayMaterial.SetVector("_Offset2", Offests[1]);
        overlayMaterial.SetVector("_Offset3", Offests[2]);
        overlayMaterial.SetVector("_Offset4", Offests[3]);
        overlayMaterial.SetVector("_Offset5", Offests[4]);
        overlayMaterial.SetVector("_Offset6", Offests[5]);

        Vector4 depth = new Vector4(depthStart, depthEnd, 1f/(depthEnd-depthStart), 0);
        overlayMaterial.SetVector("_depthOffsets", depth);
    }

    protected override bool CheckResources()
    {
        CheckSupport(false);
        var name = "Hidden/Generic Evil/Painterly/Painterly" + GetName();
        overlayMaterial = CheckShaderAndCreateMaterial (Shader.Find(name), overlayMaterial);

        if (!isSupported)
        {
            ReportAutoDisable();
        }
        return isSupported;
    }
	
    void OnRenderImage (RenderTexture source, RenderTexture destination) {		
        if (CheckResources() == false) {
            Graphics.Blit (source, destination);
            return;
        }

        
        source.filterMode = FilterMode.Point;

        var px = pixelOffset;
        var a = angle;

        overlayMaterial.SetFloat("_PaintIntensity", intensity);
        overlayMaterial.SetFloat("_curveWidth", curveWidth);
        UpdateOffsets();
        RenderTexture buffer = RenderTexture.GetTemporary(Screen.width, Screen.height);
        Graphics.Blit(source, buffer, overlayMaterial);

        for (int i = 0; i < PassCount-1; i++)
        {
            RenderTexture buffer2 = RenderTexture.GetTemporary(Screen.width, Screen.height);
            angle += (360f / PassCount);
            pixelOffset += 1;
            UpdateOffsets();
            Graphics.Blit(buffer, buffer2, overlayMaterial);
            RenderTexture.ReleaseTemporary(buffer);
            buffer = buffer2;
        }
        Graphics.Blit(buffer, destination);
        RenderTexture.ReleaseTemporary(buffer);
        pixelOffset = px;
        angle = a;
    }

    private string GetName()
    {
        return SampleCount.ToString() + (SampleMode == SampleMode.Reguar ? "" : SampleMode.ToString()) + (CombineMode == CombineMode.Max ? "" : CombineMode.ToString()) + (Gradeing == Grading.None ? "" : Gradeing.ToString()) + (EnableDepthFade ? "Depth" : "");
    }
}

/// <summary>
/// The number of samples collected per pixel. Lower values do less work. Actual sample count is enum name + 1, as we 
/// allways sample each pixels actual colour as well!
/// </summary>
public enum SampleCount
{
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6
}

/// <summary>
/// Controls how the collected samples are cimbined.
/// </summary>
public enum CombineMode
{
    /// <summary>
    /// Use the Max intrinsic to calculate the pixel colour.
    /// </summary>
    Max,
    /// <summary>
    /// Use the Min intrinsic to calculate the pixel colour.
    /// </summary>
    Min,
    /// <summary>
    /// Attepts to pick the range of colours most representative of the sample
    /// </summary>
    Intensity
}

/// <summary>
/// Determines how the samples are collected
/// </summary>
public enum SampleMode
{
    /// <summary>
    /// Regular patter, spaed in (360 degrees / sample count) units.
    /// </summary>
    Reguar,
    /// <summary>
    /// Samples for each pixel are shot in a random direction
    /// </summary>
    Randomize,
    /// <summary>
    /// Same a regular, but then rotated by the pixels U coordinate. 
    /// </summary>
    URotate,
}

/// <summary>
/// Colour grading mode
/// </summary>
public enum Grading
{
    /// <summary>
    /// No colour grading.
    /// </summary>
    None,
    /// <summary>
    /// Colour grading is used to boost the colours and make them more/less vivid.
    /// </summary>
    Grade
}

public enum StorkeAnimationAngleMode
{
    RotateLeft,
    RotateRight,
    Jitter
}