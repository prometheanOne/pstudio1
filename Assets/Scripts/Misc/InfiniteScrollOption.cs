using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InfiniteScrollOption : MonoBehaviour
{
	[SerializeField] private Sprite[] optionSprites;
	[SerializeField] private GameObject[] optionObjects;
	[SerializeField] private int startSelection;
	[SerializeField] private float duration;
	private int _selection;
	public int selection
	{
		get { return _selection; }
		set {
			_selection = value;
			OnChange();
		}
	}
	private float optionHeight;
	private Vector2 panelCenter;
	[SerializeField] private GameObject[] notifyObjects;

	void Start() {
		for(int i = 0; i < optionObjects.Length; i++) {
			UI2DSprite optionSprite = optionObjects[i].GetComponent<UI2DSprite>();
			optionSprite.sprite2D = optionSprites[Wrap(startSelection - 2 + i)];
		}
		optionHeight = optionObjects[0].GetComponent<UI2DSprite>().height;
		UIPanel panel = GetComponent<UIPanel>();
		panelCenter = new Vector2(panel.finalClipRegion.x, panel.finalClipRegion.y);
		_selection = startSelection;
	}

	public void ChangeSelection(bool up) {
		if (up) IncrementSelection();
		else DecrementSelection();
	}

	void IncrementSelection() {
		selection = Wrap(selection + 1);
		foreach(GameObject option in optionObjects) {
			iTween.MoveTo (option, iTween.Hash ("position", option.transform.localPosition - new Vector3(0f, optionHeight, 0f), "time", duration, "easetype", "easeOutQuad", "islocal", true));
		}
		StartCoroutine(ShiftLast());
	}

	void DecrementSelection() {
		selection = Wrap(selection - 1);
		foreach(GameObject option in optionObjects) {
			iTween.MoveTo (option, iTween.Hash ("position", option.transform.localPosition + new Vector3(0f, optionHeight, 0f), "time", duration, "easetype", "easeOutQuad", "islocal", true));
		}
		StartCoroutine(ShiftFirst());
	}

	int Wrap(int value) {
		if (value > optionSprites.Length - 1) return Wrap(value - optionSprites.Length);
		else if (value < 0) return Wrap(value + optionSprites.Length);
		else return value;
	}

	IEnumerator ShiftLast() {
		yield return new WaitForSeconds(duration + 0.1f);
		optionObjects[0].transform.localPosition += new Vector3(0f, optionHeight * 5f, 0f);
		UI2DSprite optionSprite = optionObjects[0].GetComponent<UI2DSprite>();
		optionSprite.sprite2D = optionSprites[Wrap(selection + 2)];
		GameObject[] newObjects = new GameObject[optionObjects.Length];
		newObjects[4] = optionObjects[0];
		for (int i = 0; i < newObjects.Length - 1; i++) {
			newObjects[i] = optionObjects[i + 1];
		}
		optionObjects = newObjects;
	}

	IEnumerator ShiftFirst() {
		yield return new WaitForSeconds(duration + 0.1f);
		optionObjects[4].transform.localPosition -= new Vector3(0f, optionHeight * 5f, 0f);
		UI2DSprite optionSprite = optionObjects[4].GetComponent<UI2DSprite>();
		optionSprite.sprite2D = optionSprites[Wrap(selection - 2)];
		GameObject[] newObjects = new GameObject[optionObjects.Length];
		newObjects[0] = optionObjects[4];
		for (int i = 1; i < newObjects.Length; i++) {
			newObjects[i] = optionObjects[i - 1];
		}
		optionObjects = newObjects;
	}

	void OnChange() {
		foreach (GameObject obj in notifyObjects) {
			obj.SendMessage ("OnChange", selection);
		}
	}

	void OnActivate() {
		OnChange();
	}
}