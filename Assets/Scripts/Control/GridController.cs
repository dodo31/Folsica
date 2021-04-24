using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
	private float size;

	private MeshRenderer meshRenderer;

	public GameObject CellPrefab;

	private CellController cellController;

	protected void Start()
	{
		size = this.ComputeSize();

		meshRenderer = this.GetComponent<MeshRenderer>();
		meshRenderer.material.mainTextureScale = Vector2.one * size;

		GameObject highlightCell = Instantiate<GameObject>(CellPrefab);
		highlightCell.transform.SetParent(transform);

		cellController = highlightCell.GetComponent<CellController>();
	}

	private float ComputeSize()
	{
		MeshFilter filter = this.GetComponent<MeshFilter>();
		Bounds gridBounds = filter.sharedMesh.bounds;
		return gridBounds.max.x - gridBounds.min.x;
	}

	public void HighlightCell(Vector2 coordinates, HighlightState state)
	{
		cellController.SetAsNeutral();
	}

	public Vector3 FreePositionToGridPosition(Vector3 position)
	{
		float rowIndex = Mathf.Floor(position.z) + 0.5f;
		float colIndex = Mathf.Floor(position.x) + 0.5f;
		return new Vector3(colIndex, transform.position.z, rowIndex);
	}

	public Vector2 FreePositionToCoordinates(Vector3 position)
	{
		Vector3 positionFromCorner = position + Vector3.one * size * 0.5f;
		float rowIndex = Mathf.Floor(positionFromCorner.y) + 0.5f;
		float colIndex = Mathf.Floor(positionFromCorner.x) + 0.5f;
		return new Vector3(colIndex, rowIndex);
	}
}
