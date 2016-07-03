using UnityEngine;
using System.Collections;

/// <summary>
/// PostProcessBase is a pritty much a straight C# port of Unities PostEffectBase class. Generic Evils Image Effects 
/// use this as a base, so you dont need to import the pro image pack, and so it wont conflit with other version of 
/// this class.
/// </summary>
public class PostProcessBase : MonoBehaviour 
{
    protected bool supportHDRTextures = true;
	protected bool supportDX11 = false;
	protected bool isSupported = true;
	
	public Material CheckShaderAndCreateMaterial (Shader shader, Material m2Create)  
    {
        if (shader == null) 
        { 
			Debug.Log("Missing shader in " + this.ToString ());
			enabled = false;
			return null;
		}

        if (shader.isSupported && m2Create && m2Create.shader == shader) 
			return m2Create;

        if (!shader.isSupported)
        {
            NotSupported();
            Debug.Log("The shader " + shader.ToString() + " on effect " + this.ToString() + " is not supported on this platform!");
            return null;
        }
        else
        {
            m2Create = new Material(shader);
            m2Create.hideFlags = HideFlags.DontSave;
            if (m2Create) return m2Create;
            else return null;
        }
	}

    public Material CreateMaterial (Shader shader, Material m2Create) 
    {
        if (shader == null)
        { 
            Debug.Log ("Missing shader in " + this.ToString ());
            return null;
        }

        if (m2Create && (m2Create.shader == shader) && (shader.isSupported)) 
            return m2Create;

        if (!shader.isSupported)
        {
            return null;
        }
        else
        {
            m2Create = new Material(shader);
            m2Create.hideFlags = HideFlags.DontSave;
            if (m2Create) return m2Create;
            else return null;
        }
    }

    void OnEnable()
    {
        isSupported = true;
    }

    public bool CheckSupport()
    {
        return CheckSupport(false);
    }
	
    protected virtual bool CheckResources ()
    {
        Debug.LogWarning ("CheckResources () for " + this.ToString() + " should be overloaded.");
        return isSupported;
    }

    void Start()
    {
        CheckResources();
    }	

    public bool CheckSupport(bool needDepth)
    {
        isSupported = true;
        supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
        supportDX11 = SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders;

        if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures)
        {
            NotSupported();
            return false;
        }

        if (needDepth && !SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.Depth))
        {
            NotSupported();
            return false;
        }

        if (needDepth)
            camera.depthTextureMode |= DepthTextureMode.Depth;

        return true;
    }

    public bool CheckSupport(bool needDepth, bool needHdr)
    {
        if (!CheckSupport(needDepth))
            return false;

        if (needHdr && !supportHDRTextures)
        {
            NotSupported();
            return false;
        }

        return true;
    }
	
    public bool Dx11Support()
    {
        return supportDX11;
    }

    protected void ReportAutoDisable()
    {
        Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
    }

    protected void NotSupported()
    {
        enabled = false;
        isSupported = false;
    }
	
    //function DrawBorder (dest : RenderTexture, material : Material ) {
    //    var x1 : float;	
    //    var x2 : float;
    //    var y1 : float;
    //    var y2 : float;		
		
    //    RenderTexture.active = dest;
    //    var invertY : boolean = true; // source.texelSize.y < 0.0f;
    //    // Set up the simple Matrix
    //    GL.PushMatrix();
    //    GL.LoadOrtho();		
        
    //    for (var i : int = 0; i < material.passCount; i++)
    //    {
    //        material.SetPass(i);
	        
    //        var y1_ : float; var y2_ : float;
    //        if (invertY)
    //        {
    //            y1_ = 1.0; y2_ = 0.0;
    //        }
    //        else
    //        {
    //            y1_ = 0.0; y2_ = 1.0;
    //        }
	        	        
    //        // left	        
    //        x1 = 0.0;
    //        x2 = 0.0 + 1.0/(dest.width*1.0);
    //        y1 = 0.0;
    //        y2 = 1.0;
    //        GL.Begin(GL.QUADS);
	        
    //        GL.TexCoord2(0.0, y1_); GL.Vertex3(x1, y1, 0.1);
    //        GL.TexCoord2(1.0, y1_); GL.Vertex3(x2, y1, 0.1);
    //        GL.TexCoord2(1.0, y2_); GL.Vertex3(x2, y2, 0.1);
    //        GL.TexCoord2(0.0, y2_); GL.Vertex3(x1, y2, 0.1);
	
    //        // right
    //        x1 = 1.0 - 1.0/(dest.width*1.0);
    //        x2 = 1.0;
    //        y1 = 0.0;
    //        y2 = 1.0;

    //        GL.TexCoord2(0.0, y1_); GL.Vertex3(x1, y1, 0.1);
    //        GL.TexCoord2(1.0, y1_); GL.Vertex3(x2, y1, 0.1);
    //        GL.TexCoord2(1.0, y2_); GL.Vertex3(x2, y2, 0.1);
    //        GL.TexCoord2(0.0, y2_); GL.Vertex3(x1, y2, 0.1);	        
	
    //        // top
    //        x1 = 0.0;
    //        x2 = 1.0;
    //        y1 = 0.0;
    //        y2 = 0.0 + 1.0/(dest.height*1.0);

    //        GL.TexCoord2(0.0, y1_); GL.Vertex3(x1, y1, 0.1);
    //        GL.TexCoord2(1.0, y1_); GL.Vertex3(x2, y1, 0.1);
    //        GL.TexCoord2(1.0, y2_); GL.Vertex3(x2, y2, 0.1);
    //        GL.TexCoord2(0.0, y2_); GL.Vertex3(x1, y2, 0.1);
	        
    //        // bottom
    //        x1 = 0.0;
    //        x2 = 1.0;
    //        y1 = 1.0 - 1.0/(dest.height*1.0);
    //        y2 = 1.0;

    //        GL.TexCoord2(0.0, y1_); GL.Vertex3(x1, y1, 0.1);
    //        GL.TexCoord2(1.0, y1_); GL.Vertex3(x2, y1, 0.1);
    //        GL.TexCoord2(1.0, y2_); GL.Vertex3(x2, y2, 0.1);
    //        GL.TexCoord2(0.0, y2_); GL.Vertex3(x1, y2, 0.1);	
	                	              
    //        GL.End();	
    //    }	
        
    //    GL.PopMatrix();
    //}
}
