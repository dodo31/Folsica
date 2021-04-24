using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{
	public GameObject CellPrefab;

	private CellController cellController;

	protected void Start()
	{
		GameObject highlightCell = Instantiate<GameObject>(CellPrefab);
		highlightCell.transform.SetParent(transform);

		cellController = highlightCell.GetComponent<CellController>();
	}

	public void HighlightCell(Vector2 cursorPosition, HighlightState state)
	{
		cellController.SetAsNeutral();
	}
}
