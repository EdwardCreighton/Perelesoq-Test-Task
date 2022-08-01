using System;
using UnityEngine;
using UnityEngine.UI;
using Project_PlayerInteractions.Items;

namespace Project_PlayerInteractions.UI
{
	[Serializable]
	public class ItemUI
	{
		#region Fields

		public GameObject itemPlaceHolder;
		public Item itemRef;

		private Image background;
		private Image itemIcon;

		#endregion

		public void OnAwakeComponent()
		{
			background = itemPlaceHolder.transform.Find("BACKGROUND").GetComponent<Image>();
			itemIcon = itemPlaceHolder.transform.Find("ICON").GetComponent<Image>();
		}

		public void SetIcon(Sprite iconSprite)
		{
			itemIcon.sprite = iconSprite;
		}

		public void SetBackground(Sprite backgroundSprite)
		{
			background.sprite = backgroundSprite;
		}
	}
}
