using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif
using System.Collections.Generic;

namespace AdvancedSS
{
    public class AdvancedShader
    {
        public enum ShaderEffect
	    {
            ADVANCEDSS_SPECULAR,
            ADVANCEDSS_SPECMAP,
            ADVANCEDSS_GLOSSMAP,
            ADVANCEDSS_BUMP,
            ADVANCEDSS_PARALLAX,
            ADVANCEDSS_POM,
            ADVANCEDSS_RELIEF,
            ADVANCEDSS_RIM,
            ADVANCEDSS_EMISSIVE,
            ADVANCEDSS_CUBEREFLECTION,
            ADVANCEDSS_LIGHTINGRAMP,
            ADVANCEDSS_TESSELLATIONPHONG,
            ADVANCEDSS_TESSELLATIONMAPFIXED,
            ADVANCEDSS_TESSELLATIONMAPDISTANCE,
            ADVANCEDSS_TESSELLATIONMAPEDGELENGTH
	    }

        public enum ShaderQuality
        {
            HIGH,
            MEDIUM,
            LOW
        }

        public static string[] shaderEffectNames = new string[15]
        {
            "Specular",
            "Spec Map",
            "Gloss Map",
            "Normal Map",
            "Parallax Map",
            "Parallax Occlusion Map",
            "Relaxed Cone Stepped Relief Map",
            "Rim Lighting",
            "Emissive Map",
            "Cubemap Reflection",
            "Lighting Ramp (Stylised lighting)",
            "Tessellation (Phong)",
            "Tessellation (Fixed)",
            "Tessellation (Distance)",
            "Tessellation (Edge Length)"
        };

        public List<ShaderEffect> shaderEffects = new List<ShaderEffect>();

        public bool mainTex = true;
        public bool color = true;
        public bool transparent = false;
        public bool transparentCutout = false;
        public string queue = "Geometry";
        public int queueOffset = 0;
        public ShaderQuality quality = ShaderQuality.HIGH;
        public bool mobile = false;

        public int subShader1LOD = 500;
        public int subShader2LOD = 250;

        public void SetQueueForTransparency()
        {
            if (transparent) queue = "Transparent";
            else if (transparentCutout) queue = "AlphaTest";
            else queue = "Geometry";
        }

        public string ShaderFileName()
        {
            return ShaderFileName(true);
        }

