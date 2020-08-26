using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABMainLoop : MonoBehaviour
{
	public ABPawnManager manager;

	public ABMenuPause pauseMenu;


	bool paused = false;
	ABInput input;

	void Start() {
		manager.Init();
		input = new ABInput();
	}

	void Update() {
		if (paused) {

		} else {
			manager.Tick(Time.deltaTime);

			if (input.Consume(KeyCode.P)) {
				StartCoroutine(PauseRoutine());
			}
		}
	}

	IEnumerator PauseRoutine() {
		paused = true;
		yield return pauseMenu.Run(input);
		paused = false;
	}
}
