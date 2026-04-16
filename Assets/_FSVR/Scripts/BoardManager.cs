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

	[SerializeField] private GameObject stamp;

	private void Awake ()
	{
		homeLetters.SetActive(false);
	}

	private void ResetBoard ()
	{
		letterKA.gameObject.SetActive(false);
		//letterKA.HideAllStrokes();
		letterGA.gameObject.SetActive(false);
		//letterGA.HideAllStrokes();
		letterKHA.gameObject.SetActive(false);
		//letterKHA.HideAllStrokes();
		letterNGA.gameObject.SetActive(false);
		//letterNGA.HideAllStrokes();

		homeLetters.SetActive(false);
		stamp.SetActive(false);
	}

	public void InitHome ()
	{
		ResetBoard();
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