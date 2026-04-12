using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ReturnToStart : MonoBehaviour
{
	private Vector3 startPos;
	private Quaternion startRot;

	private XRGrabInteractable grab;

	private void Awake ()
	{
		grab = GetComponent<XRGrabInteractable>();

		startPos = transform.position;
		startRot = transform.rotation;

		grab.selectExited.AddListener(OnRelease);
	}

	private bool returning = false;

	private void OnRelease (SelectExitEventArgs args)
	{
		returning = true;
	}

	private void Update ()
	{
		if (returning)
		{
			transform.position = Vector3.Lerp(transform.position, startPos, Time.deltaTime * 5f);
			transform.rotation = Quaternion.Slerp(transform.rotation, startRot, Time.deltaTime * 5f);

			if (Vector3.Distance(transform.position, startPos) < 0.01f)
			{
				returning = false;
			}
		}
	}
}