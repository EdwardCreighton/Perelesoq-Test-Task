using UnityEngine;
using UnityEngine.Events;

namespace Project_PlayerInteractions.Items
{
	public class Applicable : MonoBehaviour
	{
		#region Fields

		[SerializeField] private int interactionID;
		[SerializeField] private Vector3 targetPosition;
		[SerializeField] private Vector3 targetRotation;
		[Space]
		[SerializeField] private UnityEvent interactionEvent;

		#endregion

		#region Getters

		public int InteractionID => interactionID;
		public Vector3 TargetPosition => targetPosition;
		public Vector3 TargetRotation => targetRotation;

		#endregion

		public void RaiseInteraction()
		{
			interactionEvent.Invoke();
		}
	}
}