        public string ShaderFileName(bool includeExtension)
        {
            string shaderName = "";

            if (mobile) shaderName += "Mobile/";

            if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG)) shaderName += "TessellationPhong/";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED)) shaderName += "TessellationMapFixed/";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE)) shaderName += "TessellationMapDistance/";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH)) shaderName += "TessellationMapEdgeLength/";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_PARALLAX)) shaderName += "Parallax/";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_RELIEF)) shaderName += "RCSM/";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_POM)) shaderName += "ParallaxOcclusion/";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_BUMP)) shaderName += "Bump/";
            else shaderName += "Standard/";

            if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG) || HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED) || HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE) || HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH))
            {
                if (HasEffect(ShaderEffect.ADVANCEDSS_PARALLAX)) shaderName += "Parallax/";
                else if (HasEffect(ShaderEffect.ADVANCEDSS_RELIEF)) shaderName += "RCSM/";
                else if (HasEffect(ShaderEffect.ADVANCEDSS_POM)) shaderName += "ParallaxOcclusion/";
                else if (HasEffect(ShaderEffect.ADVANCEDSS_BUMP)) shaderName += "Bump/";
            }

            if (mobile) shaderName += "Mobile-";

            if (transparent) shaderName += "Alpha-";
            else if (transparentCutout) shaderName += "AlphaTest-";

            if (HasEffect(ShaderEffect.ADVANCEDSS_CUBEREFLECTION)) shaderName += "Reflect-";

            if (HasEffect(ShaderEffect.ADVANCEDSS_PARALLAX)) shaderName += "Parallax";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_RELIEF)) shaderName += "RCSM";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_POM)) shaderName += "ParallaxOcclusion";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_BUMP)) shaderName += "Bumped";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG)) shaderName += "TessellationPhong";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED)) shaderName += "TessellationMapFixed";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE)) shaderName += "TessellationMapDistance";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH)) shaderName += "TessellationMapEdgeLength";
            else shaderName += "Diffuse";

            if (HasEffect(ShaderEffect.ADVANCEDSS_SPECMAP)) shaderName += "SpecMap";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_GLOSSMAP)) shaderName += "GlossMap";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_SPECULAR)) shaderName += "Specular";

            if (HasEffect(ShaderEffect.ADVANCEDSS_EMISSIVE)) shaderName += "Emissive";
            if (HasEffect(ShaderEffect.ADVANCEDSS_RIM)) shaderName += "Rim";

            if (HasEffect(ShaderEffect.ADVANCEDSS_LIGHTINGRAMP)) shaderName += "-Ramp";

            shaderName = shaderName.TrimEnd('-');

            if (includeExtension) shaderName += ".shader";

            return shaderName;
        }

        public string ShaderName()
        {
            string shaderName = "Advanced SS/";

            if (mobile) shaderName += "Mobile/";

            if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG)) shaderName += "Tessellation (Phong)/";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED)) shaderName += "Tessellation Map (Fixed)/";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE)) shaderName += "Tessellation Map (Distance)/";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH)) shaderName += "Tessellation Map (Edge Length)/";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_PARALLAX)) shaderName += "Parallax/";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_RELIEF)) shaderName += "Relaxed Cone Stepping/";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_POM)) shaderName += "Parallax Occlusion (D3D)/";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_BUMP)) shaderName += "Bump/";
            else shaderName += "Standard/";

            if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG) || HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED) || HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE) || HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH))
            {
                if (HasEffect(ShaderEffect.ADVANCEDSS_PARALLAX)) shaderName += "Parallax/";
                else if (HasEffect(ShaderEffect.ADVANCEDSS_RELIEF)) shaderName += "Relaxed Cone Stepping/";
                else if (HasEffect(ShaderEffect.ADVANCEDSS_POM)) shaderName += "Parallax Occlusion (D3D)/";
                else if (HasEffect(ShaderEffect.ADVANCEDSS_BUMP)) shaderName += "Bump/";
            }

            if (transparent) shaderName += "Transparent/";
            else if (transparentCutout) shaderName += "Transparent/Cutout/";

            if (HasEffect(ShaderEffect.ADVANCEDSS_CUBEREFLECTION)) shaderName += "Reflective/";

            if (HasEffect(ShaderEffect.ADVANCEDSS_SPECMAP)) shaderName += "SpecMap";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_GLOSSMAP)) shaderName += "GlossMap";
            else if (HasEffect(ShaderEffect.ADVANCEDSS_SPECULAR)) shaderName += "Specular";
            else shaderName += "Diffuse";

            if (HasEffect(ShaderEffect.ADVANCEDSS_EMISSIVE)) shaderName += " Emissive";
            if (HasEffect(ShaderEffect.ADVANCEDSS_RIM)) shaderName += " Rim";
            
            if (HasEffect(ShaderEffect.ADVANCEDSS_LIGHTINGRAMP)) shaderName += " Ramp";

            return shaderName;
        }

        public void CreateShaderFile(string directory)
        {
#if UNITY_EDITOR
            string relativeFilePath = Path.Combine(directory, ShaderFileName());
            string filePath = Path.Combine(Application.dataPath, relativeFilePath);
            string fileDirectory = (new FileInfo(filePath)).DirectoryName;

            // Make sure the shader directories exists
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
                if (!Directory.Exists(fileDirectory))
                {
                    Debug.LogError("Could not create shader directory '" + fileDirectory + "'");
                    return;
                }
            }

            // Write the Shader
            File.WriteAllText(filePath, GenerateShaderCode());

            AssetDatabase.ImportAsset(relativeFilePath);
