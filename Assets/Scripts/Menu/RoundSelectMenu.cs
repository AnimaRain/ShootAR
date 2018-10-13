using UnityEngine;
using UnityEngine.UI;

namespace ShootAR.Menu
{
	public class RoundSelectMenu : MonoBehaviour
	{
		private int roundToPlay;
		public int RoundToPlay
		{
			get { return roundToPlay; }

			set
			{
				roundToPlay = Mathf.Clamp(value, 1, 999);
				labelOnUI.text = roundToPlay.ToString();
				Configuration.StartingLevel = roundToPlay;
			}
		}

		/// <summary>
		/// the text label shown in the menu UI
		/// </summary>
		[SerializeField]
		private Text labelOnUI;

		private void OnEnable()
		{
			if (RoundToPlay < 1) RoundToPlay = 1;
		}
	} 
}
