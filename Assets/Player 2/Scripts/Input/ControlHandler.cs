using UnityEngine;

namespace Project_PlayerInteractions.Player
{
	public class ControlHandler
	{
		#region Fields

		private float controlSwitchItemsTimer;
		
		private Transform playerTransform;

		private InputData inputData;
		private InputData lastInputData;

		private ControlData controlData;

		#endregion

		#region Getters

		public ControlData ControlData => controlData;

		#endregion
		
		public ControlHandler(Transform playerTransform, InputData inputData)
		{
			this.playerTransform = playerTransform;
			this.inputData = inputData;
			
			lastInputData = new InputData();
			controlData = new ControlData();
		}

		public void OnUpdateComponent()
		{
			UpdateControls();
			
			SetLastFrameInput();
		}

		private void UpdateControls()
		{
			controlData.moveDirection = playerTransform.TransformDirection(inputData.moveInput.x, 0f, inputData.moveInput.y);
			controlData.lookDirection = inputData.lookInput;

			controlData.interactTrigger = OneClick(inputData.interact, lastInputData.interact);
			controlData.interactHold = inputData.interact;
			
			controlData.drag = inputData.drag;

			controlData.switchItemNumber = OneClick(inputData.itemNumberPressed, lastInputData.itemNumberPressed);
			controlData.itemIndex = inputData.itemIndex;
			
			if (controlSwitchItemsTimer >= 0.2f)
			{
				controlData.switchItemScroll = inputData.switchItemsScroll;
				
				if (Mathf.Abs(inputData.switchItemsScroll) > 0.1f)
				{
					controlSwitchItemsTimer = 0f;
				}
			}
			else
			{
				controlData.switchItemScroll = 0f;
				controlSwitchItemsTimer += Time.deltaTime;
			}

			controlData.dropItemTrigger = OneClick(inputData.drop, lastInputData.drop);

			controlData.examineTrigger = OneClick(inputData.examine, lastInputData.examine);
		}
		
		private bool OneClick(bool currentInput, bool lastFrameInput)
		{
			if (currentInput && !lastFrameInput)
			{
				return true;
			}
			
			return false;
		}

		private void SetLastFrameInput()
		{
			lastInputData.moveInput = inputData.moveInput;
			lastInputData.lookInput = inputData.lookInput;
			lastInputData.interact = inputData.interact;
			lastInputData.switchItemsScroll = inputData.switchItemsScroll;
			lastInputData.itemNumberPressed = inputData.itemNumberPressed;
			lastInputData.itemIndex = inputData.itemIndex;
			lastInputData.drop = inputData.drop;
			lastInputData.examine = inputData.examine;
			lastInputData.drag = inputData.drag;
		}
	}
}
