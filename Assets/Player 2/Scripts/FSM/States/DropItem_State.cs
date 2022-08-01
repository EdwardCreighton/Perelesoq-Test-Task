using Project_PlayerInteractions.Items;
using Project_PlayerInteractions.UI;
using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	public class DropItem_State : Abstract_State
	{
		#region Fields

		private Item currentItem;
		private Ghost ghostItem;

		#endregion
		
		public override void OnEnterState(PlayerFSMController controller)
		{
			if (this.controller == null)
			{
				this.controller = controller;
			}

			currentItem = this.controller.Player.Inventory.CurrentItem;

			if (!currentItem)
			{
				this.controller.MoveToState(this.controller.searchState);
				return;
			}

			ray = new Ray();
			
			CreateGhost();
			
			tipsInfo = new TipsInfo()
			{
				grab = false,
				drop = true,
				interact = false,
				look = false,
				move = false
			};
			
			EventManager.ins.RaiseOnToggleTips(tipsInfo);
		}

		public override void OnUpdate()
		{
			controller.MoveCharacter();
			controller.LookAround();
			
			// TODO: Check for cancel button and move to searchState

			if (controller.Player.ControlData.switchItemNumber || Mathf.Abs(controller.Player.ControlData.switchItemScroll) > 0)
			{
				controller.MoveToState(controller.searchState);
				return;
			}
			
			SetRay();
			SphereCast();

			DroppingItem();
		}

		public override void OnExitState()
		{
			ghostItem?.Destroy();
			ghostItem = null;

			EventManager.ins.RaiseOnToggleTips();
		}

		private void CreateGhost()
		{
			ghostItem = new Ghost(currentItem);
			
			if (!hitInfo.transform)
			{
				ghostItem.SetPosition(rayOrigin + rayDirection * controller.Player.InteractionMaxDistance);
				ghostItem.SetRotation(Quaternion.identity);
			}
			else
			{
				ghostItem.SetPosition(hitInfo.point);
				ghostItem.SetTransformUp(hitInfo.normal);
			}
		}

		private void DroppingItem()
		{
			if (!hitInfo.transform)
			{
				ghostItem.SetPosition(rayOrigin + rayDirection * controller.Player.InteractionMaxDistance);
				ghostItem.SetRotation(Quaternion.identity);
			}
			else
			{
				if (currentItem.InteractionID > -1 && hitInfo.transform.CompareTag("Applicable"))
				{
					if (currentItem.InteractionID == hitInfo.transform.GetComponent<Applicable>().InteractionID)
					{
						controller.MoveToState(controller.useItemState);
						return;
					}
				}
				
				ghostItem.SetPosition(hitInfo.point);
				ghostItem.SetTransformUp(hitInfo.normal);
			}

			if (controller.Player.ControlData.interactTrigger || controller.Player.ControlData.dropItemTrigger)
			{
				EventManager.ins.RaiseOnDropItem(currentItem, ghostItem);
				
				controller.MoveToState(controller.searchState);
				return;
			}
		}
	}
}
