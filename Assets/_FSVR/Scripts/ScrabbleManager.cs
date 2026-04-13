using System;
using System.Text;
using DG.Tweening;
using UnityEngine;

namespace FSVR
{
	public class ScrabbleManager : MonoBehaviour
	{
		[SerializeField] private string targetWord = "KHANGA";

		[SerializeField] private GameObject house;
		[SerializeField] private GameObject[] houseSymbols;
		[SerializeField] private GameObject[] houseTiles;

		[SerializeField] private TileSlot[] slots;

		private FSManager _fsManager;

		//game status enum
		private enum GameStatus
		{ Waiting, Playing, Done }

		private GameStatus gameStatus = GameStatus.Waiting;

		// Start is called once before the first execution of Update after the MonoBehaviour is created
		private void Start ()
		{
			_fsManager = FSManager.Instance;
			//_fsManager.TestLoad();

			house.SetActive(false);
		}

		public void StartGame ()
		{
			gameStatus = GameStatus.Playing;
			gameObject.SetActive(true);
			DisplayHouseSymbols();
		}

		public string BuildCurrentWord ()
		{
			var sb = new StringBuilder();

			foreach (var slot in slots)
			{
				if (!slot.currentTile)
				{
					sb.Append("_");
				}
				else
				{
					sb.Append(slot.currentTile.Symbol);
				}
			}

			return sb.ToString();
		}

		public bool IsCorrectWord ()
		{
			if (!AreAllSlotsFilled())
				return false;

			var builtWord = "";

			foreach (TileSlot slot in slots)
			{
				builtWord += slot.currentTile.Symbol;
			}

			return string.Equals(builtWord, targetWord, System.StringComparison.OrdinalIgnoreCase);
		}

		public bool AreAllSlotsFilled ()
		{
			foreach (TileSlot slot in slots)
			{
				if (slot.currentTile == null)
					return false;
			}

			return true;
		}

		public void CheckWord ()
		{
			var currentWord = BuildCurrentWord();
			Debug.Log("-- Current sequence: " + currentWord);

			if (currentWord == targetWord)
			{
				Debug.Log("-- Correct word!");
				OnSolved();
			}
			else
			{
				Debug.Log("-- Not correct.");
			}
		}

		private void DisplayHouseSymbols ()
		{
			foreach (var houseSymbol in houseSymbols)
			{
				houseSymbol.SetActive(true);
			}
		}

		private void DisplayHouse ()
		{
			house.SetActive(true);
			house.transform.DOScale(house.transform.localScale, 1f).From(0f).SetEase(Ease.InOutExpo);
			house.transform.DORotateQuaternion(Quaternion.Euler(0f, 360f, 0f), 1f).From().SetEase(Ease.InOutExpo);
		}

		private void OnSolved ()
		{
			gameStatus = GameStatus.Done;
			//hide tiles and slots
			HideTiles();
			DisplayHouse();
		}

		private void HideTiles ()
		{
			foreach (var tile in houseTiles)
			{
				tile.SetActive(false);
			}

			foreach (var slot in slots)
			{
				slot.gameObject.SetActive(false);
			}
		}
	}
}