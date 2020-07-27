using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ShootAR.Menu {
	[RequireComponent(typeof(Text))]
	public class ListElement : MonoBehaviour {
		private static Color unselectedBackground = new Color(0.32f, 0.35f, 0.42f, 0.25f);
		private static Color selectedBackground = new Color(0.54f, 0.44f, 0.32f, 0.25f);

		private static int elementsCount = 0;

		public int ID { get; set; }

		private Text uiText;
		public void SetText(string value) => uiText.text = value;

		private bool selected;

		public bool Selected {
			get => selected;
			set {
				selected = value;

				uiText.GetComponentInChildren<Image>().color =
					value ? selectedBackground : unselectedBackground;

				if (value) {
					GetComponentInParent<ListSelectionController>()
						.ChangeSelection(ID);
				}
			}
		}


		public void Start() {
			if (uiText == null) uiText = GetComponent<Text>();

			ID = elementsCount++;

			SetText(Configuration.Instance.SpawnPattern);
		}
	}
}
