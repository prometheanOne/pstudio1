using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(InspectionOrbiter))]
public class InspectionOrbiterEditor : Editor {

    public override void OnInspectorGUI()
    {
        InspectionOrbiter orbiter = target as InspectionOrbiter;
        
        if (orbiter != null)
        {
            var categoryStyle = new GUIStyle();
            categoryStyle.fontStyle = FontStyle.Bold;

            EditorGUILayout.PrefixLabel("Properties", GUIStyle.none, categoryStyle);

            orbiter.AllowOrbiter = EditorGUILayout.Toggle("Allow Orbiter", orbiter.AllowOrbiter);
            orbiter.RecenterOnClick = EditorGUILayout.Toggle("Recenter On Click", orbiter.RecenterOnClick);
			orbiter.RecenterOnSpace = EditorGUILayout.Toggle("Recenter On Space", orbiter.RecenterOnSpace);
            var temp = (InspectorObject)EditorGUILayout.ObjectField("Inspector Object", orbiter.InspectorObject,
                                                                  typeof(InspectorObject), true);
            if (temp != orbiter.InspectorObject)
            {
                orbiter.InspectorObject = temp;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("InvalidGrabLayers"));
            EditorGUILayout.Separator();

            EditorGUILayout.PrefixLabel("Keyboard Controls", GUIStyle.none, categoryStyle);
            orbiter.UseKeyboardInput = EditorGUILayout.Toggle("Use Keyboard Input", orbiter.UseKeyboardInput);
            //orbiter.Forward[0] = (KeyCode)EditorGUILayout.EnumPopup("Forward", orbiter.Forward[0]);
            //orbiter.Forward[1] = (KeyCode)EditorGUILayout.EnumPopup("Alt Forward", orbiter.Forward[1]);
            //orbiter.Backward[0] = (KeyCode)EditorGUILayout.EnumPopup("Backward", orbiter.Backward[0]);
            //orbiter.Backward[1] = (KeyCode)EditorGUILayout.EnumPopup("Alt Backward", orbiter.Backward[1]);
            //orbiter.StrafeLeft[0] = (KeyCode)EditorGUILayout.EnumPopup("Strafe Left", orbiter.StrafeLeft[0]);
            //orbiter.StrafeLeft[1] = (KeyCode)EditorGUILayout.EnumPopup("Alt Strafe Left", orbiter.StrafeLeft[1]);
            //orbiter.StrafeRight[0] = (KeyCode)EditorGUILayout.EnumPopup("Strafe Right", orbiter.StrafeRight[0]);
            //orbiter.StrafeRight[1] = (KeyCode)EditorGUILayout.EnumPopup("Alt Strafe Right", orbiter.StrafeRight[1]);
            //orbiter.TurnLeft[0] = (KeyCode)EditorGUILayout.EnumPopup("Turn Left", orbiter.TurnLeft[0]);
            //orbiter.TurnLeft[1] = (KeyCode)EditorGUILayout.EnumPopup("Alt Turn Left", orbiter.TurnLeft[1]);
            //orbiter.TurnRight[0] = (KeyCode)EditorGUILayout.EnumPopup("Turn Right", orbiter.TurnRight[0]);
            //orbiter.TurnRight[1] = (KeyCode)EditorGUILayout.EnumPopup("Alt Turn Right", orbiter.TurnRight[1]);
            EditorGUILayout.Separator();

            
            EditorGUILayout.PrefixLabel("Speeds", GUIStyle.none, categoryStyle);

            orbiter.XRotationSpeed = EditorGUILayout.FloatField("X Rotation Speed", orbiter.XRotationSpeed);
            orbiter.YRotationSpeed = EditorGUILayout.FloatField("Y Rotation Speed", orbiter.YRotationSpeed);
            orbiter.ZoomSpeed = EditorGUILayout.FloatField("Zoom Speed", orbiter.ZoomSpeed);
            orbiter.TransitionTime = EditorGUILayout.FloatField("Transition Time", orbiter.TransitionTime);
            EditorGUILayout.Separator();

            EditorGUILayout.PrefixLabel("Mouse Properties", GUIStyle.none, categoryStyle);
            orbiter.UseMouseInput = EditorGUILayout.Toggle("Use Mouse Input", orbiter.UseMouseInput);
            orbiter.XMouseMultiplier = EditorGUILayout.FloatField("X Mouse Multiplier", orbiter.XMouseMultiplier);
            orbiter.YMouseMultiplier = EditorGUILayout.FloatField("Y Mouse Multiplier", orbiter.YMouseMultiplier);
            orbiter.MouseScrollZoomSpeed = EditorGUILayout.FloatField("Mouse Scroll Zoom Speed", orbiter.MouseScrollZoomSpeed);
            orbiter.MouseZoomSpeed = EditorGUILayout.FloatField("Mouse Zoom Speed", orbiter.MouseZoomSpeed);
            EditorGUILayout.Separator();

            EditorGUILayout.PrefixLabel("Sensitivity", GUIStyle.none, categoryStyle);
            orbiter.PanSensitivity = EditorGUILayout.FloatField("Pan Sensitivity", orbiter.PanSensitivity);
            orbiter.RotationSensitivity = EditorGUILayout.FloatField("Rotation Sensitivity", orbiter.RotationSensitivity);
			orbiter.PinchZoomSensitivity = EditorGUILayout.FloatField ("Pinch Zoom Sensitivity", orbiter.PinchZoomSensitivity);
            EditorGUILayout.Separator();

            EditorGUILayout.PrefixLabel("Defaults", GUIStyle.none, categoryStyle);
            orbiter.MaxVerticalRotation = EditorGUILayout.IntSlider("Vertical Max Rotation", orbiter.MaxVerticalRotation, -90, 90);
            orbiter.MinVerticalRotation = EditorGUILayout.IntSlider("Vertical Min Rotation", orbiter.MinVerticalRotation, -90, 90);
            orbiter.MaxHorizontalRotation = EditorGUILayout.IntSlider("Horizontal Max Rotation", orbiter.MaxHorizontalRotation, -180, 180);
            orbiter.MinHorizontalRotation = EditorGUILayout.IntSlider("Horizontal Min Rotation", orbiter.MinHorizontalRotation, -180, 180);
            orbiter.MaxDistance = EditorGUILayout.IntField("Zoom Max", orbiter.MaxDistance);
            orbiter.MinDistance = EditorGUILayout.IntField("Zoom Min", orbiter.MinDistance);
            orbiter.OffsetFrustumWidth = EditorGUILayout.FloatField("Offset Frustum Width", orbiter.OffsetFrustumWidth);

        }
		if (GUI.changed) EditorUtility.SetDirty(orbiter);
    }

    [MenuItem("GameObject/Create Other/Interplay/Inspector")]
    public static void CreateInspector()
    {
        GameObject go = new GameObject("Inspector");
        go.AddComponent<InspectionOrbiter>();
        go.AddComponent<CharacterController>();
        go.AddComponent<CharacterMotor>();
        go.AddComponent<FPSInputControllerKB>();
        var fps = go.GetComponent<FPSInputControllerKB>();
        fps.moveSensitivity = 0.4f;
        fps.turnSensitivity = 2;
        if (Camera.main == null)
        {
            GameObject camera = new GameObject("Main Camera");
            camera.AddComponent<Camera>();
            camera.tag = "MainCamera";
        }
        Camera.main.depth = -1;
        Camera.main.gameObject.AddComponent<UICamera>();
        Camera.main.gameObject.transform.parent = go.transform;
    }

    public void OnSceneGUI()
    {
        InspectionOrbiter orbiter = target as InspectionOrbiter;
        if (orbiter != null)
        {
            // Special handles and such would go here.
        }
    }
}
