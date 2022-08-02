using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	[RequireComponent(typeof(CharacterController))]
	public class PlayerComponent : MonoBehaviour
	{
		#region Fields

		[Header("General")]
		[SerializeField] private float movementSpeed = 5f;
		[SerializeField] private float gravityForce = -6f;

		[Space(20f, order = 0)]
		[Header("Camera Settings", order = 1)]
		[SerializeField] private Transform cameraTransform;
		[Space]
		[SerializeField] private float topClamp = 90f;
		[SerializeField] private float bottomClamp = -90f;
		[Space]
		[SerializeField] private float yawSensitivity = 10f;
		[SerializeField] private float pitchSensitivity = 14f;
		[Space]
		[SerializeField] private bool inverseY = true;
		[HideInInspector] public float currentYaw;
		[HideInInspector] public float currentPitch;

		[Space(20f, order = 0)]
		[Header("Interactions Settings", order = 1)]
		[SerializeField] private Transform itemHolderPoint;
		[SerializeField] private Transform itemExaminePoint;
		[Space]
		[SerializeField] private float itemSearchRayThickness = 0.35f;
		[SerializeField] private float interactionMaxDistance = 5f;
		[Space]
		[SerializeField] private float dragForce = 10f;

		private ControlHandler controlHandler;
		private Inventory inventory;

		private PlayerFSMController fsmController;
		private CharacterController characterController;

		#endregion

		#region Getters

		public float MovementSpeed => movementSpeed;
		public float GravityForce => gravityForce;

		public Transform CameraTransform => cameraTransform;
		public float TopClamp => topClamp;
		public float BottomClamp => bottomClamp;
		public float YawSensitivity => yawSensitivity;
		public float PitchSensitivity => pitchSensitivity;
		public bool InverseY => inverseY;

		public Transform ItemHolderPoint => itemHolderPoint;
		public Transform ItemExaminePoint => itemExaminePoint;
		public float RayThickness => itemSearchRayThickness;
		public float InteractionMaxDistance => interactionMaxDistance;
		public float DragForce => dragForce;

		public Inventory Inventory => inventory;

		public CharacterController CharacterController => characterController;
		public ControlData ControlData => controlHandler.ControlData;

		#endregion

		public void OnAwakeComponent(InputData inputData)
		{
			characterController = GetComponent<CharacterController>();

			controlHandler = new ControlHandler(transform, inputData);
			fsmController = new PlayerFSMController(this);
			inventory = new Inventory(this);
		}

		public void OnUpdateComponent()
		{
			controlHandler.OnUpdateComponent();
			
			fsmController.OnUpdate();
		}
	}
}
