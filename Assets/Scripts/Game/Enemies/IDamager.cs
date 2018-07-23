using UnityEngine;

namespace ShootAR.Enemies
{
	public interface IDamager
	{
		void Attack(Collider other);
	}
}
