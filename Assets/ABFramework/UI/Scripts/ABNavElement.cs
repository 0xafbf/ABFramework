using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ABNavElement : MonoBehaviour {


	[Header("References")]
	public Animator navAnimator;

	////// Output data
	[NonSerialized] public bool pendingSubmit;

	static readonly int animIdle = Animator.StringToHash("Idle");
	static readonly int animFocus = Animator.StringToHash("Focus");
	static readonly int animPressed = Animator.StringToHash("Pressed");

	public void SetFocus(bool inFocus) {
		if (navAnimator == null) return;
		navAnimator.Play(inFocus ? animFocus : animIdle);
	}

	// TODO: request focus when hovering object

	public bool HandleInput(ABInput input) {
		if (input.Consume(ABInput.Select)) {
			pendingSubmit = true;
			return true;
		}
		return false;
	}

	public bool ConsumeSubmit() {
		bool r = pendingSubmit;
		pendingSubmit = false;
		return r;
	}
}
