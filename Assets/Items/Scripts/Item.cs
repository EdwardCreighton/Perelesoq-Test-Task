using System.Collections.Generic;
using UnityEngine;

namespace Project_PlayerInteractions.Items
{
	public class Item : MonoBehaviour
	{
		#region Fields
		
		[SerializeField] private SO_ItemUIInfo itemUIInfo;
		[Space]
		[SerializeField] private Vector3 holdPositionOffset;
		[SerializeField] private Vector3 holdRotationOffset;
		[Space]
		[SerializeField] private Material ghostMaterial;
		[Space]
		[Tooltip("Set -1 if no interactions available for this item")][SerializeField] private int interactionID = -1;
		[Space]
		[SerializeField] private bool isNested;
		[Space]
		[SerializeField] private bool examineEnabled;
		[Space]
		[SerializeField] private float pitchTopLimit = 45f;
		[SerializeField] private float pitchBottomLimit = -45f;
		[Space]
		[SerializeField] private float yawLimitLeft = -180f;
		[SerializeField] private float yawLimitRight = 180f;

		private Mesh mesh;
		private GameObject physicCollider;
		private Transform interactablesTransform;
		private List<Collider> innerInteractablesColliders;
		private Rigidbody rigidbody;

		#endregion

		#region Getters
		
		public SO_ItemUIInfo ItemUIInfo => itemUIInfo;
		
		public Vector3 HoldPositionOffset => holdPositionOffset;
		public Vector3 HoldRotationOffset => holdRotationOffset;

		public bool ExamineEnabled => examineEnabled;
		public float PitchTopLimit => pitchTopLimit;
		public float PitchBottomLimit => pitchBottomLimit;
		public float YawLimitLeft => yawLimitLeft;
		public float YawLimitRight => yawLimitRight;

		public Material GhostMaterial => ghostMaterial;

		public int InteractionID => interactionID;

		public Mesh Mesh => mesh;
		public GameObject PhysicCollider => physicCollider;
		public Transform InteractablesTransform => interactablesTransform;
		public Rigidbody Rigidbody => rigidbody;

		#endregion

		public void OnAwakeComponent()
		{
			mesh = transform.Find("MESH").GetComponent<MeshFilter>().mesh;
			physicCollider = transform.Find("COLLIDER").gameObject;
			rigidbody = GetComponent<Rigidbody>();

			interactablesTransform = transform.Find("INTERACTABLES");
			int count = interactablesTransform.childCount;

			if (count != 0)
			{
				innerInteractablesColliders = new List<Collider>(count);
			
				for (int i = 0; i < count; i++)
				{
					innerInteractablesColliders.Add(interactablesTransform.GetChild(i).GetComponent<Collider>());
					innerInteractablesColliders[i].enabled = false;
				}
			}

			gameObject.SetActive(!isNested);
		}

		public void OnExamineModeToggle(bool examine)
		{
			if (innerInteractablesColliders == null) return;

			foreach (var collider in innerInteractablesColliders)
			{
				collider.enabled = examine;
			}
		}
	}
}
