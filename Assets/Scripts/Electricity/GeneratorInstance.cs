using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorInstance
{

	#region Internals

	private ElectricGenerator archtype;
	private float runtime = 0F;

	#endregion

	#region Accessors

	public ElectricGenerator Archtype {
		get { return archtype; }
	}

	public float Runtime {
		get { return runtime; }
	}

	#endregion

	#region Constructors

	public GeneratorInstance (ElectricGenerator archtype)
	{
		this.archtype = archtype;
	}

	#endregion

	#region Public Methods

	public void AddRuntime (float seconds)
	{
		runtime += seconds;
		if (runtime > archtype.maxRuntime) {
			runtime = archtype.maxRuntime;
		}
	}

	public void Tick (float deltaTime)
	{
		runtime -= deltaTime;
		if (runtime < 0) {
			runtime = 0;
		}
	}

	#endregion
}
