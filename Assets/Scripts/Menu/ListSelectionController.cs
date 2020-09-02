using UnityEngine;
using System;

namespace ShootAR.Menu {

	public class ListSelectionController : MonoBehaviour
	{
		private ListElement selected;

		public void Start() {
			// Populate list with patterns from directory.
			var itemTemplate = Resources.Load<GameObject>(Prefabs.PATTERN_LIST_CONTENT);
			foreach(string name in Configuration.Instance.SpawnPatterns) {
				GameObject newListItem = Instantiate(itemTemplate);

				// Put in correct place under gameobject hierarchy.
				newListItem.transform.SetParent(transform, false);

				newListItem.GetComponent<ListElement>().SetText(name);
			}

			// Fetch from configurations' file which pattern is selected.
			selected = Array.Find(
				GetComponentsInChildren<ListElement>(),
				element => element.Id == Configuration.Instance.SpawnPatternSlot
			);

			// Change background color of the selected item on the list.
			selected.Selected = true;
		}

		public void ChangeSelection(int id) {
			// Unselect the previous selected
			if (selected != null && selected.Id != id)
				selected.Selected = false;

			selected = Array.Find(
				GetComponentsInChildren<ListElement>(),
				element => element.Id == id
			);

			Configuration.Instance.SpawnPatternSlot = id;
		}

		public void DeleteSelected() {
			// Do not allow deleting last item on list.
			if (Configuration.Instance.SpawnPatterns.Length == 1) return;

			var deleted = selected; // the item to be deleted

			// Switch selection to an existing item on the list.
			if (Configuration.Instance.SpawnPatternSlot != 0) {
				ChangeSelection(Configuration.Instance.SpawnPatternSlot - 1);
			}
			else {
				ChangeSelection(1);
			}

			// Move all items in the list after the deleted one spot up.
			foreach (ListElement item in GetComponentsInChildren<ListElement>())
				if (item.Id > deleted.Id) item.DecrementId();

			// Temporarily set the to-be-deleted item as selected
			// so it will get deleted by DeletePattern().
			var newSelected = selected;
			selected = deleted;

			// Remove item from list.
			/* (Don't use Destroy() here, because the object does not get destroyed
			in time, and results in a null reference being used later.) */
			DestroyImmediate(selected.gameObject);

			/* Deleting the pattern recreates the list, ruining existing
			references to objects, so all functions that require them must
			run before, and new references to be created for after. */
			Configuration.Instance.DeletePattern();

			// Reset selected.
			selected = newSelected;

			selected.Selected = true;
		}
	}
}
