using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.CWFanGame
{
	/**<summary>Draws the terrain layer.</summary>*/
	public class TerrainDrawer : MonoBehaviour
	{
		/**<summary>Terrain level 1 prefab.</summary>*/
		public GameObject terrain1;
		/**<summary>Terrain level 2 prefab.</summary>*/
		public GameObject terrain2;
		/**<summary>Terrain level 3 prefab.</summary>*/
		public GameObject terrain3;
		/**<summary>Terrain level 4 prefab.</summary>*/
		public GameObject terrain4;
		/**<summary>Terrain level 5 prefab.</summary>*/
		public GameObject terrain5;
		/**<summary>Terrain level 6 prefab.</summary>*/
		public GameObject terrain6;
		/**<summary>Terrain level 7 prefab.</summary>*/
		public GameObject terrain7;
		/**<summary>Terrain level 8 prefab.</summary>*/
		public GameObject terrain8;
		/**<summary>Terrain level 9 prefab.</summary>*/
		public GameObject terrain9;
		/**<summary>Terrain level 10 prefab.</summary>*/
		public GameObject terrain10;
		/**<summary>Terrain layer to draw.</summary>*/
		public MapLayer terrainLayer;

		/**<summary>Depth of the terrain as currently rendered/last update.</summary>*/
		private float[,] terrainDepths = null;
		/**<summary>GameObjects for each tile game object that renders a tile.</summary>*/
		private GameObject[,] terrainTiles = null;

		public void Update()
		{
			UpdateTiles();
		}

		/**<summary>Setup the terrain drawer.</summary>*/
		public void Setup()
		{
			terrainDepths = new float[terrainLayer.Width, terrainLayer.Height];
			for (int x = 0; x < terrainLayer.Width; x++)
			{
				for (int y = 0; y < terrainLayer.Height; y++)
				{
					terrainDepths[x, y] = -1.0f;
				}
			}
			if (terrainTiles != null)
			{
				foreach (GameObject t in terrainTiles)
				{
					GameObject.Destroy(t);
				}
			}
			terrainTiles = new GameObject[terrainLayer.Width, terrainLayer.Height];
			for (int x = 0; x < terrainLayer.Width; x++)
			{
				for (int y = 0; y < terrainLayer.Height; y++)
				{
					terrainTiles[x, y] = null;
				}
			}
			UpdateTiles();
		}

		/**<summary>Update the physical tiles and other rendering stuff.</summary>*/
		private void UpdateTiles()
		{
			if (terrainTiles == null)
			{
				return;
			}
			for (int x = 0; x < terrainLayer.Width; x++)
			{
				for (int y = 0; y < terrainLayer.Height; y++)
				{
					if (terrainDepths[x, y] != terrainLayer.depth[x, y])
					{
						if (terrainTiles[x, y] != null)
						{
							GameObject.Destroy(terrainTiles[x, y]);
						}
						float newDepth = terrainLayer.depth[x, y];
						if (newDepth < 0.5f)
						{
							terrainTiles[x, y] = null;
						}
						else if (newDepth < 1.5f)
						{
							terrainTiles[x, y] = Instantiate(terrain1, transform) as GameObject;
						}
						else if (newDepth < 2.5f)
						{
							terrainTiles[x, y] = Instantiate(terrain2, transform) as GameObject;
						}
						else if (newDepth < 3.5f)
						{
							terrainTiles[x, y] = Instantiate(terrain3, transform) as GameObject;
						}
						else if (newDepth < 4.5f)
						{
							terrainTiles[x, y] = Instantiate(terrain4, transform) as GameObject;
						}
						else if (newDepth < 5.5f)
						{
							terrainTiles[x, y] = Instantiate(terrain5, transform) as GameObject;
						}
						else if (newDepth < 6.5f)
						{
							terrainTiles[x, y] = Instantiate(terrain6, transform) as GameObject;
						}
						else if (newDepth < 7.5f)
						{
							terrainTiles[x, y] = Instantiate(terrain7, transform) as GameObject;
						}
						else if (newDepth < 8.5f)
						{
							terrainTiles[x, y] = Instantiate(terrain8, transform) as GameObject;
						}
						else if (newDepth < 9.5f)
						{
							terrainTiles[x, y] = Instantiate(terrain9, transform) as GameObject;
						}
						else if (newDepth < 10.5f)
						{
							terrainTiles[x, y] = Instantiate(terrain10, transform) as GameObject;
						}
						if (terrainTiles[x, y] != null)
						{
							terrainTiles[x, y].transform.localPosition = new Vector3(x, y, 0.0f);
						}
						terrainDepths[x, y] = newDepth;
					}
				}
			}
		}
	}
}
