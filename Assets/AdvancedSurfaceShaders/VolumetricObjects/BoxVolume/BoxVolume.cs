using UnityEngine;

[ExecuteInEditMode]
public class BoxVolume : VolumetricObjectBase
{
    public Vector3 boxSize = Vector3.one * 5f;
    private Vector3 previousBoxSize = Vector3.one;

#if UNITY_EDITOR
    [UnityEditor.MenuItem("GameObject/Create Other/Volumetric Objects/Box Volume")]
    static public void CreateVolume()
    {
        GameObject newObject = new GameObject("Box Volume");
        if (UnityEditor.SceneView.currentDrawingSceneView) UnityEditor.SceneView.currentDrawingSceneView.MoveToView(newObject.transform);
        BoxVolume boxVolume = (BoxVolume)newObject.AddComponent<BoxVolume>();
        boxVolume.volumeShader = "Advanced SS/Volumetric/Box Volume";
        boxVolume.enabled = false;
        boxVolume.enabled = true;
    }
#endif

    protected override void OnEnable()
    {
        if (volumeShader == "") PopulateShaderName();
        base.OnEnable();
    }

    public override void PopulateShaderName()
    {
        volumeShader = "Advanced SS/Volumetric/Box Volume";
    }

    public override bool HasChanged()
    {
        if (boxSize != previousBoxSize || base.HasChanged())
        {
            return true;
        }
        return false;
    }

    protected override void SetChangedValues()
    {
        previousBoxSize = boxSize;
        base.SetChangedValues();
    }

    public override void UpdateVolume()
    {
        Vector3 halfBoxSize = boxSize * 0.5f;

        if (meshInstance)
        {
            ScaleMesh(meshInstance, boxSize);

            // Set bounding volume so modified vertices don't get culled
            Bounds bounds = new Bounds();
            bounds.SetMinMax(-halfBoxSize, halfBoxSize);
            meshInstance.bounds = bounds;
        }

        if (materialInstance)
        {
            materialInstance.SetVector("_BoxMin", new Vector4(-halfBoxSize.x, -halfBoxSize.y, -halfBoxSize.z, 0f));
            materialInstance.SetVector("_BoxMax", new Vector4(halfBoxSize.x, halfBoxSize.y, halfBoxSize.z, 0f));
            materialInstance.SetVector("_TextureData", new Vector4(-textureMovement.x, -textureMovement.y, -textureMovement.z, (1f / textureScale)));
            materialInstance.SetFloat("_Visibility", visibility);
            materialInstance.SetColor("_Color", volumeColor);
            materialInstance.SetTexture("_MainTex", texture);
        }
    }
}