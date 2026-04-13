using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class SocketXrDebug : MonoBehaviour
{
	private XRSocketInteractor socket;

	private void Awake ()
	{
		socket = GetComponent<XRSocketInteractor>();
	}

	private void OnEnable ()
	{
		socket.hoverEntered.AddListener(OnHoverEntered);
		socket.hoverExited.AddListener(OnHoverExited);
		socket.selectEntered.AddListener(OnSelectEntered);
		socket.selectExited.AddListener(OnSelectExited);
	}

	private void OnDisable ()
	{
		socket.hoverEntered.RemoveListener(OnHoverEntered);
		socket.hoverExited.RemoveListener(OnHoverExited);
		socket.selectEntered.RemoveListener(OnSelectEntered);
		socket.selectExited.RemoveListener(OnSelectExited);
	}

	private void OnHoverEntered (HoverEnterEventArgs args)
	{
		Debug.Log($"--SOCKET HOVER ENTER: {args.interactableObject.transform.name}");
	}

	private void OnHoverExited (HoverExitEventArgs args)
	{
		Debug.Log($"--SOCKET HOVER EXIT: {args.interactableObject.transform.name}");
	}

	private void OnSelectEntered (SelectEnterEventArgs args)
	{
		Debug.Log($"--SOCKET SELECT ENTERED: {args.interactableObject.transform.name}");
	}

	private void OnSelectExited (SelectExitEventArgs args)
	{
		Debug.Log($"--SOCKET SELECT EXITED: {args.interactableObject.transform.name}");
	}
}