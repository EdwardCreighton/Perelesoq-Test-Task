using Project_PlayerInteractions.Items;
using UnityEngine;
using UnityEngine.UI;

namespace Project_PlayerInteractions.UI
{
	public class Crosshair : MonoBehaviour
	{
		private Image image;

		public void OnAwakeComponent()
		{
			image = GetComponent<Image>();
			
			EventManager.ins.OnExamineToggle += OnExamineToggle;
		}

		private void OnExamineToggle(bool state)
		{
			image.enabled = !state;
		}
	}
}
