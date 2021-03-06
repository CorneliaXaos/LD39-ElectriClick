﻿using UnityEngine;
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

	[SerializeField]
	private Text chargeRate;

	[SerializeField]
	private GameOverMenuController gameOverController;

	[Header ("Gameplay")]
	[SerializeField]
	private Player player;

	#endregion

	#region Unity Lifecycle Methods

	void Update ()
	{
		float satisfaction = (float)((player.SatisfactionClamped + 1D) / 2D);
		demandSatisfaction.SetProgress (satisfaction);

		float repVal = (player.Reputation + 1F) / 2F;
		reputation.SetProgress (repVal);
		gameOverController.SetGameOver (repVal <= 0F);

		finances.text = player.Finances.ToString ("C");

		chargeRate.text = "Charge: " + player.DollarsPerWattYear.ToString ("C");
	}

	#endregion

	#region Public Methods

	public void IncrementCharge ()
	{
		player.DollarsPerWattYear = player.DollarsPerWattYear + 1;
	}

	public void DecrementCharge ()
	{
		player.DollarsPerWattYear = player.DollarsPerWattYear - 1;
	}

	#endregion
}
