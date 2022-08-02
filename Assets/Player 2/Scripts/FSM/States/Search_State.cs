using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	public class Search_State : Abstract_State
	{
		public override void OnEnterState(PlayerFSMController controller)
		{
			if (this.controller == null)
			{
				this.controller = controller;
			}

			ray = new Ray();

			Cursor.lockState = CursorLockMode.Locked;
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

			if (!hitInfo.transform) return;

			switch (hitInfo.transform.tag)
			{
				case "Item":
				{
					controller.MoveToState(controller.itemFocusState);
					return;
				}
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
			
		}
	}
}
