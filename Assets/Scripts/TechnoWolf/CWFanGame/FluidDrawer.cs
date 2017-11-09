using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.CWFanGame
{
	/**<summary></summary>*/
	public class FluidDrawer : MonoBehaviour
	{
		public GameObject fluidTile;
		public MapLayer fluidLayer;
		public float minAlpha = 0.0f;
		public float maxAlpha = 1.0f;

		public GameObject[,] fluidTiles;

		public void Update()
		{
			UpdateTiles();
		}

		public void Setup()
		{
			if (fluidTiles != null)
			{
				foreach (GameObject go in fluidTiles)
				{
					if (go != null)
					{
						GameObject.Destroy(go);
					}
				}
			}
			fluidTiles = new GameObject[fluidLayer.Width, fluidLayer.Height];
			for (int x = 0; x < fluidLayer.Width; x++)
			{
				for (int y = 0; y < fluidLayer.Height; y++)
				{
					fluidTiles[x, y] = Instantiate(fluidTile, transform) as GameObject;
					fluidTiles[x, y].transform.localPosition = new Vector3(x, y, 0.0f);
				}
			}
			UpdateTiles();
		}

		private void UpdateTiles()
		{
			Color baseColor = fluidTile.GetComponent<SpriteRenderer>().color;
			Color min = new Color(baseColor.r, baseColor.g, baseColor.b, minAlpha);
			Color max = new Color(baseColor.r, baseColor.g, baseColor.b, maxAlpha);
			for (int x = 0; x < fluidLayer.Width; x++)
			{
				for (int y = 0; y < fluidLayer.Height; y++)
				{
					SpriteRenderer sr = fluidTiles[x, y].GetComponent<SpriteRenderer>();
					if (fluidLayer.depth[x, y] > 0.0049f)
					{
						sr.color = Color.Lerp(min, max, Mathf.Clamp((int)fluidLayer.depth[x, y], 0.0f, 20.0f) / 20.0f);
					}
					else
					{
						sr.color = Color.clear;
					}
				}
			}
		}
	}
}
