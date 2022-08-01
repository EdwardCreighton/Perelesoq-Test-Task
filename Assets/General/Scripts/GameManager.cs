using UnityEngine;
using Project_PlayerInteractions.Player;
using Project_PlayerInteractions.Items;
using Project_PlayerInteractions.UI;

namespace Project_PlayerInteractions
{
	public class GameManager : MonoBehaviour
	{
		#region Fields

		[SerializeField] private PlayerComponent player;
		[Space]
		[SerializeField] private InputListener inputListener;
		[Space]
		[SerializeField] private EventManager eventManager;
		[Space]
		[SerializeField] private InventoryUI inventoryUI;
		[Space]
		[SerializeField] private ItemNameUI itemNameUI;
		[Space]
		[SerializeField] private TipsUI tipsUI;
		[Space]
		[SerializeField] private Crosshair crosshair;

		private InputSchemeType inputType;
		
		public static GameManager ins;

		#endregion

		#region Getters

		public InputSchemeType InputType => inputType;

		#endregion

		private void Awake()
		{
			// TODO: make ICallable interface
			// TODO: send references via EventManager

			ins = this;

			eventManager.OnAwakeComponent();
			
			if (inventoryUI)
			{
				inventoryUI.OnAwakeComponent();
			}

			if (itemNameUI)
			{
				itemNameUI.OnAwakeComponent();
			}

			if (tipsUI)
			{
				tipsUI.OnAwakeComponent();
			}

			if (crosshair)
			{
				crosshair.OnAwakeComponent();
			}
			
			inputListener.OnAwakeComponent();

			player.OnAwakeComponent(inputListener.inputData);

			Item[] items = FindObjectsOfType<Item>();
			foreach (var item in items)
			{
				item.OnAwakeComponent();
			}
			
			EventManager.ins.OnChangeControls += OnChangeControls;
		}

		private void Update()
		{
			if (player)
			{
				player.OnUpdateComponent();
			}
		}
		
		private void OnChangeControls(InputSchemeType type)
		{
			inputType = type;
		}
	}
}
