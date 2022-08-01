using System;
using System.Collections.Generic;
using UnityEngine;
using Project_PlayerInteractions.Items;
using Project_PlayerInteractions.UI;

namespace Project_PlayerInteractions
{
	public class EventManager : MonoBehaviour
	{
		#region Fields

		public static EventManager ins;

		#endregion

		#region Events

		public event Action<Item> OnPickUpItem;
		public event Action<Item, Ghost> OnDropItem;
		public event Action<Item> OnUseItem;
		public event Action<List<Item>> OnUpdateInventoryUI;
		public event Action<Item> OnPickActiveItem;
		public event Action<string> OnInteractableFocus;
		public event Action<TipsInfo> OnToggleTips;
		public event Action<Item> OnExamineItem;
		public event Action<bool> OnExamineToggle; 
		public event Action<InputSchemeType> OnChangeControls;

		#endregion

		public void OnAwakeComponent() => ins = this;

		public void RaiseOnPickUpItem(Item item) => OnPickUpItem?.Invoke(item);
		public void RaiseOnDropItem(Item item, Ghost ghost) => OnDropItem?.Invoke(item, ghost);
		public void RaiseOnUseItem(Item item) => OnUseItem?.Invoke(item);
		public void RaiseOnUpdateInventoryUI(List<Item> items) => OnUpdateInventoryUI?.Invoke(items);
		public void RaiseOnPickActiveItem(Item item) => OnPickActiveItem?.Invoke(item);
		public void RaiseOnInteractableFocus(string name = null) => OnInteractableFocus?.Invoke(name);
		public void RaiseOnToggleTips(TipsInfo tipsInfo = null) => OnToggleTips?.Invoke(tipsInfo);
		public void RaiseOnExamineItem(Item item) => OnExamineItem?.Invoke(item);
		public void RaiseOnChangeControls(InputSchemeType type) => OnChangeControls?.Invoke(type);
		public void RaiseOnExamineToggle(bool examineState) => OnExamineToggle?.Invoke(examineState);
	}
}
