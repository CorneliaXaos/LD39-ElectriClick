using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to interact with Unity UI and render a generator instance
/// </summary>
public class GeneratorDisplay : MonoBehaviour
{

	#region Unity Serializations

	#endregion

	#region Internals

	private GeneratorInstance instance;

	#endregion

	#region Unity Lifecycle Methods

	private void Start ()
	{
		
	}

	private void Update ()
	{
		
	}

	#endregion

	#region Public Methods

	public void Bind (GeneratorInstance instance)
	{
		this.instance = instance;
	}

	#endregion
}
