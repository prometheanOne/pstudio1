using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using AdvancedSS;

public class ShaderGenerator : EditorWindow
{
    private string shaderDirectory = "AdvancedSurfaceShaders/Shaders/";

    List<AdvancedShader.ShaderEffect> ignoreEffects = new List<AdvancedShader.ShaderEffect>();
    List<AdvancedShader.ShaderEffect> nonMobileEffects = new List<AdvancedShader.ShaderEffect>();
    Dictionary<AdvancedShader.ShaderEffect, List<AdvancedShader.ShaderEffect>> cantCombineEffects = new Dictionary<AdvancedShader.ShaderEffect, List<AdvancedShader.ShaderEffect>>();
    List<List<AdvancedShader.ShaderEffect>> ignoreCombinations = new List<List<AdvancedShader.ShaderEffect>>();

    Dictionary<int, bool> selectedEffects = new Dictionary<int, bool>();

    AdvancedShader.ShaderEffect[] allEffectsArray = new AdvancedShader.ShaderEffect[0];

    bool mobile = false;

    int queueOffset = 0;

    bool transparent = false;

    bool transparentCutout = false;

    Vector2 effectsScrollPosition = Vector2.zero;

    List<AdvancedShader.ShaderEffect[]> foundPermutations = new List<AdvancedShader.ShaderEffect[]>();

    AdvancedShader exactMatchShader = null;

    string exactEffectConflicts = "";

    [MenuItem("Window/Advanced Surface Shaders/Shader Generator")]
    static void Init()
    {
        ShaderGenerator window = (ShaderGenerator)EditorWindow.GetWindow(typeof(ShaderGenerator));
        window.Setup();
        window.Show();
        window.PreSetup();
    }

    public void PreSetup()
    {
        allEffectsArray = (AdvancedShader.ShaderEffect[])System.Enum.GetValues(typeof(AdvancedShader.ShaderEffect));
        for (int i = 0; i < allEffectsArray.Length; i++)
        {
            selectedEffects[i] = false;
        }
    }

