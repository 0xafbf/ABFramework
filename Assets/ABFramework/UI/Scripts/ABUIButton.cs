using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ABUIButton : MonoBehaviour
{
	[Header("Config")]
	public Color color = Color.white;
	public string text = "Button";

	[Header("References")]
	public TMP_Text tmpLabel;
	public Text textLabel;
	public Image background;
	public ABNavElement navElement;

	void OnValidate() {
		if (tmpLabel != null) {
			tmpLabel.text = text;
		}
		if (textLabel != null) {
			textLabel.text = text;
		}
		if (background != null) {
			background.color = color;
		}

		if (navElement == null) {
			navElement = GetComponent<ABNavElement>();
		}
	}

	public void OnButtonClick() {
		if (navElement != null) {
			navElement.pendingSubmit = true;
		}
	}

}
