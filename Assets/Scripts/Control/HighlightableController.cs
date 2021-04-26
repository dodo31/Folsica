using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HighlightableController : MonoBehaviour
{
	public ColorTemplate ColorTemplate;

	public Material HighlightMaterialPrefab;

	public void HighlightAsNeutral()
	{
		Material[] newHighlightMaterial = this.ShowHighlight();
		this.SetHightlightColor(newHighlightMaterial, ColorTemplate.NeutralColor);
	}

	public void HighlightAsSelected()
	{
		Material[] newHighlightMaterial = this.ShowHighlight();
		this.SetHightlightColor(newHighlightMaterial, ColorTemplate.SelectedColor);
	}

	public void HighlightAsValid()
	{
		Material[] newHighlightMaterials = this.ShowHighlight();
		this.SetHightlightColor(newHighlightMaterials, ColorTemplate.ValidColor);
	}

	public void HighlightAsInvalid()
	{
		Material[] newHighlightmaterials = this.ShowHighlight();
		this.SetHightlightColor(newHighlightmaterials, ColorTemplate.InvalidColor);
	}

	private void SetHightlightColor(Material[] highlightMaterials, Color color)
	{
		foreach (Material highlightMaterial in highlightMaterials)
		{
			highlightMaterial.color = color;
		}
	}

	private Material[] ShowHighlight()
	{
		MeshRenderer[] meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
		List<Material> highlightMaterials = new List<Material>();

		foreach (MeshRenderer meshRenderer in meshRenderers)
		{
			Material highlightMaterial = this.FindHighlightMaterial(meshRenderer);

			if (highlightMaterial == null)
			{
				highlightMaterial = this.AddHighlightMaterial();
			}

			highlightMaterials.Add(highlightMaterial);
		}

		return highlightMaterials.ToArray();
	}

	private Material AddHighlightMaterial()
	{
		MeshRenderer[] meshRenderers = this.GetComponentsInChildren<MeshRenderer>();
		Material highlightMaterial = Instantiate<Material>(HighlightMaterialPrefab);

		foreach (MeshRenderer meshRenderer in meshRenderers)
		{
			List<Material> newMaterials = new List<Material>(meshRenderer.materials);
			newMaterials.Add(highlightMaterial);
			meshRenderer.materials = newMaterials.ToArray();
		}

		return highlightMaterial;
	}

	public void HideHighlight()
	{
		MeshRenderer[] meshRenderers = this.GetComponentsInChildren<MeshRenderer>();

		foreach (MeshRenderer meshRenderer in meshRenderers)
		{
			List<Material> newMaterials = new List<Material>();
			Material highlightMaterial = this.FindHighlightMaterial(meshRenderer);

			foreach (Material material in meshRenderer.materials)
			{
				if (material != highlightMaterial)
				{
					newMaterials.Add(material);
				}
			}

			meshRenderer.materials = newMaterials.ToArray();
		}
	}

	private Material FindHighlightMaterial(MeshRenderer meshRenderer)
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
