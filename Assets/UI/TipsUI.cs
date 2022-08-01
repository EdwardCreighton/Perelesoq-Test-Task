using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Project_PlayerInteractions.UI
{
	public class TipsUI : MonoBehaviour
	{
		#region Fields

		[SerializeField] private GameObject tipPrefab;
		[Space]
		[SerializeField] private List<string> interactionText;
		[Space]
		[SerializeField] private List<TipsImageHolder> inputSprites;
		
		private Dictionary<string, GameObject> tipsHolders;
		private InputSchemeType currentInputScheme;

		#endregion

		public void OnAwakeComponent()
		{
			tipsHolders = new Dictionary<string, GameObject>(interactionText.Count);
			foreach (var text in interactionText)
			{
				GameObject tipHolder = Instantiate(tipPrefab, transform);

				tipHolder.transform.Find("TEXT").GetComponent<Text>().text = text;
				
				tipsHolders.Add(text, tipHolder);
				tipHolder.SetActive(false);
			}
			
			OnChangeControls(InputSchemeType.Mouse_Keyboard);

			EventManager.ins.OnToggleTips += ToggleTips;
			EventManager.ins.OnChangeControls += OnChangeControls;
		}

		private void ToggleTips(TipsInfo tipsInfo)
		{
			DisableTips();
			
			if (tipsInfo == null) return;
			
			if (tipsInfo.grab)
			{
				if (tipsHolders.ContainsKey("GRAB"))
				{
					tipsHolders["GRAB"].SetActive(true);
				}
			}
			
			if (tipsInfo.look)
			{
				if (tipsHolders.ContainsKey("LOOK"))
				{
					tipsHolders["LOOK"].SetActive(true);
				}
			}
			
			if (tipsInfo.move)
			{
				if (tipsHolders.ContainsKey("MOVE"))
				{
					tipsHolders["MOVE"].SetActive(true);
				}
			}
			
			if (tipsInfo.interact)
			{
				if (tipsHolders.ContainsKey("INTERACT"))
				{
					tipsHolders["INTERACT"].SetActive(true);
				}
			}

			if (tipsInfo.drop)
			{
				if (tipsHolders.ContainsKey("DROP"))
				{
					tipsHolders["DROP"].SetActive(true);
				}
			}
		}

		private void DisableTips()
		{
			foreach (var tipsHolder in tipsHolders)
			{
				tipsHolder.Value.SetActive(false);
			}
		}

		private void OnChangeControls(InputSchemeType newControls)
		{
			currentInputScheme = newControls;

			foreach (var inputSprite in inputSprites)
			{
				if (inputSprite.InputType == currentInputScheme)
				{
					for (int i = 0; i < interactionText.Count; i++)
					{
						tipsHolders[interactionText[i]].transform.Find("IMAGE").GetComponent<Image>().sprite = inputSprite.Sprites[i];
					}

					return;
				}
			}
		}
	}
}
