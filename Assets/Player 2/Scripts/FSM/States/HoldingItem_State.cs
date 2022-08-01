using Project_PlayerInteractions.Items;
using Project_PlayerInteractions.UI;
using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	public class HoldingItem_State : Abstract_State
	{
		#region Fields

		private Item item;

		#endregion
		
		public override void OnEnterState(PlayerFSMController controller)
		{
			if (this.controller == null)
			{
				this.controller = controller;
			}
			
			ray = new Ray();

			item = this.controller.Player.Inventory.CurrentItem;

			tipsInfo = new TipsInfo()
			{
				drop = true,
			};
			
			EventManager.ins.RaiseOnToggleTips(tipsInfo);
		}

		public override void OnUpdate()
		{
			controller.MoveCharacter();
			controller.LookAround();

			item = controller.Player.Inventory.CurrentItem;

			if (!item)
			{
				controller.MoveToState(controller.searchState);
				return;
			}

			tipsInfo.look = item.ExamineEnabled;
			EventManager.ins.RaiseOnToggleTips(tipsInfo);

			if (controller.Player.ControlData.examineTrigger)
			{
				controller.MoveToState(controller.examineState);
				EventManager.ins.RaiseOnExamineItem(item);
				return;
			}
			
			if (controller.Player.ControlData.dropItemTrigger)
			{
				controller.MoveToState(controller.dropItemState);
				return;
			}

			SetRay();
			SphereCast();

			if (!hitInfo.transform) return;

			switch (hitInfo.transform.tag)
			{
				case "Interactable":
				{
					controller.MoveToState(controller.interactableFocusState);
					return;
				}
				case "Moveable":
				{
					controller.MoveToState(controller.moveableFocusState);
					return;
				}
			}
		}

		public override void OnExitState()
		{
			EventManager.ins.RaiseOnToggleTips();
		}
	}
}
