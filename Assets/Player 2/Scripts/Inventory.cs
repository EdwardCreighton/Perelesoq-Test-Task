using System.Collections.Generic;
using Project_PlayerInteractions.Items;
using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	public class Inventory
	{
		#region Fields

		private List<Item> pickedItems;

		private int currentItemIndex = -1;

		private PlayerComponent player;

		#endregion

		#region Getters

		public Item CurrentItem => currentItemIndex > -1 ? pickedItems[currentItemIndex] : null;

		#endregion
		
		public Inventory(PlayerComponent playerComponent)
		{
			player = playerComponent;
			pickedItems = new List<Item>(10);

			EventManager.ins.OnPickUpItem += PickUpItem;
			EventManager.ins.OnDropItem += DropItem;
			EventManager.ins.OnUseItem += ReleaseItem;
		}
		
		public void SetCurrentItem()
		{
			if (pickedItems.Count == 0) return;

			if (player.ControlData.switchItemNumber)
			{
				if (currentItemIndex >= 0)
				{
					pickedItems[currentItemIndex].gameObject.SetActive(false);
				}

				currentItemIndex = Mathf.Clamp(player.ControlData.itemIndex, 0, pickedItems.Count - 1);
			}
			else if (Mathf.Abs(player.ControlData.switchItemScroll) > 0)
			{
				if (currentItemIndex >= 0)
				{
					pickedItems[currentItemIndex].gameObject.SetActive(false);
				}
				
				currentItemIndex += (int)Mathf.Sign(player.ControlData.switchItemScroll);
				currentItemIndex = Mathf.Clamp(currentItemIndex, -1, pickedItems.Count - 1);
			}
			else
			{
				return;
			}

			if (currentItemIndex >= 0)
			{
				pickedItems[currentItemIndex].gameObject.SetActive(true);
			}
			
			EventManager.ins.RaiseOnPickActiveItem(CurrentItem);
		}

		private void PickUpItem(Item newItem)
		{
			if (pickedItems.Count == 10)
			{
				// TODO: Tell player of full inventory
				return;
			}
			
			pickedItems.Insert(0, newItem);
			
			newItem.PhysicCollider.SetActive(false);
			newItem.gameObject.SetActive(false);
			newItem.Rigidbody.isKinematic = true;
			newItem.Rigidbody.useGravity = false;

			Transform transform = newItem.transform;
			transform.parent = player.ItemHolderPoint;
			transform.localPosition = newItem.HoldPositionOffset;
			transform.localEulerAngles = newItem.HoldRotationOffset;
			
			EventManager.ins.RaiseOnUpdateInventoryUI(pickedItems);
			EventManager.ins.RaiseOnPickActiveItem(CurrentItem);
		}

		private void DropItem(Item item, Ghost ghost)
		{
			pickedItems.Remove(item);
			
			item.PhysicCollider.SetActive(true);
			item.Rigidbody.isKinematic = false;
			item.Rigidbody.useGravity = true;

			Transform transform = item.transform;
			transform.parent = null;
			transform.position = ghost.Position;
			transform.rotation = ghost.Rotation;

			currentItemIndex = -1;
			
			EventManager.ins.RaiseOnUpdateInventoryUI(pickedItems);
			EventManager.ins.RaiseOnPickActiveItem(CurrentItem);
		}

		private void ReleaseItem(Item item)
		{
			pickedItems.Remove(item);

			item.transform.parent = null;

			currentItemIndex = -1;
			
			EventManager.ins.RaiseOnUpdateInventoryUI(pickedItems);
			EventManager.ins.RaiseOnPickActiveItem(CurrentItem);
		}
	}
}