#endif
        }

        public bool HasEffect(params ShaderEffect[] effects)
        {
            for (int i = 0; i < shaderEffects.Count; i++)
            {
                // Check if any of the effects are specular effects
                for (int j = 0; j < effects.Length; j++)
                {
                    if (shaderEffects[i] == effects[j])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public string GenerateShaderCode()
        {
            string shaderString = "";

            bool hasSpecular = HasEffect(ShaderEffect.ADVANCEDSS_SPECULAR, ShaderEffect.ADVANCEDSS_SPECMAP, ShaderEffect.ADVANCEDSS_GLOSSMAP);
            bool hasParallax = HasEffect(ShaderEffect.ADVANCEDSS_PARALLAX, ShaderEffect.ADVANCEDSS_POM, ShaderEffect.ADVANCEDSS_RELIEF);
            bool hasBumpMap = HasEffect(ShaderEffect.ADVANCEDSS_BUMP, ShaderEffect.ADVANCEDSS_PARALLAX, ShaderEffect.ADVANCEDSS_POM, ShaderEffect.ADVANCEDSS_RELIEF);
            bool hasSpecMap = HasEffect(ShaderEffect.ADVANCEDSS_SPECMAP, ShaderEffect.ADVANCEDSS_GLOSSMAP, ShaderEffect.ADVANCEDSS_PARALLAX, ShaderEffect.ADVANCEDSS_POM);
            bool hasTessellation = HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG, ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED, ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE, ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH);
            bool hasTessellationMap = HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED, ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE, ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH);

            string includePrefix = "";
            if (hasTessellation && hasBumpMap) includePrefix += "../";
            if (mobile) includePrefix += "../";
            
            // Start Shader and Properties
            shaderString += 
"Shader \"" + ShaderName() + "\"\r\n" +
"{\r\n" +
"    Properties\r\n" +
"    {\r\n";

            // Properties

            if (color && !mobile)
            {
                shaderString += 
"        _Color (\"Main Color\", Color) = (1,1,1,1)\r\n";
            }
            if (hasSpecular)
            {
                if (!mobile)
                {
                    shaderString +=
"        _SpecColor (\"Specular Color\", Color) = (0.5, 0.5, 0.5, 1)\r\n";
                }
                shaderString +=
"        _Shininess (\"Shininess\", Range (0.01, 1)) = 0.078125\r\n";
            }
            if (HasEffect(ShaderEffect.ADVANCEDSS_CUBEREFLECTION))
            {
                shaderString +=
"        _ReflectColor (\"Reflection Color\", Color) = (1,1,1,0.5)\r\n";
            }
            if (hasParallax)
            {
                shaderString += 
"        _Parallax (\"Height\", Range (0.005, 0.08)) = 0.08\r\n";
            }
            if (HasEffect(ShaderEffect.ADVANCEDSS_POM))
            {
                shaderString +=
"        _ParallaxSamples (\"Parallax Samples\", Range (10, 50)) = 40\r\n";
            }
            if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED) || HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE))
            {
                shaderString +=
"        _Tess (\"Tessellation\", Range(1,32)) = 4\r\n";
            }
            if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE))
            {
                shaderString +=
"        _TessDistanceMin (\"Tessellation Min Distance\", Float) = 10\r\n";
            }
            if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE))
            {
                shaderString +=
"        _TessDistanceMax (\"Tessellation Max Distance\", Float) = 25\r\n";
            }
            if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG) || HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH))
            {
                shaderString +=
"        _EdgeLength (\"Edge length\", Range(2,50)) = 5\r\n";
            }
            if (hasTessellationMap)
            {
                shaderString +=
"        _Displacement (\"Displacement\", Range(0, 1.0)) = 0.3\r\n";
            }
            if (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG))
            {
                shaderString +=
"        _Phong (\"Phong Strength\", Range(0,1)) = 0.54\r\n";
            }
            if (HasEffect(ShaderEffect.ADVANCEDSS_RIM))
            {
                shaderString +=
"        _RimColor (\"Rim Color\", Color) = (0.75,0.75,0.75,0.0)\r\n" +
"        _RimPower (\"Rim Power\", Range(0.5,8.0)) = 3.0\r\n";
            }
            if (HasEffect(ShaderEffect.ADVANCEDSS_EMISSIVE))
            {
                shaderString +=
"        _EmissiveStrength (\"Emissive Strength\", Float) = 1.0\r\n";
            }
            if (mainTex)
            {
                shaderString += 
"        _MainTex (\"Texture\", 2D) = \"white\" {}\r\n";
            }
            if (HasEffect(ShaderEffect.ADVANCEDSS_CUBEREFLECTION))
            {
                shaderString +=
"        _Cube (\"Reflection Cubemap\", Cube) = \"_Skybox\" { TexGen CubeReflect }\r\n";
            }
            if (hasBumpMap)
            {
                shaderString += 
"        _BumpMap (\"Bumpmap\", 2D) = \"bump\" {}\r\n";
            }
            if (hasSpecMap || hasTessellationMap)
            {
                string heightmap = "";
                if (HasEffect(ShaderEffect.ADVANCEDSS_PARALLAX, ShaderEffect.ADVANCEDSS_PARALLAX))
                {
                    heightmap = ", Heightmap (A)";
                }
                if (HasEffect(ShaderEffect.ADVANCEDSS_SPECMAP))
                {
                    shaderString +=
    "        _SpecMap (\"SpecMap (RGB)" + heightmap + "\", 2D) = \"white\" {}\r\n";
                }
                else if (HasEffect(ShaderEffect.ADVANCEDSS_GLOSSMAP))
                {
                    shaderString +=
    "        _SpecMap (\"Gloss (R), Shininess (G)" + heightmap + "\", 2D) = \"white\" {}\r\n";
                }
                else
                {
                    shaderString +=
    "        _SpecMap (\"Heightmap (A)\", 2D) = \"white\" {}\r\n";
                }
            }
            if (HasEffect(ShaderEffect.ADVANCEDSS_RELIEF))
            {
                shaderString +=
"        _RelaxedConeMap (\"RelaxedConeMap\", 2D) = \"white\" {}\r\n" +
"        _ClipTiling (\"Relief Clip Tiling U,V\", Vector) = (1, 1, 0, 0)\r\n";
            }
            if (HasEffect(ShaderEffect.ADVANCEDSS_EMISSIVE))
            {
                shaderString +=
"        _EmissiveMap (\"EmissiveMap (RGB)\", 2D) = \"black\" {}\r\n";
            }
            if (HasEffect(ShaderEffect.ADVANCEDSS_LIGHTINGRAMP))
            {
                shaderString +=
"        _LightingRamp (\"Lighting Ramp (RGB)\", 2D) = \"white\" {}\r\n";
            }
            if (transparentCutout)
            {
                shaderString += 
"        _Cutoff (\"Alpha cutoff\", Range(0,1)) = 0.5\r\n";
            }

            // End Properties and begin Subshader 3.0
            shaderString += 
