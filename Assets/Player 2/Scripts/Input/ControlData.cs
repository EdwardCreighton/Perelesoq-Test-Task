using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	public class ControlData
	{
		public Vector3 moveDirection;
		public Vector2 lookDirection;

		public bool interactTrigger;
		public bool interactHold;
		public bool drag;

		public bool switchItemNumber;
		public int itemIndex;

		public float switchItemScroll;

		public bool dropItemTrigger;

		public bool examineTrigger;
	}
}
