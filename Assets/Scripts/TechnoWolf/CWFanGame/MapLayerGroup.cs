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
		/**<summary>The layer for water.</summary>*/
		public MapLayer waterLayer;
		/**<summary>The layer for creeper.</summary>*/
		public MapLayer creeperLayer;
		/**<summary>What draws the terrain layer for this group.</summary>*/
		public TerrainDrawer terrainDrawer;
		/**<summary>What draws the water layer.</summary>*/
		public FluidDrawer waterDrawer;
		/**<summary>What draws the creeper layer.</summary>*/
		public FluidDrawer creeperDrawer;
		/**<summary>Emitter prefab for testing purposes.</summary>*/
		public GameObject emitter;
		/**<summary>Color of an emitter putting out water.</summary>*/
		public Color waterEmitterColor = Color.cyan;
		/**<summary>Color of an emitter putting out creeper.</summary>*/
		public Color creeperEmitterColor = Color.blue;

		public void FixedUpdate()
		{
			waterLayer.SimulateFluidMotion(
				terrainLayer,
				MapLayer.CalculateTotalDepths(terrainLayer, waterLayer),
				0.249f,
				Time.fixedDeltaTime
				);
			creeperLayer.SimulateFluidMotion(
				terrainLayer,
				MapLayer.CalculateTotalDepths(terrainLayer, waterLayer, creeperLayer),
				0.1f,
				Time.fixedDeltaTime
				);
		}

		/**<summary>Setup the group and the layers.</summary>
		 * <param name="width">Width of the map</param>
		 * <param name="height">Height of the map</param>
		 * <param name="dummyMap">Specify the ID of the dummy/test map configuration to setup</param>
		 */
		public void Setup(int width, int height, int dummyMap = -1)
		{
			terrainLayer.ResizeAndClear(width, height, 4.0f);
			waterLayer.ResizeAndClear(width, height, 0.0f);
			creeperLayer.ResizeAndClear(width, height, 0.0f);
			SetupDummyMap(dummyMap);
			terrainDrawer.Setup();
			waterDrawer.Setup();
			creeperDrawer.Setup();
		}

		/**<summary>Configure the layers according to a dummy/test
		 * configuration.</summary>
		 * <param name="mapOption">The dummy configuration ID</param>
		 */
		private void SetupDummyMap(int mapOption)
		{
			GameObject e = null;
			switch (mapOption)
			{
				case 0:
					e = Instantiate(emitter, transform) as GameObject;
					e.transform.localPosition = new Vector3(10.0f, 10.0f, 0.0f);
					e.GetComponent<TestEmitter>().amount = 200.0f;
					e.GetComponent<TestEmitter>().interval = 4.0f;
					e.GetComponent<TestEmitter>().delay = 2.0f;
					e.GetComponent<TestEmitter>().emitIntoLayer = waterLayer;
					e.GetComponent<SpriteRenderer>().color = waterEmitterColor;
					break;
				case 1:
					e = Instantiate(emitter, transform) as GameObject;
					e.transform.localPosition = new Vector3(1.0f, 4.0f, 0.0f);
					e.GetComponent<TestEmitter>().amount = 20.0f;
					e.GetComponent<TestEmitter>().interval = 2.0f;
					e.GetComponent<TestEmitter>().delay = 1.0f;
					e.GetComponent<TestEmitter>().emitIntoLayer = waterLayer;
					e.GetComponent<SpriteRenderer>().color = waterEmitterColor;
					e = Instantiate(emitter, transform) as GameObject;
					e.transform.localPosition = new Vector3(4.0f, 4.0f, 0.0f);
					e.GetComponent<TestEmitter>().amount = 20.0f;
					e.GetComponent<TestEmitter>().interval = 1.0f;
					e.GetComponent<TestEmitter>().delay = 1.0f;
					e.GetComponent<TestEmitter>().emitIntoLayer = creeperLayer;
					e.GetComponent<SpriteRenderer>().color = creeperEmitterColor;
					terrainLayer.depth[10, 0] = 10.0f;
					terrainLayer.depth[10, 1] = 10.0f;
					terrainLayer.depth[10, 2] = 10.0f;
					terrainLayer.depth[10, 3] = 10.0f;
					terrainLayer.depth[10, 4] = 10.0f;
					terrainLayer.depth[10, 5] = 10.0f;
					terrainLayer.depth[10, 6] = 10.0f;
					terrainLayer.depth[10, 7] = 10.0f;
					terrainLayer.depth[10, 8] = 10.0f;
					terrainLayer.depth[10, 9] = 10.0f;
					terrainLayer.depth[0, 10] = 10.0f;
					terrainLayer.depth[1, 10] = 10.0f;
					terrainLayer.depth[2, 10] = 10.0f;
					terrainLayer.depth[3, 10] = 10.0f;
					terrainLayer.depth[4, 10] = 10.0f;
					terrainLayer.depth[5, 10] = 10.0f;
					terrainLayer.depth[6, 10] = 10.0f;
					terrainLayer.depth[7, 10] = 10.0f;
					terrainLayer.depth[8, 10] = 10.0f;
					terrainLayer.depth[9, 10] = 10.0f;
					terrainLayer.depth[10, 10] = 10.0f;
					terrainLayer.depth[0, 6] = 7.0f;
					terrainLayer.depth[1, 6] = 7.0f;
					terrainLayer.depth[2, 6] = 7.0f;
					terrainLayer.depth[3, 6] = 7.0f;
					terrainLayer.depth[4, 6] = 7.0f;
					break;
				default:
					break;
			}
		}
	}
}