"    }\r\n" +
"\r\n" +
"    SubShader\r\n" +
"    {\r\n";

            // Tags
            string queuePlusOffset = queue;
            if (queueOffset > 0) queuePlusOffset += "+" + queueOffset.ToString();
            else if (queueOffset < 0) queuePlusOffset += "-" + queueOffset.ToString();

            if (transparent)
            {
                shaderString += 
"        Tags {\"Queue\"=\"" + queuePlusOffset + "\" \"IgnoreProjector\"=\"True\" \"RenderType\"=\"Transparent\"}\r\n";
            }
            else if (transparentCutout)
            {
                shaderString += 
"        Tags {\"Queue\"=\"" + queuePlusOffset + "\" \"IgnoreProjector\"=\"True\" \"RenderType\"=\"TransparentCutout\"}\r\n";
            }
            else
            {
                shaderString +=
"        Tags { \"Queue\"=\"" + queuePlusOffset + "\" \"RenderType\"=\"Opaque\"}\r\n";
            }

            // LOD and start CG section
            shaderString += 
"        LOD " + subShader1LOD.ToString() + "\r\n" +
"\r\n" +
"        CGPROGRAM\r\n" +
"\r\n";

            if (mobile)
            {
                shaderString +=
"        #define ADVANCEDSS_MOBILE\r\n";
            }

            // Shader effect defines
            for (int i = 0; i < shaderEffects.Count; i++)
			{
                shaderString += 
"        #define " + shaderEffects[i].ToString() + "\r\n";
			}

            string lightingFunction = "BlinnPhong";
            if (hasSpecular)
            {
                if (mobile)
                {
                    lightingFunction = "MobileBlinnPhong";
                }

                if (HasEffect(ShaderEffect.ADVANCEDSS_LIGHTINGRAMP))
                {
                    lightingFunction += "Ramp";
                }
            }
            else
            {
                if (HasEffect(ShaderEffect.ADVANCEDSS_LIGHTINGRAMP))
                {
                    lightingFunction = "LambertRamp";
                }
                else
                {
                    lightingFunction = "Lambert";
                }
            }

            // Setup, Includes and end CG section and Subshader 3.0
            shaderString +=
