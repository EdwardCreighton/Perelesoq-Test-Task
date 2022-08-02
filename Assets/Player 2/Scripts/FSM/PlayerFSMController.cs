using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	public class PlayerFSMController
	{
		#region Fields

		private PlayerComponent player;

		#endregion

		#region Getters

		public PlayerComponent Player => player;

		#endregion
		
		#region States

		private Abstract_State currentState;

		public readonly Abstract_State searchState = new Search_State();
		
		public readonly Abstract_State itemFocusState = new ItemFocus_State();
		public readonly Abstract_State holdingItemState = new HoldingItem_State();
		public readonly Abstract_State dropItemState = new DropItem_State();
		public readonly Abstract_State useItemState = new UseItem_State();
		
		public readonly Abstract_State interactableFocusState = new InteractableFocus_State();
		
		public readonly Abstract_State moveableFocusState = new MoveableFocus_State();
		public readonly Abstract_State moveObjectState = new MoveObject_State();

		public readonly Abstract_State examineState = new Examine_State();

		#endregion

		public PlayerFSMController(PlayerComponent playerComponent)
		{
			player = playerComponent;
			
			MoveToState(searchState);
		}
		
		public void OnUpdate()
		{
			currentState.OnUpdate();
		}

		public void MoveToState(Abstract_State nextState)
		{
			currentState?.OnExitState();

			currentState = nextState;
			currentState.OnEnterState(this);
		}

		public void MoveCharacter()
		{
			Vector3 motion;

			if (player.CharacterController.isGrounded)
			{
				motion = player.MovementSpeed * player.ControlData.moveDirection;
			}
			else
			{
				Vector3 horizontalMovement = player.MovementSpeed / 2f * player.ControlData.moveDirection;
				Vector3 verticalMovement = Vector3.down * (player.CharacterController.velocity.y + player.GravityForce);
				motion = horizontalMovement + verticalMovement;
			}

			player.CharacterController.Move(motion * Time.deltaTime);
		}

		public void LookAround()
		{
			if (player.ControlData.lookDirection.sqrMagnitude < 0.01f) return;
			
			player.currentYaw += player.ControlData.lookDirection.x * player.YawSensitivity * Time.deltaTime;
			player.currentPitch += player.ControlData.lookDirection.y * player.PitchSensitivity * (player.InverseY ? -1f : 1f) * Time.deltaTime;
			
			ClampYaw();
			player.currentPitch = Mathf.Clamp(player.currentPitch, player.BottomClamp, player.TopClamp);
			
			player.CameraTransform.localRotation = Quaternion.Euler(player.currentPitch, 0f, 0f);
			player.transform.rotation = Quaternion.Euler(0f, player.currentYaw, 0f);
		}
		
		private void ClampYaw()
		{
			if (player.currentYaw >= 360f)
			{
				player.currentYaw -= 360f;
			}
			else if (player.currentYaw <= -360f)
			{
				player.currentYaw += 360f;
			}
		}
	}
}
