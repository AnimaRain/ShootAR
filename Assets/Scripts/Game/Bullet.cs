using UnityEngine;

//#pragma warning disable 649   //the unassigned fields are actually assigned in Unity Editor

namespace ShootAR
{
	public class Bullet : MonoBehaviour, ISpawnable
	{
		[SerializeField] private float Speed { get; }
		/// <summary>
		/// Total count of spawned bullets during the current round.
		/// </summary>
		public static int Count { private set; get; }
		/// <summary>
		/// Count of currently active bullets.
		/// </summary>
		public static int ActiveCount { get; private set; }
		private static TVScript tvScreen;
		private static AudioSource shotSfx;
		private UnityEngine.UI.Text countText;

		protected void Start()
		{
			if (tvScreen == null)
				tvScreen = GameObject.FindGameObjectWithTag("TVScreen").GetComponent<TVScript>();
			if (shotSfx == null)
				shotSfx = GetComponent<AudioSource>();
			transform.rotation = Camera.main.transform.rotation;
			transform.position = Vector3.zero;
			shotSfx.Play();
			GetComponent<Rigidbody>().velocity = transform.forward * Speed;

			Count++;
			ActiveCount++;
			countText.text = Count.ToString();
		}

		protected void OnTriggerEnter(Collider col)
		{
			if (col.CompareTag("Enemy") || col.CompareTag("Capsule"))
			{
				Destroy(col.gameObject);
				Destroy(gameObject);
			}
			if (col.gameObject.tag == "Remote")
			{
				if (tvScreen.tvOn)
				{
					tvScreen.CloseTV();
				}
				else
				{
					tvScreen.StartTV();
				}
			}
		}

		protected void OnDestroy()
		{
			ActiveCount--;
		}
	}
}