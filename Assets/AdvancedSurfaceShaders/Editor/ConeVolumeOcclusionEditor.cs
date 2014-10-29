using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(ConeVolumeOcclusionEditor))]
public class ConeVolumeOcclusionEditor : UnityEditor.Editor
{
	public override void OnInspectorGUI () {
		DrawDefaultInspector();
		EditorGUILayout.HelpBox ("The Cone Volume Occlusion is a work in progress in this version.", MessageType.Warning ); 
	}
}
