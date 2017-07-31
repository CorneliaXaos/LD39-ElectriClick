using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AboutMenuController : MonoBehaviour
{

	#region Public Methods

	public void MenuAction ()
	{
		SceneManager.LoadScene ("Main");
	}

	public void LudumDare ()
	{
		Application.OpenURL ("http://ldjam.com/");
	}

	public void GitHub ()
	{
		Application.OpenURL ("https://github.com/CorneliaXaos/LD39-ElectriClick");
	}

	#endregion
}
