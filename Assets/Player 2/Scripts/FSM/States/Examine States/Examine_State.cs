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

		private bool isRotating;
		private bool isUsingItem;
		
		private Item applyItem;
		private GameObject applyItemCopy;
		private Applicable focusedApplicable;

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

			controller.Player.Inventory.SetCurrentItem();

			EventManager.ins.OnExamineItem += SetExamineItem;
			EventManager.ins.OnExamineUseItem += StartUseItem;
		}

		public override void OnUpdate()
		{
			if (controller.Player.ControlData.examineTrigger) // TODO: Add cancel trigger (Esc)
			{
				controller.MoveToState(currentItem.transform.parent ? controller.holdingItemState : controller.searchState);
				return;
			}

			if (isUsingItem)
			{
				UseItem();
				
				return;
			}

			if (isRotating)
			{
				RotateItem();
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
					}
				}
			}

			if (controller.Player.ControlData.interactHold && !isRotating)
			{
				SetRay();
				Raycast();

				if (!hitInfo.transform || hitInfo.transform != itemTransform)
				{
					isRotating = true;
				}
				else if (hitInfo.transform.CompareTag("Interactable") && hitInfo.transform.parent != currentItem.InteractablesTransform)
				{
					isRotating = true;
				}
				else
				{
					isRotating = false;
				}
			}
			else if (!controller.Player.ControlData.interactHold)
			{
				isRotating = false;
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

			controller.Player.Inventory.ForceSetCurrentItemToNull();
			
			Cursor.lockState = CursorLockMode.Locked;
			EventManager.ins.RaiseOnExamineToggle(false);
			
			EventManager.ins.OnExamineItem -= SetExamineItem;
			EventManager.ins.OnExamineUseItem -= StartUseItem;
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

		private void StartUseItem(Item item)
		{
			if (item == currentItem)
			{
				EventManager.ins.RaiseOnPickUpItem(null);
				return;
			}
			
			isUsingItem = true;

			applyItem = item;
			applyItemCopy = Object.Instantiate(applyItem.gameObject, Vector3.zero, Quaternion.identity);
			applyItemCopy.SetActive(true);
		}

		private void UseItem()
		{
			SetRay();
			Raycast();
			
			applyItemCopy.transform.parent = null;

			if (!hitInfo.transform)
			{
				SimplePositionApplyItem();
				return;
			}

			if (applyItem.InteractionID == -1) // Cannot use item
			{
				SimplePositionApplyItem();
				return;
			}

			if (!hitInfo.transform.CompareTag("Applicable"))
			{
				SimplePositionApplyItem();
				return;
			}

			if (!focusedApplicable || hitInfo.transform != focusedApplicable.transform)
			{
				focusedApplicable = hitInfo.transform.GetComponent<Applicable>();
			}

			if (applyItem.InteractionID != focusedApplicable.InteractionID)
			{
				SimplePositionApplyItem();
				return;
			}
			
			// All checks are passed

			applyItemCopy.transform.parent = focusedApplicable.transform;
			
			applyItemCopy.transform.localPosition = focusedApplicable.TargetPosition;
			applyItemCopy.transform.localRotation = Quaternion.Euler(focusedApplicable.TargetRotation);

			if (!controller.Player.ControlData.interactHold)
			{
				EventManager.ins.RaiseOnUseItem(applyItem);

				applyItem.transform.position = applyItemCopy.transform.position;
				applyItem.transform.rotation = applyItemCopy.transform.rotation;
								
				applyItem.gameObject.SetActive(true);
								
				focusedApplicable.RaiseInteraction();
								
				isUsingItem = false;
				Object.Destroy(applyItemCopy);
			}
		}

		private void SimplePositionApplyItem()
		{
			applyItemCopy.transform.position = controller.Player.CameraTransform.position + ray.direction * 0.7f;
			applyItemCopy.transform.rotation = Quaternion.identity;

			if (!controller.Player.ControlData.interactHold)
			{
				isUsingItem = false;
				Object.Destroy(applyItemCopy);
				EventManager.ins.RaiseOnPickUpItem(null);
			}
		}

		private new void SetRay()
		{
			rayOrigin = Input.mousePosition;

			ray = mainCamera.ScreenPointToRay(rayOrigin);
		}

		private new void Raycast()
		{
			Physics.Raycast(ray, out hitInfo);
			//Debug.Log(hitInfo.transform.name);
		}
	}
}