    public void Setup()
    {
        nonMobileEffects.Add(AdvancedShader.ShaderEffect.ADVANCEDSS_POM);
        nonMobileEffects.Add(AdvancedShader.ShaderEffect.ADVANCEDSS_RELIEF);
        nonMobileEffects.Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG);
        nonMobileEffects.Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED);
        nonMobileEffects.Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE);
        nonMobileEffects.Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH);

        AdvancedShader.ShaderEffect[] effectsArray = (AdvancedShader.ShaderEffect[])System.Enum.GetValues(typeof(AdvancedShader.ShaderEffect));
        for (int i = 0; i < effectsArray.Length; i++)
        {
            cantCombineEffects[effectsArray[i]] = new List<AdvancedShader.ShaderEffect>();
        }

        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_SPECMAP].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_SPECULAR);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_SPECMAP].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_GLOSSMAP);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_RIM].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_RIM].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_RIM].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_RIM].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_BUMP].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_PARALLAX);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_BUMP].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_POM);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_BUMP].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_RELIEF);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_PARALLAX].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_POM);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_PARALLAX].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_RELIEF);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_PARALLAX].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_PARALLAX].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_PARALLAX].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_PARALLAX].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_POM].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_RELIEF);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_POM].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_POM].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_POM].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_POM].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_RELIEF].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_RELIEF].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_RELIEF].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_RELIEF].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH);
        cantCombineEffects[AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE].Add(AdvancedShader.ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH);

        // Ignore Built-In Combos
        /*List<AdvancedShader.ShaderEffect> bump = new List<AdvancedShader.ShaderEffect>();
        bump.Add(AdvancedShader.ShaderEffect.ADVANCEDSS_BUMP);
        bump.Sort();
        ignoreCombinations.Add(bump);

        List<AdvancedShader.ShaderEffect> bumpSpecular = new List<AdvancedShader.ShaderEffect>();
        bumpSpecular.Add(AdvancedShader.ShaderEffect.ADVANCEDSS_BUMP);
        bumpSpecular.Add(AdvancedShader.ShaderEffect.ADVANCEDSS_SPECULAR);
        bumpSpecular.Sort();
        ignoreCombinations.Add(bumpSpecular);

        List<AdvancedShader.ShaderEffect> parallax = new List<AdvancedShader.ShaderEffect>();
        parallax.Add(AdvancedShader.ShaderEffect.ADVANCEDSS_PARALLAX);
        parallax.Sort();
        ignoreCombinations.Add(parallax);

        List<AdvancedShader.ShaderEffect> parallaxSpecular = new List<AdvancedShader.ShaderEffect>();
        parallaxSpecular.Add(AdvancedShader.ShaderEffect.ADVANCEDSS_PARALLAX);
        parallaxSpecular.Add(AdvancedShader.ShaderEffect.ADVANCEDSS_SPECULAR);
        parallaxSpecular.Sort();
        ignoreCombinations.Add(parallaxSpecular);*/
    }

    void OnGUI()
    {
        bool hasChanged = false;
        bool current = false;

        Color previousColour = GUI.contentColor;
        GUI.contentColor = new Color(0.7f, 0.7f, 0.7f, 1f);
        GUILayout.Label("Select Required Shader Effects");
        GUI.contentColor = previousColour;

        GUILayout.Space(5f);

        GUIStyle scrollViewStyle = new GUIStyle(GUI.skin.GetStyle("ScrollView"));
        scrollViewStyle.margin = new RectOffset(5, 5, 5, 5);

        effectsScrollPosition = EditorGUILayout.BeginScrollView(effectsScrollPosition, scrollViewStyle);
        int numSelectedEffects = 0;
        for (int i = 0; i < allEffectsArray.Length; i++)
        {
            if (!selectedEffects.ContainsKey(i))
            {
                PreSetup();
                return;
            }
            current = selectedEffects[i];
            if (mobile && nonMobileEffects.Contains(allEffectsArray[i]))
            {
                selectedEffects[i] = false;
                if (current) hasChanged = true;
                EditorGUI.BeginDisabledGroup(true);
            }
            selectedEffects[i] = GUILayout.Toggle(selectedEffects[i], AdvancedShader.shaderEffectNames[i]);
            if (current != selectedEffects[i])
            {
                hasChanged = true;
            }
            if (selectedEffects[i]) numSelectedEffects++;
            if (mobile && nonMobileEffects.Contains(allEffectsArray[i]))
            {
                selectedEffects[i] = false;
                EditorGUI.EndDisabledGroup();
            }
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Space(20f);

        mobile = GUILayout.Toggle(mobile, "Mobile");
        if (exactMatchShader != null && exactMatchShader.mobile != mobile)
        {
            if (exactMatchShader != null)
            {
                exactMatchShader.mobile = mobile;
            }
        }

        transparent = GUILayout.Toggle(transparent, "Transparent");
        if (exactMatchShader != null && exactMatchShader.transparent != transparent)
        {
            if (exactMatchShader != null)
            {
                exactMatchShader.transparent = transparent;
                if (transparent)
                {
                    exactMatchShader.transparentCutout = false;
                }
                exactMatchShader.SetQueueForTransparency();
            }
        }

        transparentCutout = GUILayout.Toggle(transparentCutout, "Transparent Cutout");
        if (exactMatchShader != null && exactMatchShader.transparentCutout != transparentCutout)
        {
            if (exactMatchShader != null)
            {

                exactMatchShader.transparentCutout = transparentCutout;
                if (transparentCutout)
                {
                    exactMatchShader.transparent = false;
                }
                exactMatchShader.SetQueueForTransparency();
            }
        }

        EditorGUILayout.HelpBox("Leave this as 0 if unsure.", MessageType.Info, true);
        int currentQueueOffset = queueOffset;
        queueOffset = EditorGUILayout.IntField("Queue Offset", queueOffset);
        if (currentQueueOffset != queueOffset)
        {
            exactMatchShader.queueOffset = queueOffset;
        }

        if (hasChanged)
        {
            FindShaderFromCurrentOptions();
            if (numSelectedEffects < 9) FindPermutationFromCurrentOptions();
        }

        bool create = false;
        bool specificShader = false;

        EditorGUILayout.Separator();

        if (exactMatchShader == null && foundPermutations.Count == 0)
        {
            EditorGUILayout.HelpBox("Select shader effects from the list above", MessageType.None, true);
        }
        else
        {
            GUILayout.Label("Generate Exact Shader:");
            if (exactMatchShader == null)
            {
                EditorGUILayout.HelpBox("Can't create this specific shader. Conflicting effects: " + exactEffectConflicts, MessageType.Error, true);
            }
            else
            {
                EditorGUILayout.HelpBox("This will generate the shader \"" + exactMatchShader.ShaderName() + "\"", MessageType.Info, true);

                if (GUILayout.Button("Create \"" + exactMatchShader.ShaderName() + "\" shader"))
                {
                    create = true;
                    specificShader = true;
                }
            }

            EditorGUILayout.Separator();

            GUILayout.Label("Generate All Selected Shader Effect Permutations:");
            MessageType permMessageType = MessageType.Info;
            string permutationStringEnd = foundPermutations.Count.ToString() + " unique shaders.";
            if (foundPermutations.Count == 1) permutationStringEnd = foundPermutations.Count.ToString() + " unique shader.";
            if (numSelectedEffects >= 9)
            {
                permutationStringEnd = "over one hundred unique shaders. It may take a few minutes.";
                permMessageType = MessageType.Warning;
            }

            EditorGUILayout.HelpBox("This will generate " + permutationStringEnd, permMessageType, true);

            if (GUILayout.Button("Create all (" + foundPermutations.Count.ToString() + ") selected shader permutations"))
            {
                create = true;
                specificShader = false;
            }

            if (create)
            {
                if (specificShader && exactMatchShader != null)
                {
                    exactMatchShader.CreateShaderFile(shaderDirectory);
                }
                else
                {
                    for (int i = 0; i < foundPermutations.Count; i++)
                    {
                        AdvancedShader newShader = new AdvancedShader();

                        newShader.mobile = mobile;

                        for (int j = 0; j < foundPermutations[i].Length; j++)
                        {
                            newShader.shaderEffects.Add(foundPermutations[i][j]);
                        }

                        // Create opaque, alpha and alphatest shaders
                        //newShader.CreateShaderFile(shaderDirectory);

                        newShader.transparentCutout = transparentCutout;
                        newShader.transparent = transparent;
                        newShader.SetQueueForTransparency();

                        newShader.CreateShaderFile(shaderDirectory);
                    }
                }

                AssetDatabase.Refresh();
            }
        }
    }

    void FindShaderFromCurrentOptions()
    {
        Setup();

        exactMatchShader = null;
        exactEffectConflicts = "";

        //List<AdvancedShader.ShaderEffect> selectedEffectsList = new List<AdvancedShader.ShaderEffect>();
        //int[] selectedEffectsIndices = new int[selectedEffects.Count];
        List<int> selectedEffectsIndicesList = new List<int>();
        for (int i = 0; i < allEffectsArray.Length; i++)
        {
            //if (selectedEffects[i]) selectedEffectsList.Add(allEffectsArray[i]);
            if (selectedEffects[i])
            {
                //selectedEffectsIndices[c] = i;
                selectedEffectsIndicesList.Add(i);
            }
        }
        int[] selectedEffectsIndices = selectedEffectsIndicesList.ToArray();

        bool valid = true;

        for (int i = 0; i < selectedEffectsIndices.Length; i++)
        {
            AdvancedShader.ShaderEffect effectI = allEffectsArray[selectedEffectsIndices[i]];
            if (ignoreEffects.Contains(effectI))
            {
                valid = false;
                break;
            }

            for (int j = 0; j < selectedEffectsIndices.Length; j++)
            {
                AdvancedShader.ShaderEffect effectJ = allEffectsArray[selectedEffectsIndices[j]];

                if (i == j) continue;
                if (cantCombineEffects[effectI].Contains(effectJ))
                {
                    exactEffectConflicts = AdvancedShader.shaderEffectNames[selectedEffectsIndices[i]] + " with " + AdvancedShader.shaderEffectNames[selectedEffectsIndices[j]];
                    valid = false;
                    break;
                }
                if (cantCombineEffects[effectJ].Contains(effectI))
                {
                    exactEffectConflicts = AdvancedShader.shaderEffectNames[selectedEffectsIndices[i]] + " with " + AdvancedShader.shaderEffectNames[selectedEffectsIndices[j]];
                    valid = false;
                    break;
                }
                if (ignoreEffects.Contains(effectJ))
                {
                    valid = false;
                    break;
                }
            }

            if (!valid) break;
        }

        if (valid && selectedEffectsIndices.Length > 0)
        {
            exactMatchShader = new AdvancedShader();

            for (int k = 0; k < selectedEffectsIndices.Length; k++)
            {
                exactMatchShader.shaderEffects.Add(allEffectsArray[selectedEffectsIndices[k]]);
            }
        }
    }

    void FindPermutationFromCurrentOptions()
    {
        Setup();

        List<AdvancedShader.ShaderEffect> selectedEffectsList = new List<AdvancedShader.ShaderEffect>();
        for (int i = 0; i < allEffectsArray.Length; i++)
        {
            if (selectedEffects[i]) selectedEffectsList.Add(allEffectsArray[i]);
        }
        AdvancedShader.ShaderEffect[] effectsArray = selectedEffectsList.ToArray();
        selectedEffectsList.Sort();

        List<string> foundPermutationNames = new List<string>();
        foundPermutations.Clear();

        List<AdvancedShader.ShaderEffect> alreadyCheckedEffects = new List<AdvancedShader.ShaderEffect>();

        for (int i = 0; i < effectsArray.Length; i++)
        {
            if (ignoreEffects.Contains(effectsArray[i])) continue;
            
            List<AdvancedShader.ShaderEffect> validCombineEffects = new List<AdvancedShader.ShaderEffect>();
            validCombineEffects.Add(effectsArray[i]);

            for (int j = 0; j < effectsArray.Length; j++)
            {
                if (alreadyCheckedEffects.Contains(effectsArray[j])) continue;
                if (i == j) continue;
                if (cantCombineEffects[effectsArray[i]].Contains(effectsArray[j]))
                {
                    continue;
                }
                if (cantCombineEffects[effectsArray[j]].Contains(effectsArray[i])) continue;
                if (ignoreEffects.Contains(effectsArray[j])) continue;

                validCombineEffects.Add(effectsArray[j]);
            }

            alreadyCheckedEffects.Add(effectsArray[i]);

            for (int j = 1; j <= validCombineEffects.Count; j++)
            {
                //List<AdvancedShader.ShaderEffect> perms = PermuteUtils.Permute<AdvancedShader.ShaderEffect>(validCombineEffects, j);
                //IEnumerable<IEnumerable<AdvancedShader.ShaderEffect>> perms = PermuteUtils.Permute<AdvancedShader.ShaderEffect>(validCombineEffects, j);

                //for (int l = 0; l < perms.Count; l++)
                foreach (IEnumerable<AdvancedShader.ShaderEffect> perms in PermuteUtils.Permute<AdvancedShader.ShaderEffect>(validCombineEffects, j))
                {
                    bool ignorePerm = false;
                    List<AdvancedShader.ShaderEffect> permutationList = new List<AdvancedShader.ShaderEffect>();
                    permutationList.AddRange(perms);
                    permutationList.Sort();
                    string permEffectNames = "";
                    for (int k = 0; k < permutationList.Count; k++)
                    {
                        for (int l = 0; l < permutationList.Count; l++)
                        {
                            if (cantCombineEffects[permutationList[k]].Contains(permutationList[l]))
                            {
                                ignorePerm = true;
                                break;
                            }
                        }
                        for (int p = 0; p < ignoreCombinations.Count; p++)
                        {
                            if (ignoreCombinations[p].Count == permutationList.Count)
                            {
                                bool noMatch = false;
                                for (int o = 0; o < ignoreCombinations[p].Count; o++)
                                {
                                    if (ignoreCombinations[p][o] != permutationList[o])
                                    {
                                        noMatch = true;
                                        break;
                                    }
                                }
                                if (!noMatch)
                                {
                                    ignorePerm = true;
                                    break;
                                }
                            }
                        }
                        if (ignorePerm) break;
                        permEffectNames += System.Enum.GetName(typeof(AdvancedShader.ShaderEffect), permutationList[k]).Replace("ADVANCEDSS_", "");
                    }
                    if (ignorePerm) continue;

                    if (!foundPermutationNames.Contains(permEffectNames))
                    {
                        foundPermutationNames.Add(permEffectNames);

                        //// Check if this is exactly the same as the selected option
                        //bool exactMatch = false;
                        //if (selectedEffectsList.Count == permutationList.Count)
                        //{
                        //    exactMatch = true;
                        //    for (int k = 0; k < selectedEffectsList.Count; k++)
                        //    {
                        //        if (selectedEffectsList[k] != permutationList[k])
                        //        {
                        //            exactMatch = false;
                        //            break;
                        //        }
                        //    }

                        //    if (exactMatch)
                        //    {
                        //        exactMatchShader = new AdvancedShader();

                        //        for (int k = 0; k < selectedEffectsList.Count; k++)
                        //        {
                        //            exactMatchShader.shaderEffects.Add(selectedEffectsList[k]);
                        //        }
                        //    }
                        //}
                        foundPermutations.Add(permutationList.ToArray());
                    }
                }
            }
        }
    }
}