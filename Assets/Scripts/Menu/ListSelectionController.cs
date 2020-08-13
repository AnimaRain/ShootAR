using UnityEngine;
using System;

namespace ShootAR.Menu {
	public class ListSelectionController : MonoBehaviour
	{
		private ListElement selected;

		public void Start() {
			// Populate list with patterns from directory.
			foreach(string name in Configuration.Instance.SpawnPatterns) {
				GameObject newContent = Instantiate(
					Resources.Load<GameObject>(Prefabs.PATTERN_LIST_CONTENT)
				);

				// Put in correct place under gameobject hierarchy.
				newContent.transform.SetParent(transform, false);

				newContent.GetComponent<ListElement>().SetText(name);
			}

			// Fetch from configurations' file which pattern is selected.
			ChangeSelection(Configuration.Instance.SpawnPatternSlot);

			/* Change background color of the selected item on the list.
			(This will cause ChangeSelection to be called again, but its
			initial check will cause it to return early anyway.) */
			selected.Selected = true;
		}

		public void ChangeSelection(uint id) {
			if (selected?.Id == id) return;

			// Unselect the previous selected
			if (selected != null)
				selected.Selected = false;

			selected = Array.Find(
				GetComponentsInChildren<ListElement>(),
				element => element.Id == id
			);

			Configuration.Instance.SpawnPatternSlot = id;
		}
	}
}
