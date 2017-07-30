using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenuController : MonoBehaviour
{

	#region Unity Serializations

	[Header ("UI Components")]
	[SerializeField]
	private GameObject gameOverMenu;

	[Header ("Gameplay")]
	[SerializeField]
	private World world;

	[SerializeField]
	private Player player;

	[SerializeField]
	private Land land;

	#endregion

	#region Public Methods

	public void SetGameOver (bool gameOver)
	{
		gameOverMenu.SetActive (gameOver);
	}

	public void StartOver ()
	{
		world.Reset ();
		player.Reset ();
		land.Reset ();
		SetGameOver (false);
	}

	public void Quit ()
	{
		// TODO Main menu
	}

	#endregion
}
