using System;
using System.Collections.Generic;
using UnityEngine;

namespace Project_PlayerInteractions.UI
{
	[Serializable]
	public class TipsImageHolder
	{
		#region Fields

		[SerializeField] private InputSchemeType inputType;

		[Tooltip("Follow the order in 'Interaction Text' list")] [SerializeField] private List<Sprite> sprites;

		#endregion

		#region Getters

		public InputSchemeType InputType => inputType;
		public List<Sprite> Sprites => sprites;

		#endregion
	}
}
