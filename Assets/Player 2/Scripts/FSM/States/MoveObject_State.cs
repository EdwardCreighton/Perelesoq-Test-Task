using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	public class MoveObject_State : Abstract_State
	{
		#region Fields
		
		private float distance;
		
		private Transform dragPoint;
		private Rigidbody rigidbody;

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

			// TODO: Refactor drag point implementation
			GameObject dragPointGO = new GameObject("Drag Point");

			dragPoint = dragPointGO.transform;
			dragPoint.position = hitInfo.point;
			dragPoint.parent = hitInfo.transform;

			rigidbody = hitInfo.rigidbody;
			
			distance = hitInfo.distance;
		}

		public override void OnUpdate()
		{
			controller.Player.Inventory.SetCurrentItem();
			
			controller.MoveCharacter();
			controller.LookAround();

			if (!controller.Player.ControlData.drag)
			{
				controller.MoveToState(controller.searchState);
				return;
			}
			
			SetRay();
			
			Vector3 targetPoint = rayOrigin + rayDirection * distance;
			Vector3 dragDirection = targetPoint - dragPoint.position;

			dragDirection = Vector3.ClampMagnitude(dragDirection, 1f);

			// TODO: Use PID controller
			rigidbody.AddForceAtPosition(dragDirection * (controller.Player.DragForce * Time.deltaTime), dragPoint.position);
		}

		public override void OnExitState()
		{
			Object.Destroy(dragPoint.gameObject);
		}
	}
}
