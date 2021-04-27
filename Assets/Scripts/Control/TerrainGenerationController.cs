using UnityEngine;

public class TerrainGenerationController : MonoBehaviour
{
	public float tileCount = 123;

	public int sideCount = 32;

	public float tileSize = 2.4f;

	protected void Start()
	{
		this.Generate();
	}

	public void Generate()
	{
		float cursorStartX = -sideCount * tileSize * 0.5f;
		float cursorStartY = -sideCount * tileSize * 0.5f;

		Map map = new Map();

		for (int x = 0; x < sideCount; x++)
		{
			float cursorX = cursorStartX + x * tileSize;

			for (int y = 0; y < sideCount; y++)
			{
				float cursorY = cursorStartY + y * tileSize;

				Vector3 cursorPosition = new Vector3(cursorX, 0, cursorY);

				int tileId = map.Disposition[y, sideCount - x - 1];

				if (tileId > 0)
				{
					string tilePath = "Models/Tiles 2/" + tileId;

					GameObject tileObjectPrefab = Resources.Load<GameObject>(tilePath);
					GameObject tileObject = Instantiate<GameObject>(tileObjectPrefab);
					// tileObject.transform.Rotate(Vector3.up, 90, Space.World);

					MeshFilter tileFilter = tileObject.GetComponentInChildren<MeshFilter>();
					tileFilter.sharedMesh.RecalculateBounds();
					Bounds tileBounds = tileFilter.sharedMesh.bounds;

					tileObject.name = tileId.ToString();
					tileObject.transform.position = cursorPosition - tileBounds.min - new Vector3(0, 4.3f, 0);
					tileObject.transform.SetParent(transform);

					if (tileId == 123)
					{
						tileObject.transform.Translate(0, -0.1f, 0);
					}
				}
			}
		}

		transform.Rotate(0, -90, 0, Space.Self);
	}
}