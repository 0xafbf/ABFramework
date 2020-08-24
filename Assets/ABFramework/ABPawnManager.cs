using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABPawnManager : MonoBehaviour
{

	public ABPawn pawn;
	public ABEnemy enemy;

	float remainingTime = 0;
	public float timeStep = 0.1f;
	public float timeScale = 1.0f;
	bool running = true;

	const float MIN_TIMESTEP = 0.01f;

	public void Init() {
		pawn.Init();
		enemy.Init();
	}

	public void Tick(float deltaTime) {

		if (timeStep < MIN_TIMESTEP) {
			timeStep = MIN_TIMESTEP;
		}

		pawn.EarlyTick(deltaTime);

		if (running) {
			remainingTime += deltaTime * timeScale;
		}

		while (remainingTime > timeStep) {
			FixedTick(timeStep);
			remainingTime -= timeStep;
		}

		pawn.LateTick(deltaTime);
		enemy.LateTick(deltaTime);

		ABDraw.BatchDraw();

		if (Input.GetKeyDown(KeyCode.F1)) {
			drawDebug = !drawDebug;
		}
	}

	public void FixedTick(float deltaTime) {
		ABDraw.TickLines(deltaTime);
		Physics.SyncTransforms();
		pawn.FixedTick(deltaTime);
		enemy.FixedTick(deltaTime);

	}

	bool drawDebug = false;
	void OnGUI() {

		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal("box");

		string runText = running ? "||" : "|>";
		if (GUILayout.Button(runText)) {
			running = !running;
		}
		if (GUILayout.Button("||>")) {
			FixedTick(timeStep);
		}

		GUILayout.Label($"Current timescale: {timeScale}");
		if (GUILayout.Button("x2")) {
			timeScale *= 2;
		}

		if (GUILayout.Button("x0.5")) {
			timeScale *= 0.5f;
		}


		GUILayout.EndHorizontal();

		if (drawDebug) {
			GUILayout.BeginHorizontal();
			pawn.GUIDebug();
			// enemy.GUIDebug();

			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		GUILayout.EndVertical();

	}

}
