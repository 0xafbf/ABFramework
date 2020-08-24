using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABEnemy : MonoBehaviour
{

	public ABEnemyProjectile projectileTemplate;
	ABEnemyProjectile projectile;

	public void Init() {
		projectile = Instantiate(projectileTemplate);
		ResetProjectile(projectile);
	}
	public void ResetProjectile(ABEnemyProjectile proj) {
		proj.physicPosition = transform.position;
		proj.Init();
	}

	public void FixedTick(float deltaTime) {
		projectile.FixedTick(deltaTime);
		if (projectile.dead) {
			ResetProjectile(projectile);
		}
	}

	public void LateTick(float deltaTime) {
		projectile.LateTick(deltaTime);
	}
}
