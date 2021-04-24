using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HighlightableController : MonoBehaviour
{
	public ColorTemplate ColorTemplate;

	public Material HighlightMaterialPrefab;

	private MeshRenderer meshRenderer;

	protected void Start()
	{
		meshRenderer = this.GetComponent<MeshRenderer>();
	}

	public void HighlightAsNeutral()
	{
		Material newHighlightMaterial = this.ShowHighlight();
		newHighlightMaterial.color = ColorTemplate.NeutralColor;
	}

	public void HighlightAsValid()
	{
		Material newHighlightMaterial = this.ShowHighlight();
		newHighlightMaterial.color = ColorTemplate.ValidColor;
	}

	public void HighlightAsInvalid()
	{
		Material newHighlightmaterial = this.ShowHighlight();
		newHighlightmaterial.color = ColorTemplate.InvalidColor;
	}

	private Material ShowHighlight()
	{
		Material highlightMaterial = this.FindHighlightMaterial();

		if (highlightMaterial == null)
		{
			highlightMaterial = this.AddHighlightMaterial();
		}

		return highlightMaterial;
	}

	private Material AddHighlightMaterial()
	{
		List<Material> newMaterials = new List<Material>(meshRenderer.materials);
		Material highlightMaterial = Instantiate<Material>(HighlightMaterialPrefab);
		newMaterials.Add(highlightMaterial);
		meshRenderer.materials = newMaterials.ToArray();
		return highlightMaterial;
	}

	public void HideHighlight()
	{
		List<Material> newMaterials = new List<Material>();

		Material highlightMaterial = this.FindHighlightMaterial();

		foreach (Material material in meshRenderer.materials)
		{
			if (material != highlightMaterial)
			{
				newMaterials.Add(material);
			}
		}

		meshRenderer.materials = newMaterials.ToArray();
	}

	private Material FindHighlightMaterial()
	{
		return Array.Find(meshRenderer.materials, (material) =>
		{
			if (this.isHighlightMaterial(material))
			{
				return true;
			}
			else
			{
				return false;
			}
		});
	}

	private bool isHighlightMaterial(Material material)
	{
		return material.name.ToLower().Contains("clone")
			&& material.name.ToLower().Contains("highlight");
	}
}
