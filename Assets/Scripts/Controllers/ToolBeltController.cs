using UnityEngine;
using System.Collections;


public enum BeltTool {
	comb = 0,
	clip,
	shears
}

public class ToolBeltController : SingletonComponent<ToolBeltController>
{
	private bool visible;
	private TweenPosition showTween;
	[SerializeField] private float timeout;
	public BeltTool currentTool;
	[SerializeField] private GameObject[] pointers;
	private ToolBeltButtonView[] buttonViews;
	[SerializeField] private GameObject hairClipPrefab;

	void Start() {
		showTween = GetComponent<TweenPosition>();
		buttonViews = GetComponentsInChildren<ToolBeltButtonView>();
	}

	void OnHover(bool isOver) {
		if (isOver && !visible) {
			showTween.Play (true);
			visible = true;
			StartCoroutine(Timeout(timeout));
		}
	}

	public void Hide() {
		if (visible) {
			StopAllCoroutines();
			showTween.Play (false);
		}
	}

	public void NotVisible() {
		visible = false;
	}

	IEnumerator Timeout(float delay) {
		yield return new WaitForSeconds(timeout);
		RaycastHit hitInfo;
		if (Physics.Raycast (UICamera.mainCamera.ScreenPointToRay (Input.mousePosition), out hitInfo)) {
			if (hitInfo.collider != collider && hitInfo.collider.transform.parent != transform && visible) Hide();
			else StartCoroutine (Timeout (0.2f));
		}
		else Hide ();
	}

	public void ToolClicked(BeltTool tool) {
		Hide ();
		for(int i = 0; i < pointers.Length; i++) pointers[i].SetActive (i == (int)tool);
		foreach (ToolBeltButtonView button in buttonViews) {
			if (button.tool == tool && !button.multiUse) button.Visible(false);
			else button.Visible(true);
		}
		switch (tool) {
		case BeltTool.comb:
			currentTool = BeltTool.comb;
			HeadController.Instance.ShowAllRefPoints (true);
			break;
		case BeltTool.clip:
			currentTool = BeltTool.clip;
			GameObject newClip = GameObject.Instantiate (hairClipPrefab);
			break;
		case BeltTool.shears:
			currentTool = BeltTool.shears;
			//HeadController.Instance.StartCutting();
			break;
		}
	}
}
