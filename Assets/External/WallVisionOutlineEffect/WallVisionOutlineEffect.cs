using UnityEngine;
using System.Collections;

[AddComponentMenu("Image Effects/Wall Vision Outline Effect")]
[ExecuteInEditMode]
public class WallVisionOutlineEffect : ImageEffectBase {

	public Shader occluderShader;
	public Color color = Color.white;
	public float glowStrength;
	public Texture patternTexture = null;
	public float patternScale = 1.0f;
	public float patternWeight = 1.0f;

	public LayerMask occluderLayer;
	public LayerMask wallVisionLayer;

	public AnimationCurve visibilityCurve = AnimationCurve.EaseInOut(0.0f, 1.0f, 1.0f, 0.0f);
	public Texture2D rampTexture;

	private RenderTexture _silhouetteTexture;
	private RenderTexture _occluderTexture;
	private GameObject _silhouetteCamGameObject;
	private GameObject _occluderCamGameObject;
		
	void CleanUpTextures() {
		if (_silhouetteTexture) {
			RenderTexture.ReleaseTemporary(_silhouetteTexture);
			_silhouetteTexture = null;
		}
		
		if (_occluderTexture) {
			RenderTexture.ReleaseTemporary(_occluderTexture);
			_occluderTexture = null;
		}
	}
	
	void OnPreRender () {
		if (!enabled || !(gameObject.active == true))
			return;
		
		CleanUpTextures();
		
		_silhouetteTexture = RenderTexture.GetTemporary((int)camera.pixelWidth, (int)camera.pixelHeight, 16, RenderTextureFormat.ARGB32);
		_occluderTexture = RenderTexture.GetTemporary((int)camera.pixelWidth, (int)camera.pixelHeight, 16, RenderTextureFormat.ARGB32);
		
		if (!_silhouetteCamGameObject) {
			_silhouetteCamGameObject = new GameObject("SilhouetteCamera");
			_silhouetteCamGameObject.AddComponent<Camera>();
			_silhouetteCamGameObject.camera.enabled = false;
			_silhouetteCamGameObject.hideFlags = HideFlags.HideAndDontSave;
		}
		
		_silhouetteCamGameObject.camera.CopyFrom(camera);
		_silhouetteCamGameObject.camera.backgroundColor = new Color(0,0,0,0);
		_silhouetteCamGameObject.camera.clearFlags = CameraClearFlags.SolidColor;
		_silhouetteCamGameObject.camera.cullingMask = wallVisionLayer;
		_silhouetteCamGameObject.camera.targetTexture = _silhouetteTexture;
		_silhouetteCamGameObject.camera.RenderWithShader(Shader.Find("Hidden/Camera-DepthNormalTexture"), null);
		_silhouetteCamGameObject.camera.depthTextureMode |= DepthTextureMode.DepthNormals;
		
		if (!_occluderCamGameObject) {
			_occluderCamGameObject = new GameObject("OccluderCamera");
			_occluderCamGameObject.AddComponent<Camera>();
			_occluderCamGameObject.camera.enabled = false;
			_occluderCamGameObject.hideFlags = HideFlags.HideAndDontSave;
		}
		
		_occluderCamGameObject.camera.CopyFrom(camera);
		_occluderCamGameObject.camera.backgroundColor = new Color(0,0,0,0);
		_occluderCamGameObject.camera.clearFlags = CameraClearFlags.SolidColor;
		_occluderCamGameObject.camera.cullingMask = occluderLayer;
		_occluderCamGameObject.camera.targetTexture = _occluderTexture;
		_occluderCamGameObject.camera.RenderWithShader(occluderShader, null);
		_occluderCamGameObject.camera.depthTextureMode = DepthTextureMode.Depth;
		
		camera.depthTextureMode = DepthTextureMode.DepthNormals;
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		
		material.SetColor("_Color", color);
		material.SetTexture("_Silhouette", _silhouetteTexture);
		material.SetTexture("_Occluder", _occluderTexture);
		material.SetTexture("_PatternTex", patternTexture);
		material.SetFloat("_PatternScale", patternScale);
		material.SetFloat("_PatternWeight", patternWeight);
		material.SetFloat("_GlowStrength", glowStrength);
		material.SetFloat("_Aspect", camera.aspect);
		material.SetTexture("_RampTex", rampTexture);
		Graphics.Blit(source, destination, material);
		
		CleanUpTextures();
	}
	
	new void OnDisable () {
		if (_silhouetteCamGameObject) {
			DestroyImmediate(_silhouetteCamGameObject);
		}
		
		if (_occluderCamGameObject) {
			DestroyImmediate(_occluderCamGameObject);
		}
		
		base.OnDisable();
	}
}
