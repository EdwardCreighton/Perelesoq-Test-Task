using Project_PlayerInteractions.UI;
using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	public abstract class Abstract_State
	{
		#region Fields

		protected PlayerFSMController controller;
		
		protected Vector3 rayOrigin;
		protected Vector3 rayDirection;
		protected Ray ray;
		protected RaycastHit hitInfo;

		protected TipsInfo tipsInfo;

		#endregion
		
		protected void SetRay()
		{
			rayOrigin = controller.Player.CameraTransform.position;
			rayDirection = controller.Player.CameraTransform.forward;
			
			ray.origin = rayOrigin;
			ray.direction = rayDirection;
		}
		
		protected void Raycast()
		{
			Physics.Raycast(ray, out hitInfo, controller.Player.InteractionMaxDistance);
		}

		protected void SphereCast()
		{
			Physics.SphereCast(ray, controller.Player.RayThickness, out hitInfo, controller.Player.InteractionMaxDistance);
		}
		
		public abstract void OnEnterState(PlayerFSMController controller);
		public abstract void OnUpdate();
		public abstract void OnExitState();
	}
}
