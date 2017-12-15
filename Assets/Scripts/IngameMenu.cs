using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace SM3.UI
{
	public class IngameMenu : MonoBehaviour 
	{
		[SerializeField] private Button _endGameButton;

		private Action _endGame;

		public void Init(Action endGame)
		{
			_endGame = endGame;
		}

		void Start() 
		{
			_endGameButton.onClick.AddListener(() => 
			{
				_endGame?.Invoke();
				gameObject.SetActive(false);
			});
		}

	}
}