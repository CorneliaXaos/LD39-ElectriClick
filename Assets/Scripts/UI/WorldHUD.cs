using UnityEngine;
using UnityEngine.UI;

public class WorldHUD : MonoBehaviour
{

	#region Unity Serializations

	[Header ("UI Components")]
	[SerializeField]
	private Text year;

	[SerializeField]
	private Text population;

	[Header ("Gameplay")]
	[SerializeField]
	private World world;

	#endregion

	#region Unity Lifecycle Methods

	void Update ()
	{
		year.text = ((int)(world.CurrentTimeYears)) + " (Years)";
		population.text = ((long)(world.Population)) + " (Population)";
	}

	#endregion
}
