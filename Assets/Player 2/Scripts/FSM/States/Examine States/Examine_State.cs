using Project_PlayerInteractions.Items;
using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	public class Examine_State : Abstract_State
	{
		#region Fields

		private Item currentItem;
		private Transform itemTransform;
		private Transform parentTransform;
		
		private float currentYaw;
		private float currentPitch;

		private Camera mainCamera;

		private Vector3 lastPosition;
		private Quaternion lastRotation;

		#endregion
		
		public override void OnEnterState(PlayerFSMController controller)
		{
			// TODO: controller support
			Cursor.lockState = CursorLockMode.None;
			
			EventManager.ins.RaiseOnExamineToggle(true);

			if (this.controller == null)
			{
				this.controller = controller;
			}

			ray = new Ray();
			
			mainCamera = Camera.main;

			currentPitch = 0f;
			currentYaw = 0f;

			EventManager.ins.OnExamineItem += SetExamineItem;
		}

		public override void OnUpdate()
		{
			if (controller.Player.ControlData.examineTrigger) // TODO: Add cancel trigger (Esc)
			{
				controller.MoveToState(currentItem.transform.parent ? controller.holdingItemState : controller.searchState);
				return;
			}

			if (controller.Player.ControlData.interactTrigger)
			{
				SetRay();
				Raycast();

				if (hitInfo.transform)
				{
					if (hitInfo.transform.parent == currentItem.InteractablesTransform)
					{
						if (hitInfo.transform.CompareTag("Interactable"))
						{
							hitInfo.transform.GetComponent<Interactable>().RaiseInteraction();
							return;
						}
						
						if (hitInfo.transform.CompareTag("Item"))
						{
							EventManager.ins.RaiseOnPickUpItem(hitInfo.transform.GetComponent<Item>());
							return;
						}
					}
				}
			}

			if (!controller.Player.ControlData.interactHold) return;
			
			SetRay();
			Raycast();

			if (!hitInfo.transform || hitInfo.transform != itemTransform)
			{
				RotateItem();
			}
			else if (hitInfo.transform.CompareTag("Interactable") && hitInfo.transform.parent != currentItem.InteractablesTransform)
			{
				RotateItem();
			}
		}

		public override void OnExitState()
		{
			currentItem.Rigidbody.isKinematic = parentTransform;
			currentItem.Rigidbody.useGravity = !parentTransform;
			
			currentItem.PhysicCollider.SetActive(!parentTransform);
			currentItem.OnExamineModeToggle(false);

			itemTransform.parent = parentTransform;
			
			itemTransform.localPosition = lastPosition;
			itemTransform.localRotation = lastRotation;

			controller.Player.ItemExaminePoint.localEulerAngles = Vector3.zero;

			Cursor.lockState = CursorLockMode.Locked;
			EventManager.ins.RaiseOnExamineToggle(false);
			EventManager.ins.OnExamineItem -= SetExamineItem;
		}

		private void SetExamineItem(Item item)
		{
			currentItem = item;

			currentItem.Rigidbody.isKinematic = true;
			currentItem.Rigidbody.useGravity = false;
			
			currentItem.PhysicCollider.SetActive(true);
			currentItem.OnExamineModeToggle(true);

			itemTransform = currentItem.transform;
			parentTransform = itemTransform.parent;

			lastPosition = itemTransform.localPosition;
			lastRotation = itemTransform.localRotation;

			itemTransform.parent = controller.Player.ItemExaminePoint;
			itemTransform.localPosition = Vector3.zero;
			itemTransform.localRotation = Quaternion.identity;
		}

		private void RotateItem()
		{
			Vector2 move = controller.Player.ControlData.lookDirection;

			if (move.sqrMagnitude < 0.01f) return;

			currentYaw += -move.x * 15f * Time.deltaTime;
			currentPitch += move.y * 15f * Time.deltaTime;
			
			currentYaw = Mathf.Clamp(currentYaw, currentItem.YawLimitLeft, currentItem.YawLimitRight);

			Vector3 currentRotation = itemTransform.localEulerAngles;
			currentRotation.y = currentYaw;
			itemTransform.localEulerAngles = currentRotation;

			currentPitch = Mathf.Clamp(currentPitch, currentItem.PitchBottomLimit, currentItem.PitchTopLimit);
			currentRotation = controller.Player.ItemExaminePoint.localEulerAngles;
			currentRotation.x = currentPitch;
			controller.Player.ItemExaminePoint.localEulerAngles = currentRotation;
		}

		private new void SetRay()
		{
			rayOrigin = Input.mousePosition;

			ray = mainCamera.ScreenPointToRay(rayOrigin);
		}

		private new void Raycast()
		{
			Physics.Raycast(ray, out hitInfo);
		}
	}
}
