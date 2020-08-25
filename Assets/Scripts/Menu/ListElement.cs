using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ShootAR.Menu {
	[RequireComponent(typeof(Text))]
	public class ListElement : MonoBehaviour {
		private static Color unselectedBackground = new Color(0.32f, 0.35f, 0.42f, 0.25f);
		private static Color selectedBackground = new Color(0.54f, 0.44f, 0.32f, 0.25f);

		// Used to auto-generate id number for each element.
		private static uint elementsCount = 0;

		public uint Id { get; set; }

		[SerializeField] private Text uiText;
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
						.ChangeSelection(Id);
				}
			}
		}


		public void Awake() {
			Id = elementsCount++;
			/* Do not initialize Id on declaration, because Unity does
			unexpected things and "elementCount++" gets executed one
			additional time, resulting with elementCount being one number
			higher than expected. */
		}
	}
}
