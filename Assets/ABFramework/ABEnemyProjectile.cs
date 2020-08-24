using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;

public class ABEnemyProjectile : MonoBehaviour
{
	public float speed;
	public float3 physicPosition;
	public SphereCollider coll;

	public float lifetime;
	float currentLifetime;
	[NonSerialized] public bool dead;

	public void Init() {
		transform.position = physicPosition;
		currentLifetime = 0;
		dead = false;
	}

	public void FixedTick(float deltaTime) {

		float3 direction = transform.forward;
		float distance = speed * deltaTime;

		RaycastHit hit;
		float3 deltaPos;
		Color lineColor = Color.white;
		if (Physics.SphereCast(physicPosition, coll.radius, direction, out hit, distance)) {
			deltaPos = direction * (hit.distance - 0.01f);
			lineColor = Color.red;
		} else {
			deltaPos = direction * distance;
		}

		physicPosition += deltaPos;

		float3 start = physicPosition + float3(transform.forward) * coll.radius;
		float3 end = start + deltaPos;
		ABDraw.Line(start, end, lineColor);


		currentLifetime += deltaTime;
		if (currentLifetime > lifetime) {
			dead = true;
		}

	}

	public float smoothAmount = 4;
	public void LateTick(float deltaTime) {
		float3 smoothPosition = lerp(transform.position, physicPosition, deltaTime * smoothAmount);
		transform.position = smoothPosition;
	}

	public void OnDrawGizmos() {
		Gizmos.color = Color.magenta;
		Gizmos.DrawWireSphere(physicPosition, coll.radius);
	}
}
