using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{

	#region Public Methods

	public void StartAction ()
	{
		SceneManager.LoadScene ("Game");
	}

	public void AboutAction ()
	{
		SceneManager.LoadScene ("About");
	}

	public void QuitAction ()
	{
		Application.Quit ();
	}

	#endregion
}
