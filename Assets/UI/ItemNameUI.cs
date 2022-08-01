using UnityEngine;
using UnityEngine.UI;

namespace Project_PlayerInteractions.UI
{
	public class ItemNameUI : MonoBehaviour
	{
		#region Fields

		private GameObject holder;
		private Text textField;

		#endregion
		
		public void OnAwakeComponent()
		{
			holder = transform.GetChild(0).gameObject;
			textField = holder.transform.GetChild(1).GetComponent<Text>();
			
			holder.SetActive(false);

			EventManager.ins.OnInteractableFocus += OnInteractableFocus;
		}

		private void OnInteractableFocus(string interactableName = null)
		{
			if (interactableName != null)
			{
				holder.SetActive(true);
				textField.text = interactableName;
			}
			else
			{
				holder.SetActive(false);
			}
		}
	}
}
