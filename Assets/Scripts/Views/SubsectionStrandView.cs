using UnityEngine;
using System.Collections;

public class SubsectionStrandView : MonoBehaviour
{
	[SerializeField] private Color hoverColor;
	private Color normalColor;
	private SkinnedMeshRenderer[] meshes;

	void Start() {
		meshes = GetComponentsInChildren<SkinnedMeshRenderer>();
		normalColor = meshes[0].material.color;
	}

	void OnHover(bool isOver) {
		if (isOver) {
			foreach (SkinnedMeshRenderer mesh in meshes) mesh.material.color = normalColor + (hoverColor.a * hoverColor);
		}
		else foreach (SkinnedMeshRenderer mesh in meshes) mesh.material.color = normalColor;
	}

	void OnMouseUpAsButton() {
		HeadController.Instance.StartCutting(transform);
	}
}
