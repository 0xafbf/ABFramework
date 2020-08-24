using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;

public class ABPawn : MonoBehaviour
{

	float3 physicPosition;
	float3 moveDirection;

	public float speed;
	public Camera cam;

	public void Init() {
		physicPosition = transform.localPosition;
	}

	public void EarlyTick(float deltaTime) {
		float horizontal = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");

		moveDirection = float3(0,0,0);
		moveDirection += horizontal * float3(cam.transform.right);
		moveDirection += vertical * cross(cam.transform.right, transform.up);

		float rotX = Input.GetAxisRaw("Mouse X");
		float3 eulers = transform.localEulerAngles;
		eulers.y += rotX;
		// transform.localEulerAngles = eulers;
	}

	public void FixedTick(float deltaTime) {
		physicPosition += moveDirection * speed * deltaTime;
	}

	public void LateTick(float deltaTime) {
		transform.localPosition = physicPosition;
	}

	public void GUIDebug() {
		GUILayout.BeginVertical("box");
		GUILayout.Label($"Pos: {physicPosition}");

		GUILayout.BeginHorizontal();
		GUILayout.Label($"Speed:");
		float power = GUILayout.Button("-") ? -1 : 0;
		GUILayout.Label($"{speed}");
		power += GUILayout.Button("+") ? 1 : 0;
		if (power != 0) speed *= pow(1.1f, power);
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

	}
}
