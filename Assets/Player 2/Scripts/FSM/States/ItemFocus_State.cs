using Project_PlayerInteractions.Items;
using Project_PlayerInteractions.UI;
using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	public class ItemFocus_State : Abstract_State
	{
		#region Fields

		private Transform lastFocusedTransform;
		private Item focusedItem;

		#endregion
		
		public override void OnEnterState(PlayerFSMController controller)
		{
			if (this.controller == null)
			{
				this.controller = controller;
			}

			ray = new Ray();
			
			SetRay();
			SphereCast();

			lastFocusedTransform = hitInfo.transform;
			focusedItem = lastFocusedTransform.GetComponent<Item>();

			tipsInfo = new TipsInfo()
			{
				grab = true,
			};
			
			EventManager.ins.RaiseOnToggleTips(tipsInfo);
			EventManager.ins.RaiseOnInteractableFocus(focusedItem.ItemUIInfo.name);
		}

		public override void OnUpdate()
		{
			controller.Player.Inventory.SetCurrentItem();
			
			controller.MoveCharacter();
			controller.LookAround();

			if (controller.Player.Inventory.CurrentItem)
			{
				controller.MoveToState(controller.holdingItemState);
				return;
			}

			SetRay();
			SphereCast();

			if (!hitInfo.transform || lastFocusedTransform != hitInfo.transform)
			{
				controller.MoveToState(controller.searchState);
				return;
			}

			if (focusedItem.ExamineEnabled)
			{
				tipsInfo.look = true;
				EventManager.ins.RaiseOnToggleTips(tipsInfo);

				if (controller.Player.ControlData.examineTrigger)
				{
					controller.MoveToState(controller.examineState);
					EventManager.ins.RaiseOnExamineItem(focusedItem);
					return;
				}
			}

			if (controller.Player.ControlData.interactTrigger)
			{
				EventManager.ins.RaiseOnPickUpItem(hitInfo.transform.GetComponent<Item>());
				controller.MoveToState(controller.searchState);
				return;
			}
		}

		public override void OnExitState()
		{
			EventManager.ins.RaiseOnToggleTips();
			EventManager.ins.RaiseOnInteractableFocus();
		}
	}
}
