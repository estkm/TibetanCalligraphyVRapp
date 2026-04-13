using UnityEngine;

public class SocketTriggerDebug : MonoBehaviour
{
	private void OnTriggerEnter (Collider other)
	{
		Debug.Log($"-- SOCKET TRIGGER ENTER: {other.name}");
	}

	private void OnTriggerExit (Collider other)
	{
		Debug.Log($"-- SOCKET TRIGGER EXIT: {other.name}");
	}
}