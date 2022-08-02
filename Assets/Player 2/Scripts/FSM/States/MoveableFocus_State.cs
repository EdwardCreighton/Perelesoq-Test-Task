using Project_PlayerInteractions.UI;
using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	public class MoveableFocus_State : Abstract_State
	{
		#region Fields

		private Transform lastFocusedTransform;

		#endregion
		
		public override void OnEnterState(PlayerFSMController controller)
		{
			if (this.controller == null)
			{
				this.controller = controller;
			}

			ray = new Ray();
			
			SetRay();
			Raycast();

			if (!hitInfo.transform || !hitInfo.transform.CompareTag("Moveable"))
			{
				controller.MoveToState(controller.searchState);
				return;
			}
			
			lastFocusedTransform = hitInfo.transform;

			tipsInfo = new TipsInfo()
			{
				drop = false,
				grab = false,
				interact = false,
				look = false,
				move = true
			};
			
			EventManager.ins.RaiseOnToggleTips(tipsInfo);
			EventManager.ins.RaiseOnInteractableFocus(lastFocusedTransform.name);
		}

		public override void OnUpdate()
		{
			controller.Player.Inventory.SetCurrentItem();
			
			controller.MoveCharacter();
			controller.LookAround();
			
			SetRay();
			Raycast();

			if (!hitInfo.transform)
			{
				controller.MoveToState(controller.searchState);
				return;
			}

			if (lastFocusedTransform != hitInfo.transform)
			{
				lastFocusedTransform = hitInfo.transform;
				
				if (!lastFocusedTransform.CompareTag("Moveable"))
				{
					controller.MoveToState(controller.searchState);
					return;
				}
			}

			if (!controller.Player.ControlData.drag) return;
			
			controller.MoveToState(controller.moveObjectState);
			return;
		}

		public override void OnExitState()
		{
			EventManager.ins.RaiseOnToggleTips();
			EventManager.ins.RaiseOnInteractableFocus();
		}
	}
}
