using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.CWFanGame
{
	/**<summary>A group of terrain, fluid, other MapLayer objects, and
	 * other objects that interact directly with eachother in a map.</summary>
	 */
	public class MapLayerGroup : MonoBehaviour
	{
		/**<summary>The layer for terrain.</summary>*/
		public MapLayer terrainLayer;
		/**<summary>What draws the terrain layer for this group.</summary>*/
		public TerrainDrawer terrainDrawer;

		/**<summary>Setup the group and the layers.</summary>
		 * <param name="width">Width of the map</param>
		 * <param name="height">Height of the map</param>
		 * <param name="dummyMap">Specify the ID of the dummy/test map configuration to setup</param>
		 */
		public void Setup(int width, int height, int dummyMap = -1)
		{
			terrainLayer.ResizeAndClear(width, height, 1.0f);
			SetupDummyMap(dummyMap);
			terrainDrawer.Setup();
		}

		/**<summary>Configure the layers according to a dummy/test
		 * configuration.</summary>
		 * <param name="mapOption">The dummy configuration ID</param>
		 */
		private void SetupDummyMap(int mapOption)
		{
			switch (mapOption)
			{
				// 4x5
				case 0:
					terrainLayer.depth[0, 0] = 2.0f;
					terrainLayer.depth[1, 1] = 3.0f;
					terrainLayer.depth[2, 2] = 4.0f;
					terrainLayer.depth[3, 3] = 5.0f;
					terrainLayer.depth[0, 3] = 0.0f;
					break;
				default:
					break;
			}
		}
	}
}
