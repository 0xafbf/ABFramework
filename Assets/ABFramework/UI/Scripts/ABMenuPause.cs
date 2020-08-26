using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Mathematics;
using static Unity.Mathematics.math;


public enum ABNavType {
	Horizontal,
	Vertical,
	Tabs,
}

public class ABInput {

	public const KeyCode Pause = KeyCode.P;
	public const KeyCode Select = KeyCode.Return;
	public const KeyCode Cancel = KeyCode.Escape;

	public bool Consume(KeyCode key) {
		return Input.GetKeyDown(key);
	}

	public int ConsumeNav(ABNavType navType) {
		int output = 0;
		if (navType == ABNavType.Vertical) {
			if (Input.GetKeyDown(KeyCode.UpArrow)) output -= 1;
			if (Input.GetKeyDown(KeyCode.DownArrow)) output += 1;

			if (Input.GetKeyDown(KeyCode.W)) output -= 1;
			if (Input.GetKeyDown(KeyCode.S)) output += 1;
		}
		return output;
	}
}

[Serializable]
public class ABUINavContext {
	public ABNavType navType;
	public bool wrap = false;

	[NonSerialized] public ABNavElement[] navElements;
	// if -1, use full navElements array, read with GetNumElements
	[NonSerialized] public int numElements = -1;

	public int GetNumElements() => numElements != -1 ? numElements : navElements.Length;


	int focusedElement = -1;

	public void PushInput(ABInput input) {
		int nav = input.ConsumeNav(navType);
		if (nav != 0) {
			NavigateDelta(nav);
			return;
		}
		navElements[focusedElement].HandleInput(input);
	}

	public void NavigateDelta(int delta) {
		int elements = GetNumElements();
		int newIndex = focusedElement + delta;
		if (wrap) {
			newIndex = (newIndex + elements) % elements;
		} else {
			newIndex = clamp(newIndex, 0, elements-1);
		}
		NavigateTo(newIndex);
	}

	public void NavigateTo(int newFocus) {
		if (newFocus == focusedElement) return;

		if (focusedElement != -1) {
			navElements[focusedElement].SetFocus(false);
		}
		// should ensure to focus first item
		focusedElement = newFocus;
		if (focusedElement != -1) {
			navElements[focusedElement].SetFocus(true);
		}
	}
}

public class ABMenuPause : MonoBehaviour
{

	public ABUINavContext navContext;
	public ABNavElement btnContinue;
	public ABNavElement btnSettings;
	public ABNavElement btnExit;

	bool init = false;
	void Init() {
		var nav = new ABNavElement[3];
		nav[0] = btnContinue;
		nav[1] = btnSettings;
		nav[2] = btnExit;
		navContext.navElements = nav;
	}

	public IEnumerator Run(ABInput input) {
		if (!init) Init();
		navContext.NavigateTo(0);
		gameObject.SetActive(true);
		bool running = true;
		while (running) {
			yield return null;

			navContext.PushInput(input);

			if (btnContinue.ConsumeSubmit()) {
				running = false;
			}

			if (btnSettings.ConsumeSubmit()) {
				Debug.LogError("UNIMPLEMENTED");
			}

			if (btnExit.ConsumeSubmit()) {
				Debug.LogError("EXIT");
			}

			if (input.Consume(ABInput.Pause)) {
				running = false;
			}
		}
		gameObject.SetActive(false);
	}
}
