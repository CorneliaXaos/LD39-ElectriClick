using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{

	#region Unity Serializations

	[Header ("Defaults")]
	[SerializeField]
	[Range (0, 1)]
	private float startingProgress = 0.75F;

	[Header ("Behaviour")]
	[SerializeField]
	private bool autoColor = true;
	[SerializeField]
	private bool useThreeColors = false;

	[Header ("AutoColor")]
	[SerializeField]
	private Color emptyColor = Color.red;
	[SerializeField]
	private Color halfwayColor = Color.yellow;
	[SerializeField]
	private Color fullColor = Color.green;

	[Header ("Prefab Components - Don't Modify!")]
	[SerializeField]
	private Image barScaled;

	#endregion

	#region Unity Lifecycle Methods

	private void OnValidate ()
	{
		SetProgress (startingProgress);
	}

	#endregion

	#region Public Methods

	public void SetProgress (float progress)
	{
		Assert.IsTrue (progress >= 0F && progress <= 1F);

		Vector3 scale = new Vector3 (progress, 1, 1);
		barScaled.transform.localScale = scale;

		// If at least the two required colors are provided we'll color the bar automatically.
		if (autoColor) {
			barScaled.color = DetermineColor (progress);
		}
	}

	#endregion

	#region Private Methods

	private Color DetermineColor (float progress)
	{
		if (useThreeColors) {
			return DetermineTriColor (progress);
		} else {
			return DetermineBiColor (progress);
		}
	}

	private Color DetermineBiColor (float progress)
	{
		return Color.Lerp (emptyColor, fullColor, progress);
	}

	private Color DetermineTriColor (float progress)
	{
		float scaledProgress = progress * 2;

		if (scaledProgress <= 1F) {
			return Color.Lerp (emptyColor, halfwayColor, scaledProgress);
		} else {
			return Color.Lerp (halfwayColor, fullColor, scaledProgress - 1);
		}
	}

	#endregion
}
