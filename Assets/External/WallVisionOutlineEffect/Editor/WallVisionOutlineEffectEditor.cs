using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof(WallVisionOutlineEffect))]
public class WallVisionOutlineEffectEditor : Editor {

	WallVisionOutlineEffect effect;
	bool showHelp = true;
	Texture2D rampTexture = null;
 
    public void OnEnable()
    {
        effect = (WallVisionOutlineEffect)target;
    }

	public override void OnInspectorGUI () {
		if (showHelp) {
			EditorGUILayout.HelpBox("Use this property to change the color of the objects behind occluders.", MessageType.Info);
		}
		effect.color = EditorGUILayout.ColorField ("Color", effect.color);

		if (showHelp) {
			EditorGUILayout.HelpBox("Unity's Glow/Bloom effect uses the alpha channel of the frame buffer. This slider determines how much of a glow will be applied to this effect. Note: The glow affecting this effect has to be behind this in the inspector view.", MessageType.Info);
		}
		effect.glowStrength = EditorGUILayout.Slider("Glow Strength", effect.glowStrength, 0.0f, 1.0f);

		if (showHelp) {
			EditorGUILayout.HelpBox("This Layer Masks determine which objects will have the outline effect (Visible) when they are behind some other objects (Occluder).", MessageType.Info);
		}
		effect.occluderLayer = CustomFields.LayerMaskField("Occluder", effect.occluderLayer, true);
		effect.wallVisionLayer = CustomFields.LayerMaskField("Visible", effect.wallVisionLayer, true);

		if (showHelp) {
			EditorGUILayout.HelpBox("The visibility curve effects the visibility of the outline. You can set the visibility (vertical axis) at a certain degree (horizontal axis) between the surface normal and the vector to the camera. Both values are normalized between 0.0 und 1.0.", MessageType.Info);
		}
		effect.visibilityCurve = EditorGUILayout.CurveField("Visibility Curve", effect.visibilityCurve);

		EditorGUILayout.Space();
		EditorGUILayout.LabelField("Pattern", EditorStyles.boldLabel);
		if (showHelp) {
			EditorGUILayout.HelpBox("You can use a pattern texture to enhance the effect. The pattern affects the opacity of the effect in relation to it's screen position. The texture has to be a grayscale texture, where white means full opacity and black zero opacity. The scale factor is in relation to the screen resolution. 1.0 means one tile of the pattern is scaled over the full screen (with respect to the aspect ratio). The weight parameter defines how much the pattern is affecting the end-result.", MessageType.Info);
		}
		effect.patternTexture = (Texture) EditorGUILayout.ObjectField("Texture", effect.patternTexture, typeof(Texture), true);
		effect.patternScale = EditorGUILayout.Slider("Scale", effect.patternScale, 0.0f, 5.0f);
		effect.patternWeight = EditorGUILayout.Slider("Weight", effect.patternWeight, -5.0f, 5.0f);

		EditorGUILayout.Space();
		showHelp = EditorGUILayout.Toggle("Show Help", showHelp);


		if (GUI.changed) {
			RampUpdate();
			EditorUtility.SetDirty(target);
		}
    }

    void RampUpdate() {
    	if (rampTexture != null)
    		DestroyImmediate(rampTexture);

		rampTexture = new Texture2D(256, 1, TextureFormat.Alpha8, false);
		for (int i=0; i<rampTexture.width; i++) {
			float value = effect.visibilityCurve.Evaluate(i/(float)rampTexture.width);
			rampTexture.SetPixel(i, 0, new Color(value, value, value, value));
		}
		rampTexture.Apply();

		effect.rampTexture = rampTexture;
	}
}