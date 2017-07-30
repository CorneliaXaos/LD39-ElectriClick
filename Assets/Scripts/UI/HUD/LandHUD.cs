using UnityEngine;
using UnityEngine.UI;

public class LandHUD : MonoBehaviour
{

	#region Unity Serializations

	[Header ("UI Components")]
	[SerializeField]
	private Text cost;

	[Header ("Gameplay")]
	[SerializeField]
	private Land land;

	#endregion

	#region Unity Lifecycle Methods

	void Update ()
	{
		cost.text = land.LandCost.ToString ("C");
	}

	#endregion
}
