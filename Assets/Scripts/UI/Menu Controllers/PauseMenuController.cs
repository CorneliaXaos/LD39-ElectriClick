using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{

	#region Unity Serializations

	[Header ("UI Components")]
	[SerializeField]
	private GameObject pauseMenu;

	[Header ("Gameplay")]
	[SerializeField]
	private World world;

	[SerializeField]
	private Player player;

	[SerializeField]
	private Land land;

	#endregion

	#region Public Methods

	public void SetPaused (bool paused)
	{
		world.Paused = paused;
		pauseMenu.SetActive (paused);
	}

	public void StartOver ()
	{
		world.Reset ();
		player.Reset ();
		land.Reset ();
		SetPaused (false);
	}

	public void Quit ()
	{
		SceneManager.LoadScene ("Main");
	}

	#endregion
}
