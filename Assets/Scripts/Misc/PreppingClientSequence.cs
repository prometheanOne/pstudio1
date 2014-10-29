using UnityEngine;
using System.Collections;

public class PreppingClientSequence : MonoBehaviour
{
	[SerializeField] private ParticleSystem washParticle;
	[SerializeField] private ParticleSystem rinseParticle;
	[SerializeField] private ParticleSystem sparkleParticle;
	[SerializeField] private UI2DSprite drapeSprite;
	[SerializeField] private Animation particleEmitter;
	private TweenAlpha panelFade;
	

	void OnEnable() {
		if (panelFade == null) panelFade = GetComponent<TweenAlpha>();
		StartCoroutine(RunSequence());
	}

	IEnumerator RunSequence() {
		washParticle.Play ();
		particleEmitter.Play ();
		yield return new WaitForSeconds(3.0f);
		particleEmitter.Stop ();
		rinseParticle.Play ();
		sparkleParticle.Play ();
		particleEmitter.Play ();
		yield return new WaitForSeconds(4.0f);
		/*for (int i = 1; i < 21; i++) {
			drapeSprite.fillAmount = i * 0.05f;
			yield return new WaitForSeconds(0.1f);
		}*/

		panelFade.PlayForward();
		yield return new WaitForSeconds(panelFade.duration);
		gameObject.SetActive (false);
	}
}
