using System;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	[SerializeField] private Letter letterKA;
	[SerializeField] private Letter letterKHA;
	[SerializeField] private Letter letterGA;
	[SerializeField] private Letter letterNGA;

	[Header("Home Letters")]
	[SerializeField] private GameObject homeLetters;

	private void Awake ()
	{
		homeLetters.SetActive(false);
	}

	private void ResetBoard ()
	{
		//letterKA.SetActive(false);
		letterKA.HideAllStrokes();
		//letterGA.SetActive(false);
		letterGA.HideAllStrokes();
		//letterKHA.SetActive(false);
		letterKHA.HideAllStrokes();
		//letterNGA.SetActive(false);
		letterNGA.HideAllStrokes();
	}

	public void InitHome ()
	{
		ResetBoard();
		//letterKHA.SetActive(true);
		//letterNGA.SetActive(true);
	}

	internal void Show ()
	{
		gameObject.SetActive(true);
	}

	internal void HideLetters ()
	{
		ResetBoard();
	}

	internal void ShowHouseLetters ()
	{
		homeLetters.SetActive(true);
	}
}