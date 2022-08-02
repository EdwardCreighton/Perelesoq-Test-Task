using Project_PlayerInteractions.Items;
using Project_PlayerInteractions.UI;
using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	public class InteractableFocus_State : Abstract_State
	{
		#region Fields

		private Transform lastFocusedTransform;
		private Interactable focusedInteractable;

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
			focusedInteractable = lastFocusedTransform.GetComponent<Interactable>();

			tipsInfo = new TipsInfo()
			{
				drop = false,
				grab = false,
				interact = true,
				look = false,
				move = false
			};
			
			EventManager.ins.RaiseOnToggleTips(tipsInfo);
			EventManager.ins.RaiseOnInteractableFocus(focusedInteractable.Name);
		}

		public override void OnUpdate()
		{
			controller.Player.Inventory.SetCurrentItem();
			
			controller.MoveCharacter();
			controller.LookAround();
			
			SetRay();
			SphereCast();

			if (!hitInfo.transform)
			{
				controller.MoveToState(controller.searchState);
				return;
			}

			if (lastFocusedTransform != hitInfo.transform)
			{
				lastFocusedTransform = hitInfo.transform;
				
				if (hitInfo.transform.CompareTag("Interactable"))
				{
					focusedInteractable = lastFocusedTransform.GetComponent<Interactable>();
				}
				else
				{
					controller.MoveToState(controller.searchState);
					return;
				}
			}
			
			if (!controller.Player.ControlData.interactTrigger) return;
			
			focusedInteractable.RaiseInteraction();
			controller.MoveToState(controller.searchState);
		}

		public override void OnExitState()
		{
			EventManager.ins.RaiseOnToggleTips();
			EventManager.ins.RaiseOnInteractableFocus();
		}
	}
}