"\r\n";

            // I hate D3D11_9x
            if (HasEffect(ShaderEffect.ADVANCEDSS_CUBEREFLECTION) && HasEffect(ShaderEffect.ADVANCEDSS_RELIEF))
            {
                shaderString +=
"        #pragma exclude_renderers d3d11_9x\r\n";
            }

            if (!mobile)
            {
                if (hasTessellation)
                {
                    shaderString +=
"        #pragma target 5.0\r\n";
                }
                else
                {
                    shaderString +=
"        #if defined(SHADER_API_D3D11) || defined(SHADER_API_D3D11_9X)\r\n" +
"        #pragma target 5.0\r\n" +
"        #else\r\n" +
"        #pragma target 3.0\r\n" +
"        #endif\r\n";
                }
            }

            shaderString +=
"" + (HasEffect(ShaderEffect.ADVANCEDSS_POM) ? "        #pragma only_renderers d3d9 d3d11 d3d11_9x\r\n" : "") +
"" + (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE, ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG, ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH) ? "        #include \"Tessellation.cginc\"\r\n" : "") +
"        #include \"" + includePrefix + "../AdvancedSS.cginc\"\r\n" +
"        #pragma surface advancedSurfaceShader " + lightingFunction + ((mobile && !hasSpecular) ? " noforwardadd" : "") + ((mobile && hasSpecular) ? " exclude_path:prepass nolightmap noforwardadd halfasview" : "") + (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPFIXED) ? " addshadow fullforwardshadows vertex:Disp tessellate:TessFixed" : "") + (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPDISTANCE) ? " addshadow fullforwardshadows vertex:Disp tessellate:TessDistance" : "") + (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONMAPEDGELENGTH) ? " addshadow fullforwardshadows vertex:Disp tessellate:TessEdge" : "") + (HasEffect(ShaderEffect.ADVANCEDSS_TESSELLATIONPHONG) ? " vertex:DispNone tessellate:TessEdge tessphong:_Phong" : "") + (transparent ? " alpha" : "") + (transparentCutout ? " alphatest:_Cutoff" : "") + ((hasTessellation) ? " nolightmap" : "") + "\r\n" +
"\r\n" +
"        ENDCG\r\n" +
"    }\r\n" +
"\r\n";

            // Fallback and end shader

            if (mobile)
            {
                shaderString +=
    "    Fallback \"Mobile/VertexLit\"\r\n";
            }
            else
            {
                if (hasSpecular)
                {
                    shaderString +=
    "    Fallback \"Specular\"\r\n";
                }
                else
                {
                    shaderString +=
    "    Fallback \"Diffuse\"\r\n";
                }
            }

            shaderString += 
"}";

            return shaderString;
        }
    }

    public class MaterialUtils
    {
        static public void SetTextureOnMaterials(Material[] materials, string propertyName, Texture texture)
        {
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].SetTexture(propertyName, texture);
            }
        }
    }

    //http://www.interact-sw.co.uk/iangblog/2004/09/16/permuterate
    public class PermuteUtils
    {
        // Returns an enumeration of enumerators, one for each permutation
        // of the input.
        public static IEnumerable<IEnumerable<T>> Permute<T>(IEnumerable<T> list, int count)
        {
            if (count == 0)
            {
                yield return new T[0];
            }
            else
            {
                int startingElementIndex = 0;
                foreach (T startingElement in list)
                {
                    IEnumerable<T> remainingItems = AllExcept(list, startingElementIndex);

                    foreach (IEnumerable<T> permutationOfRemainder in Permute(remainingItems, count - 1))
                    {
                        yield return Concat<T>(
                            new T[] { startingElement },
                            permutationOfRemainder);
                    }
                    startingElementIndex += 1;
                }
            }
        }

        // Enumerates over contents of both lists.
        public static IEnumerable<T> Concat<T>(IEnumerable<T> a, IEnumerable<T> b)
        {
            foreach (T item in a) { yield return item; }
            foreach (T item in b) { yield return item; }
        }

        // Enumerates over all items in the input, skipping over the item
        // with the specified offset.
        public static IEnumerable<T> AllExcept<T>(IEnumerable<T> input, int indexToSkip)
        {
            int index = 0;
            foreach (T item in input)
            {
                if (index != indexToSkip) yield return item;
                index += 1;
            }
        }
    }
}