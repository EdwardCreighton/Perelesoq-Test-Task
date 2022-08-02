using UnityEngine;
using System.Collections.Generic;
using Project_PlayerInteractions.Items;

namespace Project_PlayerInteractions.UI
{
	public class InventoryUI : MonoBehaviour
	{
		#region Fields

		[SerializeField] private Sprite standardBackground;
		[SerializeField] private Sprite onChooseBackground;
		
		private List<ItemUI> items;
		private ItemUI lastActiveItem;

		#endregion

		public void OnAwakeComponent()
		{
			int count = transform.childCount;
			
			items = new List<ItemUI>(count);
			
			for (int i = 0; i < count; i++)
			{
				ItemUI itemUI = new ItemUI
				{
					itemPlaceHolder = transform.GetChild(i).gameObject
				};

				items.Add(itemUI);
				items[i].OnAwakeComponent();
				items[i].itemPlaceHolder.SetActive(false);
			}

			lastActiveItem = null;

			EventManager.ins.OnUpdateInventoryUI += UpdateUI;
			EventManager.ins.OnPickActiveItemUI += SetCurrentItemUI;
		}

		private void UpdateUI(List<Item> pickedItems)
		{
			int index = 0;
			
			foreach (var itemUI in items)
			{
				itemUI.itemPlaceHolder.SetActive(false);
				itemUI.itemRef = null;

				if (index < pickedItems.Count)
				{
					items[index].itemRef = pickedItems[index];
					items[index].itemPlaceHolder.SetActive(true);
					items[index].SetBackground(standardBackground);
					items[index].SetIcon(pickedItems[index].ItemUIInfo.logo);

					++index;
				}
			}
		}

		private void SetCurrentItemUI(Item item)
		{
			lastActiveItem?.SetBackground(standardBackground);

			if (!item)
			{
				lastActiveItem = null;
				return;
			}
			
			foreach (var itemUI in items)
			{
				if (itemUI.itemRef == item)
				{
					lastActiveItem = itemUI;
					lastActiveItem.SetBackground(onChooseBackground);
					return;
				}
			}
		}
	}
}
