using UnityEngine;

[ExecuteInEditMode]
public class ConeVolume : VolumetricObjectBase
{
    public float coneHeight = 2f;
    public float coneAngle = 30f;
    public float startOffset = 0f;

    private float previousConeHeight = 0f;
    private float previousConeAngle = 0f;
    private float previousStartOffset = 0f;

#if UNITY_EDITOR
    [UnityEditor.MenuItem("GameObject/Create Other/Volumetric Objects/Cone Volume")]
    static public void CreateVolume()
    {
        GameObject newObject = new GameObject("Cone Volume");
        if (UnityEditor.SceneView.currentDrawingSceneView) UnityEditor.SceneView.currentDrawingSceneView.MoveToView(newObject.transform);
        ConeVolume coneVolume = (ConeVolume)newObject.AddComponent<ConeVolume>();
        coneVolume.volumeShader = "Advanced SS/Volumetric/Cone Volume";
        coneVolume.enabled = false;
        coneVolume.enabled = true;
    }
#endif

    protected override void OnEnable()
    {
        if (volumeShader == "") PopulateShaderName();
        base.OnEnable();
    }

    public override void PopulateShaderName()
    {
        volumeShader = "Advanced SS/Volumetric/Cone Volume";
    }

    public override bool HasChanged()
    {
        if (coneHeight != previousConeHeight ||
            coneAngle != previousConeAngle ||
            startOffset != previousStartOffset || 
            base.HasChanged())
        {
            return true;
        }
        return false;
    }

    protected override void SetChangedValues()
    {
        if (coneHeight < 0f) coneHeight = 0f;
        if (coneAngle >= 89f) coneAngle = 89f;
        previousConeHeight = coneHeight;
        previousConeAngle = coneAngle;
        previousStartOffset = startOffset;
        base.SetChangedValues();
    }

    public override void UpdateVolume()
    {
        float angleRads = coneAngle * Mathf.Deg2Rad;
        float bottomRadius = Mathf.Tan(angleRads) * coneHeight;
        float bottomRadiusHalf = bottomRadius * 0.5f;

        Vector3 halfBoxSize = new Vector3(bottomRadius, coneHeight, bottomRadius);

        if (meshInstance)
        {
            ScaleMesh(meshInstance, halfBoxSize, -Vector3.up * coneHeight * 0.5f);

            // Set bounding volume so modified vertices don't get culled
            Bounds bounds = new Bounds();
            bounds.SetMinMax(-halfBoxSize, halfBoxSize);
            meshInstance.bounds = bounds;
        }

        if (materialInstance)
        {
            materialInstance.SetVector("_ConeData", new Vector4(bottomRadiusHalf, coneHeight, startOffset, Mathf.Cos(angleRads)));
            materialInstance.SetVector("_TextureData", new Vector4(-textureMovement.x, -textureMovement.y, -textureMovement.z, (1f / textureScale)));
            materialInstance.SetFloat("_Visibility", visibility);
            materialInstance.SetColor("_Color", volumeColor);
            materialInstance.SetTexture("_MainTex", texture);
        }
    }
}