using FSVR;
using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

[RequireComponent(typeof(XRSocketInteractor))]
public class TileSlot : MonoBehaviour
{
	[SerializeField] private Transform snapPoint;
	[SerializeField] private int slotIndex;

	private XRSocketInteractor socket;

	private SymbolTile _currentTile;

	public SymbolTile currentTile => _currentTile;
	public int SlotIndex => slotIndex;

	//public bool IsOccupied => _currentTile != null;

	private void Awake ()
	{
		socket = GetComponent<XRSocketInteractor>();
	}

	private void OnEnable ()
	{
		socket.selectEntered.AddListener(OnSelectEntered);
		socket.selectExited.AddListener(OnSelectExited);
	}

	private void OnDisable ()
	{
		socket.selectEntered.RemoveListener(OnSelectEntered);
		socket.selectExited.RemoveListener(OnSelectExited);
	}

	private void OnSelectEntered (SelectEnterEventArgs args)
	{
		_currentTile = args.interactableObject.transform.GetComponent<SymbolTile>();
		print($"-- Tile {currentTile.Symbol} placed in slot {slotIndex}");
		NotifyPuzzle();
	}

	private void OnSelectExited (SelectExitEventArgs args)
	{
		_currentTile = null;
		NotifyPuzzle();
	}

	private void NotifyPuzzle ()
	{
		var puzzle = GetComponentInParent<ScrabbleManager>();
		//print($"-- Notifying puzzle of change in slot {slotIndex}");
		if (puzzle != null)
			puzzle.CheckWord();
	}
}