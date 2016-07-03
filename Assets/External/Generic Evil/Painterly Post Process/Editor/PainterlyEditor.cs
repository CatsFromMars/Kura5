// Painterly Post Process
// Copyright 2014 Generic Evil Business Ltd.
// http://www.genericevil.com

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;

[CustomEditor(typeof(PainterlyImageEffect))]
public class PainterlyEditor : Editor 
{
    public override void OnInspectorGUI()
    {
        EditorStyles.label.wordWrap = true;

        var logo = AssetDatabase.LoadAssetAtPath("Assets/Generic Evil/Painterly Post Process/painterlyLogo.png", typeof(Texture2D)) as Texture2D;
        //EditorGUILayout.LabelField(logo);
        GUILayout.Label(logo);

        PainterlyImageEffect effect = target as PainterlyImageEffect;
        SerializedObject obj = new SerializedObject(effect);

        // get the properties we care about.
        var sampleCount = obj.FindProperty("SampleCount");
        var sampleMode = obj.FindProperty("SampleMode");
        var combineMode = obj.FindProperty("CombineMode");
        var grade = obj.FindProperty("Gradeing");
        var intensity = obj.FindProperty("intensity");
        var offset = obj.FindProperty("pixelOffset");
        var curveWidth = obj.FindProperty("curveWidth");
        var enableDepthFade = obj.FindProperty("EnableDepthFade");
        var depthStart = obj.FindProperty("depthStart");
        var depthEnd = obj.FindProperty("depthEnd");

        obj.Update();

        int sc = (int)Enum.GetValues(typeof(SampleCount)).GetValue(sampleCount.enumValueIndex);

        sampleCount.enumValueIndex = ((int)EditorGUILayout.Slider("Samples", sc, 3, 6)) - 3;
        EditorGUILayout.LabelField("Controls how many per-pixel samples are collected. The more samples, the smoother the effect.");

        offset.floatValue = ((float)EditorGUILayout.Slider("Sample Radius", offset.floatValue, 0, 5));
        EditorGUILayout.LabelField("Controls how far out samples are taken. Higher values make brushstrokes look really chunky.");

        EditorGUILayout.PropertyField(sampleMode);
        switch ((SampleMode)sampleMode.enumValueIndex)
        {
            case SampleMode.Reguar:
                EditorGUILayout.LabelField("Sends out samples in a circular pattern around  each pixel.");
                break;
            case SampleMode.Randomize:
                EditorGUILayout.LabelField("Jitters each sample by a random amount. This gives edges a fuzzy effect.");
                break;
            case SampleMode.URotate:
                EditorGUILayout.LabelField("Creates a curvy brush-stroke effect.");
                EditorGUILayout.PropertyField(curveWidth);
                EditorGUILayout.LabelField("Defines the width of the stroke effect.");
                break;
        }

        EditorGUILayout.PropertyField(combineMode);
        switch ((CombineMode)combineMode.enumValueIndex)
        {
            case CombineMode.Max:
                EditorGUILayout.LabelField("The brightest colours are used in the paint strokes.");
                break;
            case CombineMode.Min:
                EditorGUILayout.LabelField("The darkest colours are used in the paint strokes.");
                break;
            case CombineMode.Intensity:
                EditorGUILayout.LabelField("The most common colour intensity is used.");
                break;
        }

        bool g = (Grading)Enum.GetValues(typeof(Grading)).GetValue(grade.enumValueIndex) == Grading.Grade;
        g = EditorGUILayout.Toggle("Enable Color Grading", g);
        grade.enumValueIndex = g ? 1 : 0;
        if (g)
        {
            // intensity
            EditorGUILayout.PropertyField(intensity);
        }

        // Depth fade...
        EditorGUILayout.PropertyField(enableDepthFade);
        if (enableDepthFade.boolValue)
        {
            EditorGUILayout.PropertyField(depthStart);
            EditorGUILayout.PropertyField(depthEnd);
        }
        

        EditorGUILayout.Space(); EditorGUILayout.Space();
        EditorGUILayout.LabelField("Animation Config");
        EditorGUILayout.Space();

        // grab some properties...

        var doAnimateSamples = obj.FindProperty("AnimateSamples");
        var sampleFrameRate = obj.FindProperty("SamplesFramerate");

        var doAnimateStrokeAngle = obj.FindProperty("AnimateStrokeAngle");
        var storkeAnimationAngleMode = obj.FindProperty("StorkeAnimationAngleMode");
        var strokeRotateAmmount = obj.FindProperty("StrokeRotateAmmount");
        var strokeJitterFramerate = obj.FindProperty("StrokeJitterFramerate");

        EditorGUILayout.PropertyField(doAnimateSamples);

        if (doAnimateSamples.boolValue)
        {
            EditorGUILayout.PropertyField(sampleFrameRate, new GUIContent("Jitter framerate"));
        }

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(doAnimateStrokeAngle, new GUIContent("Animate Strokes"));

        if (doAnimateStrokeAngle.boolValue)
        {
            EditorGUILayout.PropertyField(storkeAnimationAngleMode);
            switch ((StorkeAnimationAngleMode)storkeAnimationAngleMode.enumValueIndex)
            {
                case StorkeAnimationAngleMode.RotateLeft:
                case StorkeAnimationAngleMode.RotateRight:
                    EditorGUILayout.PropertyField(strokeRotateAmmount, new GUIContent("Rotation Modifier"));
                    break;
                case StorkeAnimationAngleMode.Jitter:
                    EditorGUILayout.PropertyField(strokeJitterFramerate, new GUIContent("Jitter framerate"));
                    EditorGUILayout.PropertyField(strokeRotateAmmount, new GUIContent("Jitter Ammount"));
                    break;
            }
        }
        
        obj.ApplyModifiedProperties();
    }
}
