using UnityEngine;
using UnityEngine.Events;

namespace Project_PlayerInteractions.Items
{
	public class Interactable : MonoBehaviour
	{
		#region Fields

		[SerializeField] private string name;
		[Space(20f)]
		[SerializeField] private UnityEvent interactionEvent;

		#endregion

		#region Getters

		public string Name => name;

		#endregion

		public void RaiseInteraction()
		{
			interactionEvent.Invoke();
		}
	}
}
