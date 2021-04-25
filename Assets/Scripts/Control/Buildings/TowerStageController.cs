using System.Globalization;
using UnityEngine;

public abstract class TowerStageController : MonoBehaviour
{
	private MeshFilter meshFilter;
	private float height;

	protected void Awake()
	{
		meshFilter = this.GetComponent<MeshFilter>();
		height = this.ComputeHeight();
	}

	private float ComputeHeight()
	{
		Bounds meshBounds = meshFilter.sharedMesh.bounds;
		return meshBounds.max.z - meshBounds.min.z;
	}

	public float Height { get => height; set => height = value; }
}