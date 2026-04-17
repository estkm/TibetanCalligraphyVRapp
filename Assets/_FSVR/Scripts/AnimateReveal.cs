using DG.Tweening;
using UnityEngine;

public class AnimateReveal : MonoBehaviour
{
	[SerializeField]
	private float revealDuration = 0.7f;

	[SerializeField]
	private Ease easeOption = Ease.OutExpo;

	private void OnEnable ()
	{
		transform.DOScale(transform.localScale, revealDuration).From(0f).SetEase(easeOption);
	}
}