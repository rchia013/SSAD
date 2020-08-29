using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CandyCabinets.Components.Colour;

public class OptionController : MonoBehaviour {

	public Toggle Blue;
	public Toggle Red;
	public Toggle Green;
	public Toggle Yellow;

	void Start () {
		// NOTE: make sure all our toggles are set to false in the editor
		string selected = ColourManager.Instance.SelectedPaletteName();
		if (Blue.name == selected) Blue.isOn = true;
		if (Red.name == selected) Red.isOn = true;
		if (Green.name == selected) Green.isOn = true;
		if (Yellow.name == selected) Yellow.isOn = true;
	}
	
	public void OnOptionChanged() {
		if (Blue.isOn)
			ColourManager.Instance.SelectPalette("Blue");

		if (Red.isOn)
			ColourManager.Instance.SelectPalette("Red");

		if (Green.isOn)
			ColourManager.Instance.SelectPalette("Green");

		if (Yellow.isOn)
			ColourManager.Instance.SelectPalette("Yellow");
	}
}
