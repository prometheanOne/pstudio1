using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class DrawDepth :  MonoBehaviour
{
	protected bool isSupported = true;
	protected bool supportHDRTextures = true;

	public Shader depthShader = null;
	private Material depthMaterial = null;	
	
	void OnEnable()
	{
	    if (depthShader == null) depthShader = Shader.Find("Hidden/AdvancedSS/DrawDepth");
		isSupported = true;
        camera.depthTextureMode |= DepthTextureMode.Depth;
        camera.renderingPath = RenderingPath.Forward;
	}	

	// deprecated but needed for old effects to survive upgrade
	bool CheckSupport ()
    {
		return CheckSupport (true);
	}

	void Start ()
    {
		 CheckResources ();
	}	

	void OnDisable()
	{
	    if (depthMaterial)
	        DestroyImmediate(depthMaterial);
	}

	bool CheckResources ()
    {	
		CheckSupport (true);
		depthMaterial = CheckShaderAndCreateMaterial(depthShader,depthMaterial);
		
		if(!isSupported)
			ReportAutoDisable ();
		return isSupported;			
	}

    bool CheckSupport(bool needDepth)
    {
		isSupported = true;
		supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf);
		
		if (!SystemInfo.supportsImageEffects || !SystemInfo.supportsRenderTextures) {
			NotSupported ();
			return false;
		}		
		
		if(needDepth && !SystemInfo.SupportsRenderTextureFormat (RenderTextureFormat.Depth)) {
			NotSupported ();
			return false;
		}
		
		if(needDepth)
			camera.depthTextureMode |= DepthTextureMode.Depth;
		
		return true;
	}

	bool CheckSupport (bool needDepth, bool needHdr)
    {
		if(!CheckSupport(needDepth))
			return false;
		
		if(needHdr && !supportHDRTextures) {
			NotSupported ();
			return false;		
		}
		
		return true;
	}	

    void ReportAutoDisable()
    {
        Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
    }
			
	// deprecated but needed for old effects to survive upgrading
	bool CheckShader (Shader s)
    {
		Debug.Log("The shader " + s.ToString () + " on effect "+ this.ToString () + " is not part of the Unity 3.2+ effects suite anymore. For best performance and quality, please ensure you are using the latest Standard Assets Image Effects (Pro only) package.");		
		if (!s.isSupported) {
			NotSupported ();
			return false;
		} 
		else {
			return false;
		}
	}
	
	void NotSupported ()
    {
		enabled = false;
		isSupported = false;
		return;
	}

    private void LateUpdate()
    {
        DoDepthRender();
    }

    public void DoDepthRender()
    {
        camera.RenderWithShader(depthShader, "");
    }

    //void OnRenderImage (RenderTexture source, RenderTexture destination)
    //{		
    //    if(CheckResources()==false) {
    //        Graphics.Blit (source, destination);
    //        return;
    //    }
		
    //    Graphics.Blit (source, destination, depthMaterial); 	
    //}

	Material CheckShaderAndCreateMaterial (Shader s, Material m2Create)
    {
		if (!s) { 
			Debug.Log("Missing shader in " + this.ToString ());
			enabled = false;
			return null;
		}
			
		if (s.isSupported && m2Create && m2Create.shader == s) 
			return m2Create;
		
		if (!s.isSupported) {
			NotSupported ();
			Debug.LogError("The shader " + s.ToString() + " on effect "+this.ToString()+" is not supported on this platform!");
			return null;
		}
		else {
			m2Create = new Material (s);	
			m2Create.hideFlags = HideFlags.DontSave;		
			if (m2Create) 
				return m2Create;
			else return null;
		}
	}

	Material CreateMaterial (Shader s, Material m2Create)
    {
		if (!s) { 
			Debug.Log ("Missing shader in " + this.ToString ());
			return null;
		}
			
		if (m2Create && (m2Create.shader == s) && (s.isSupported)) 
			return m2Create;
		
		if (!s.isSupported) {
			return null;
		}
		else {
			m2Create = new Material (s);	
			m2Create.hideFlags = HideFlags.DontSave;		
			if (m2Create) 
				return m2Create;
			else return null;
		}
	}
}