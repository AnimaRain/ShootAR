using UnityEngine;
using UnityEngine.UI;

class RoundSelectMenu : MonoBehaviour
{
	private int roundToPlay;
	public int RoundToPlay
	{
		get
		{
			return roundToPlay;
		}

		set
		{
			roundToPlay = Mathf.Clamp(value, 1, 999);
			labelOnUI.text = roundToPlay.ToString();
		}
	}

	/// <summary>
	/// the text label shown in the menu UI
	/// </summary>
	[SerializeField]
	private Text labelOnUI;

	private void OnEnable()
	{
		RoundToPlay = 1;
	}
}
