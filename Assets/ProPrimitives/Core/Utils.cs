/*
  Created by Juan Sebastian Munoz Arango 2013
  naruse@gmail.com
  All rights reserved
 */
using UnityEngine;
using System.Collections;

public static class Utils {

    public static int ConvertToArrIndex(int i, int j, int numRow) {
        return i+numRow*j;
    }

    public static Mesh SewMeshes(Mesh[] meshes) {
        Mesh combinedMesh = new Mesh();
        combinedMesh.Clear();
        CombineInstance[] meshesToCombine = new CombineInstance[meshes.Length];
        for(int i = 0; i < meshes.Length; i++) {
            meshesToCombine[i].mesh = meshes[i];
            meshesToCombine[i].transform = Matrix4x4.identity;//g.transform.localToWorldMatrix;
        }
        combinedMesh.CombineMeshes(meshesToCombine);
        combinedMesh.Optimize();
        return combinedMesh;
    }
}
