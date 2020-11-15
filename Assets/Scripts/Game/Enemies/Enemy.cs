using UnityEngine;
using System.Collections;

namespace ShootAR.Enemies
{
	/// <summary>
	/// Parent class of all types of enemies.
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public abstract class Enemy : Spawnable
	{
		[SerializeField] private ulong pointsValue;
		/// <summary>
		/// The amount of points added to the player's score when destroyed.
		/// </summary>
		public ulong PointsValue {
			get { return pointsValue; }
			protected set { pointsValue = value; }
		}
		/// <summary>
		/// The amount of damage the player recieves from this object's attack.
		/// </summary>
		[Range(-Player.MAXIMUM_HEALTH, Player.MAXIMUM_HEALTH), SerializeField]
		private int damage;
		public int Damage { get { return damage; } set { damage = value; } }
		/// <summary>
		/// Count of currently active enemies.
		/// </summary>

		/// <summary>
		/// Dictates if this enemy can or not move due to gameplay status-effects.
		/// </summary>
		public bool CanMove { get; set; } = true;

		public bool IsMoving { get; protected set; } = false;

		/// <summary>Is attacking and moving AI enabled?</summary>
		/// <remarks>Mostly useful in testing.</remarks>
		public bool AiEnabled { get; set; } = true;

		public static int ActiveCount { get; protected set; }

		[SerializeField] protected AudioClip attackSfx;
		[SerializeField] protected GameObject explosion;
		protected AudioSource sfx;
		[SerializeField] protected static ScoreManager score;

		protected virtual void Awake() {
			if (score == null) score = FindObjectOfType<ScoreManager>();
		}

		protected virtual void Start() {
			sfx = GetComponent<AudioSource>();
			/* If the AudioSource component on all enemy prefabs are distinctively
			 * configured, either through scripts or the inspector, and you are
			 * sure of it because you just checked, the following lines can be
			 * removed. */
			sfx.clip = attackSfx;
			sfx.volume = 0.3f;
			sfx.playOnAwake = false;
			sfx.maxDistance = 10f;
		}

		protected virtual void OnEnable() {
			ActiveCount++;
		}

		public override void Destroy() {
			score?.AddScore(PointsValue);
			if (explosion != null)
				Instantiate(explosion, transform.position, transform.rotation);
		}

		protected virtual void OnDisable() {
			ActiveCount--;
		}

		private Coroutine lastMoveAction;

		/// <summary>
		/// Move to a point.
		/// </summary>
		public void MoveTo(Vector3 point) {
			if (!CanMove) return;
			if (IsMoving) StopMoving();

			IEnumerator LerpTo() {
				Vector3 start = transform.position;
				float startTime = Time.time;
				float distance = Vector3.Distance(start, point);
				float moveRatio;

				do {
					if (distance == 0f) break;

					moveRatio = (Time.time - startTime) * Speed / distance;
					if (moveRatio == 0f) {
						yield return new WaitForEndOfFrame();
						continue;
					}

					transform.position = Vector3.Slerp(start, point, moveRatio);
					yield return new WaitForEndOfFrame();
				} while (CanMove && Speed > 0 && transform.position != point);

				IsMoving = false;
			};

			IsMoving = true;
			lastMoveAction = StartCoroutine(LerpTo());
		}

		public void StopMoving() {
			if (lastMoveAction == null) return;

			StopCoroutine(lastMoveAction);
			IsMoving = false;
		}

		/// <summary>
		/// Object orbits around a defined point by an angle based on its speed.
		/// </summary>
		/// <param name="orbit">The orbit to move in</param>
		public void OrbitAround(Orbit orbit) {
			transform.LookAt(orbit.direction, orbit.perpendicularAxis);
			transform.RotateAround(
				orbit.direction, orbit.perpendicularAxis, Speed * Time.deltaTime);
		}

		/// <summary>
		/// Command enemy to attack.
		/// </summary>
		public abstract void Attack();

		public override void ResetState() {
			StopMoving();
			CanMove = true;
		}
	}
}
