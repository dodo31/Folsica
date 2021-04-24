using UnityEngine;
using UnityEngine.UI;

public abstract class ToggleButton : MonoBehaviour
{
	public Image IconImage;

	private Button button;

	private void Awake()
	{
		button = this.GetComponent<Button>();
	}

	public void ToggleInterractable(bool setInterractable)
	{
		if (setInterractable)
		{
			this.SetOpacity(1);
		}
		else
		{
			this.SetOpacity(0.5f);
		}

		button.interactable = setInterractable;
	}

	protected void SetOpacity(float opacity)
	{
		Image[] images = this.GetComponentsInChildren<Image>();

		foreach (Image image in images)
		{
			Color imageColor = image.color;
			image.color = new Color(imageColor.r, imageColor.g, imageColor.b, opacity);
		}
	}
}