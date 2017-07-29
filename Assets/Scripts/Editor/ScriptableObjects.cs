using System.IO;
using UnityEditor;
using UnityEngine;

public class ScriptableObjects
{

	[MenuItem ("Assets/Create/LD39/Electric Generator")]
	public static void CreateElectricGenerator ()
	{
		CreateAsset<ElectricGenerator> ("Generator.asset");
	}

	// INTERNALS

	// Swiped from another project because this is just so damn useful in creating
	// a scriptable object asset file where you want it to be.  It should be fine
	// copy-pasting this... I could have typed it manually.. but it's just a
	// productivity tool more than anything.
	private static void CreateAsset<T> (string file) where T : ScriptableObject
	{
		T asset = ScriptableObject.CreateInstance<T> ();

		string path = AssetDatabase.GenerateUniqueAssetPath (CreatePath (file));
		ProjectWindowUtil.CreateAsset (asset, path);
	}

	private static string CreatePath (string file)
	{
		string path = AssetDatabase.GetAssetPath (Selection.activeObject);
		if (path == string.Empty) {
			path = "Assets";
		} else if (Path.GetExtension (path) != string.Empty) {
			path = path.Replace (Path.GetFileName (AssetDatabase.GetAssetPath (Selection.activeObject)), string.Empty);
		}
		return path + "/" + file;
	}
}
