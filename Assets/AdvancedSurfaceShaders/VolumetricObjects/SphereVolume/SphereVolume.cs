using UnityEngine;

[ExecuteInEditMode]
public class SphereVolume : VolumetricObjectBase
{
    public float radius = 3.0f;
    private float previousRadius = 1.0f;
	public bool pulsate;
	public float pulsateFreq;

#if UNITY_EDITOR
    [UnityEditor.MenuItem("GameObject/Create Other/Volumetric Objects/Sphere Volume")]
    static public void CreateVolume()
    {
        GameObject newObject = new GameObject("Sphere Volume");
        if (UnityEditor.SceneView.currentDrawingSceneView) UnityEditor.SceneView.currentDrawingSceneView.MoveToView(newObject.transform);
        SphereVolume sphereVolume = (SphereVolume)newObject.AddComponent<SphereVolume>();
        sphereVolume.volumeShader = "Advanced SS/Volumetric/Sphere Volume";
        sphereVolume.enabled = false;
        sphereVolume.enabled = true;
    }
#endif

    protected override void OnEnable()
    {
        if (volumeShader == "") PopulateShaderName();
        base.OnEnable();
    }

    public override void PopulateShaderName()
    {
        volumeShader = "Advanced SS/Volumetric/Sphere Volume";
    }

    public override bool HasChanged()
    {
        if (radius != previousRadius || base.HasChanged())
        {
            return true;
        }
        return false;
    }

    protected override void SetChangedValues()
    {
        previousRadius = radius;
        base.SetChangedValues();
    }

	void Update() {
		if (pulsate) {
			SetChangedValues ();
			UpdateVolume ();
		}
	}

    public override void UpdateVolume()
    {
		Vector3 halfBoxSize;
		if (pulsate) halfBoxSize = Vector3.one * (((radius * 0.3f) * Mathf.Sin(2f * Mathf.PI * pulsateFreq * Time.time)) + radius) * 2.0f;
        else halfBoxSize = Vector3.one * radius * 2.0F;

        if (meshInstance)
        {
            ScaleMesh(meshInstance, halfBoxSize);

            // Set bounding volume so modified vertices don't get culled
            Bounds bounds = new Bounds();
            bounds.SetMinMax(-halfBoxSize, halfBoxSize);
            meshInstance.bounds = bounds;
        }

        if (materialInstance)
        {
            materialInstance.SetVector("_TextureData", new Vector4(-textureMovement.x, -textureMovement.y, -textureMovement.z, (1f / textureScale)));
            materialInstance.SetFloat("_RadiusSqr", radius * radius);
            materialInstance.SetFloat("_Visibility", visibility);
            materialInstance.SetColor("_Color", volumeColor);
            materialInstance.SetTexture("_MainTex", texture);
        }
    }
}