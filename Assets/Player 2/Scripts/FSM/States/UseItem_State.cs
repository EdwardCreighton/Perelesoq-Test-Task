using Project_PlayerInteractions.Items;
using Project_PlayerInteractions.UI;
using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	public class UseItem_State : Abstract_State
	{
		#region Fields

		private Transform lastFocusedTransform;
		private Applicable focusedApplicable;
		private Item currentItem;

		private Ghost ghost;

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
			focusedApplicable = lastFocusedTransform.GetComponent<Applicable>();

			currentItem = this.controller.Player.Inventory.CurrentItem;
			
			CreateGhost();

			tipsInfo = new TipsInfo()
			{
				drop = true,
				grab = false,
				interact = true,
				look = false,
				move = false
			};
			
			EventManager.ins.RaiseOnToggleTips(tipsInfo);
		}

		public override void OnUpdate()
		{
			controller.MoveCharacter();
			controller.LookAround();
			
			if (controller.Player.ControlData.dropItemTrigger)
			{
				EventManager.ins.RaiseOnDropItem(currentItem, ghost);
				
				controller.MoveToState(controller.searchState);
				return;
			}

			if (controller.Player.ControlData.switchItemNumber || Mathf.Abs(controller.Player.ControlData.switchItemScroll) > 0f)
			{
				controller.MoveToState(controller.searchState);
				return;
			}
			
			SetRay();
			SphereCast();

			if (!hitInfo.transform)
			{
				controller.MoveToState(controller.dropItemState);
				return;
			}
			
			if (lastFocusedTransform != hitInfo.transform)
			{
				if (hitInfo.transform.CompareTag("Applicable"))
				{
					lastFocusedTransform = hitInfo.transform;
					focusedApplicable = lastFocusedTransform.GetComponent<Applicable>();

					if (focusedApplicable.InteractionID != currentItem.InteractionID)
					{
						controller.MoveToState(controller.dropItemState);
						return;
					}
				}
				else
				{
					controller.MoveToState(controller.dropItemState);
					return;
				}
			}

			Vector3 position = focusedApplicable.TargetPosition + lastFocusedTransform.position;
			ghost.SetPosition(position);
			Vector3 rotation = lastFocusedTransform.eulerAngles + lastFocusedTransform.TransformDirection(focusedApplicable.TargetRotation);
			ghost.SetRotation(Quaternion.Euler(rotation));

			if (!controller.Player.ControlData.interactTrigger) return;
			
			EventManager.ins.RaiseOnUseItem(currentItem);

			Transform transform = currentItem.transform;
			transform.position = ghost.Position;
			transform.rotation = ghost.Rotation;
			
			focusedApplicable.RaiseInteraction();
			
			controller.MoveToState(controller.searchState);
		}

		public override void OnExitState()
		{
			ghost.Destroy();
			ghost = null;
			
			EventManager.ins.RaiseOnToggleTips();
		}

		private void CreateGhost()
		{
			ghost = new Ghost(currentItem);
			
			Vector3 position = focusedApplicable.TargetPosition + lastFocusedTransform.position;
			ghost.SetPosition(position);
			Vector3 rotation = lastFocusedTransform.eulerAngles + lastFocusedTransform.TransformDirection(focusedApplicable.TargetRotation);
			ghost.SetRotation(Quaternion.Euler(rotation));
		}
	}
}
