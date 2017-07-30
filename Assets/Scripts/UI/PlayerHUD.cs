using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{

	#region Unity Serializations

	[Header ("UI Components")]
	[SerializeField]
	private StatusBar demandSatisfaction;

	[SerializeField]
	private StatusBar reputation;

	[SerializeField]
	private Text finances;

	[Header ("Gameplay")]
	[SerializeField]
	private Player player;

	#endregion

	#region Unity Lifecycle Methods

	void Update ()
	{
		float satisfaction = (float)((player.SatisfactionClamped + 1D) / 2D);
		demandSatisfaction.SetProgress (satisfaction);
		reputation.SetProgress ((player.Reputation + 1F) / 2F);
		finances.text = player.Finances.ToString ("C");
	}

	#endregion
}
