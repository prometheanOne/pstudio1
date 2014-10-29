using UnityEngine;
using System.Collections;

public class ToolBeltButtonView : MonoBehaviour 
{
	public BeltTool tool;
	private TweenPosition posTween;
	private TweenColor colTween;
	public bool multiUse;
	private bool visible;

	void Start() {
		posTween = GetComponent<TweenPosition>();
		colTween = GetComponent<TweenColor>();
	}

	void OnHover(bool isOver) {
		posTween.Play (isOver);
	}

	void OnClick() {
		ToolBeltController.Instance.ToolClicked (tool);
		//if (!multiUse) colTween.Play (true);
	}

	public void Visible (bool vis) {
		visible = vis;
		//Debug.Log ("Setting tool button "+gameObject.name+" "+vis.ToString ());
		if (vis) gameObject.SetActive (true);
		colTween.Play (!vis);
	}

	public void Disable() {
		if (!visible) gameObject.SetActive (false);
	}
}

