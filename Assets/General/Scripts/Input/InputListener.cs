using UnityEngine;
using UnityEngine.InputSystem;

namespace Project_PlayerInteractions
{
	public class InputListener : MonoBehaviour
	{
		public InputData inputData { get; private set; }
		public void OnAwakeComponent()
		{
			inputData = new InputData();
		}
		
		public void OnMove(InputValue value)
		{
			inputData.moveInput = value.Get<Vector2>();
		}

		public void OnLook(InputValue value)
		{
			inputData.lookInput = value.Get<Vector2>();
		}
		
		public void OnInteract(InputValue value)
		{
			inputData.interact = value.isPressed;
		}

		public void OnDropItem(InputValue value)
		{
			inputData.drop = value.isPressed;
		}

		public void OnSwitchItems(InputValue value)
		{
			inputData.switchItemsScroll = value.Get<float>();
		}

		public void OnExamineItem(InputValue value)
		{
			inputData.examine = value.isPressed;
		}

		public void OnDragObject(InputValue value)
		{
			inputData.drag = value.isPressed;
		}

		#region Numbers Input

		public void On_1(InputValue value)
		{
			inputData.itemNumberPressed = value.isPressed;
			inputData.itemIndex = 0;
		}

		public void On_2(InputValue value)
		{
			inputData.itemNumberPressed = value.isPressed;
			inputData.itemIndex = 1;
		}

		public void On_3(InputValue value)
		{
			inputData.itemNumberPressed = value.isPressed;
			inputData.itemIndex = 2;
		}

		public void On_4(InputValue value)
		{
			inputData.itemNumberPressed = value.isPressed;
			inputData.itemIndex = 3;
		}

		public void On_5(InputValue value)
		{
			inputData.itemNumberPressed = value.isPressed;
			inputData.itemIndex = 4;
		}

		public void On_6(InputValue value)
		{
			inputData.itemNumberPressed = value.isPressed;
			inputData.itemIndex = 5;
		}

		public void On_7(InputValue value)
		{
			inputData.itemNumberPressed = value.isPressed;
			inputData.itemIndex = 6;
		}

		public void On_8(InputValue value)
		{
			inputData.itemNumberPressed = value.isPressed;
			inputData.itemIndex = 7;
		}

		public void On_9(InputValue value)
		{
			inputData.itemNumberPressed = value.isPressed;
			inputData.itemIndex = 8;
		}

		public void On_10(InputValue value)
		{
			inputData.itemNumberPressed = value.isPressed;
			inputData.itemIndex = 9;
		}

		#endregion

		public void OnControlsChanged(PlayerInput input)
		{
			if (!EventManager.ins) return;
			
			switch (input.currentControlScheme)
			{
				case "MouseKeyboard":
				{
					EventManager.ins.RaiseOnChangeControls(InputSchemeType.Mouse_Keyboard);
					break;
				}
				case "PS4 Controller":
				{
					EventManager.ins.RaiseOnChangeControls(InputSchemeType.PS4_Controller);
					break;
				}
			}
		}
	}
}
