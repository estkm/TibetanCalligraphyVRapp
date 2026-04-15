using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ReturnHomeIfNotSocketed : MonoBehaviour
{
	private Vector3 homePos;
	private Quaternion homeRot;
	private XRGrabInteractable grab;
	private Rigidbody rb;

	private void Awake ()
	{
		homePos = transform.position;
		homeRot = transform.rotation;
		grab = GetComponent<XRGrabInteractable>();
		rb = GetComponent<Rigidbody>();
	}

	private void OnEnable ()
	{
		grab.selectExited.AddListener(OnReleased);
	}

	private void OnDisable ()
	{
		grab.selectExited.RemoveListener(OnReleased);
	}

	private void OnReleased (SelectExitEventArgs args)
	{
		bool stillSelected = false;
		if (grab.interactorsSelecting != null)
			stillSelected = grab.interactorsSelecting.Count > 0;

		if (stillSelected)
			return;

		rb.linearVelocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;
		transform.SetPositionAndRotation(homePos, homeRot);
	}
}