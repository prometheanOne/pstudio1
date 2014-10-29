using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(InspectorObject))]
public class InspectorObjectEditor : Editor
{
    /// <summary>
    /// Inspector code for Objects being inspected.
    /// </summary>
    public override void OnInspectorGUI()
    {
        InspectorObject obj = target as InspectorObject;
        if (obj != null)
        {
            var categoryStyle = new GUIStyle();
            categoryStyle.fontStyle = FontStyle.Bold;

            EditorGUILayout.PrefixLabel("Properties", GUIStyle.none, categoryStyle);
            obj.StartDistance = EditorGUILayout.FloatField("Starting Distance", obj.StartDistance);
            EditorGUILayout.Separator();

            EditorGUILayout.PrefixLabel("Horizontal Properties", GUIStyle.none, categoryStyle);
            obj.UseHorizontalRotation = EditorGUILayout.Toggle("Use Horizontal Rotations", obj.UseHorizontalRotation);
            obj.MaxHorizontalRotation = EditorGUILayout.IntSlider("Horizontal Max Rotation", obj.MaxHorizontalRotation, -360, 360);
            obj.MinHorizontalRotation = EditorGUILayout.IntSlider("Horizontal Min Rotation", obj.MinHorizontalRotation, -360, 360);
            EditorGUILayout.Separator();

            EditorGUILayout.PrefixLabel("Vertical Properties", GUIStyle.none, categoryStyle);
            obj.UseVerticalRotation = EditorGUILayout.Toggle("Use Vertical Rotations", obj.UseVerticalRotation);
            obj.MaxVerticalRotation = EditorGUILayout.IntSlider("Vertical Max Rotation", obj.MaxVerticalRotation, -90, 90);
            obj.MinVerticalRotation = EditorGUILayout.IntSlider("Vertical Min Rotation", obj.MinVerticalRotation, -90, 90);
            EditorGUILayout.Separator();

            EditorGUILayout.PrefixLabel("Zoom Properties", GUIStyle.none, categoryStyle);
            obj.UseZoom = EditorGUILayout.Toggle("Use Custom Zoom", obj.UseZoom);
            obj.MaxDistance = EditorGUILayout.FloatField("Zoom Max", obj.MaxDistance);
            obj.MinDistance = EditorGUILayout.FloatField("Zoom Min", obj.MinDistance);
			EditorGUILayout.Separator();

			EditorGUILayout.PrefixLabel ("Touch Properties", GUIStyle.none, categoryStyle);
			obj.PanX = EditorGUILayout.BeginToggleGroup ("Pan X-axis",obj.PanX);
				obj.PanBoundsX = EditorGUILayout.Vector2Field ("Pan Bounds",obj.PanBoundsX);
			EditorGUILayout.EndToggleGroup ();
			obj.PanY = EditorGUILayout.BeginToggleGroup ("Pan Y-axis",obj.PanY);
				obj.PanBoundsY = EditorGUILayout.Vector2Field ("Pan Bounds",obj.PanBoundsY);
			EditorGUILayout.EndToggleGroup ();
			obj.PanZ = EditorGUILayout.BeginToggleGroup ("Pan Z-axis",obj.PanZ);
				obj.PanBoundsZ = EditorGUILayout.Vector2Field ("Pan Bounds",obj.PanBoundsZ);
			EditorGUILayout.EndToggleGroup ();
        }
    }
}
