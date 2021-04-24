using UnityEngine;

public class CellController : MonoBehaviour
{
	public ColorTemplate ColorTemplate;

	private MeshRenderer meshRenderer;

	protected void Awake()
	{
		meshRenderer = this.GetComponent<MeshRenderer>();
	}

	public void SetAsNeutral()
	{
		this.SetColor(ColorTemplate.NeutralColor);
	}

	public void SetAsValid()
	{
		this.SetColor(ColorTemplate.ValidColor);
	}

	public void SetAsInvalid()
	{
		this.SetColor(ColorTemplate.InvalidColor);
	}

	private void SetColor(Color color)
	{
		meshRenderer.material.color = color;
	}
}