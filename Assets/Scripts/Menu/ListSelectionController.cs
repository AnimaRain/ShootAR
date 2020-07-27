using UnityEngine;
using System;

namespace ShootAR.Menu {
	public class ListSelectionController : MonoBehaviour
	{
		private ListElement selected;

		public void Start() {
			//TODO: Fetch from configurations' file which pattern is selected.
		}

		public void ChangeSelection(int id) {
			if (selected?.ID == id) return;

			if (selected != null)
				selected.Selected = false;

			selected = Array.Find(
				GetComponentsInChildren<ListElement>(),
				element => element.ID == id
			);
		}
	}
}
