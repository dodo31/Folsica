using System.Globalization;
using UnityEngine;

public abstract class TowerStageController : MonoBehaviour
{
	public int Price = 0;

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
		return meshBounds.max.y - meshBounds.min.y;
	}

	protected Vector3 HostTowerPosition()
	{
		return transform.parent.transform.position;
	}

	public float Height { get => height; set => height = value; }
}