using UnityEngine;
using System.Collections;

public class AmbientRewardController : SingletonComponent<AmbientRewardController>
{
	[SerializeField] private UILabel rewardText;
	[SerializeField] private float endScale;
	[SerializeField] private Vector3 path;
	[SerializeField] private float fadeDuration;
	private TweenPosition posTween;
	private TweenColor colTween;
	private TweenScale scaleTween;

	void Start() {
		posTween = rewardText.gameObject.GetComponent<TweenPosition>();
		colTween = rewardText.gameObject.GetComponent<TweenColor>();
		scaleTween = rewardText.gameObject.GetComponent<TweenScale>();
		colTween.duration = fadeDuration;
		posTween.duration = fadeDuration;
		scaleTween.duration = fadeDuration;
		scaleTween.from = Vector3.one;
		scaleTween.to = new Vector3(endScale, endScale, 1f);
	}

	public void ShowReward(Vector3 origin, string text, Color startColor) {
		//Debug.Log ("Showing ambient reward");
		rewardText.text = text;
		Vector3 startLoc = UICamera.mainCamera.ScreenToWorldPoint (Camera.main.WorldToScreenPoint (origin));
		rewardText.color = startColor;
		rewardText.transform.position = startLoc;
		posTween.from = startLoc;
		posTween.to = startLoc + path;
		colTween.from = startColor;
		colTween.to = new Color(startColor.r, startColor.g, startColor.b, 0f);
		posTween.PlayForward();
		colTween.PlayForward();
		scaleTween.PlayForward ();
		StartCoroutine(Reset());
	}

	IEnumerator Reset() {
		yield return new WaitForSeconds(fadeDuration);
		rewardText.text = "";
		rewardText.transform.localScale = Vector3.one;
	}
}