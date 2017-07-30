using UnityEngine;
using UnityEngine.UI;

public class DropdownDisabler : MonoBehaviour
{

	#region Unity Serializations

	[SerializeField]
	private Text text;

	[SerializeField]
	private Toggle toggle;

	#endregion

	#region Unity Lifecycle Methods

	void Update ()
	{
		toggle.interactable = !text.text.StartsWith ("*");
	}

	#endregion
}
