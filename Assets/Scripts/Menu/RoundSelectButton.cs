using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A button used in the round selection menu.
/// (Attach it to a button)
/// </summary>
[RequireComponent(typeof(Button))]
public class RoundSelectButton : MonoBehaviour
{
	/// <summary>
	/// the number labelled on the button
	/// </summary>
	private int numberOnLabel;

	private RoundSelectMenu menu;

	private void Start()
	{
		menu = FindObjectOfType<RoundSelectMenu>();
		if (menu == null)
		{
			Debug.LogError("Round Select Menu not found!");
			return;
		}

		string label = GetComponentInChildren<Text>().text;
		if (int.TryParse(label, out numberOnLabel))
		{
			GetComponent<Button>().onClick.AddListener(ChangeLevelIndex);
		}
		else
			Debug.LogError($"{label} is not an acceptable number. Try labeling" +
				" the button with an integer and without using spaces, letters " +
				"and other symbols. ('+', '-', '.' and 'e' are allowed)");
	}

	/// <summary>
	/// Takes the label of the button, converts it into a number and adds it to 
	/// the level index.
	/// </summary>
	public void ChangeLevelIndex()
	{
		menu.RoundToPlay += numberOnLabel;
	}
}
