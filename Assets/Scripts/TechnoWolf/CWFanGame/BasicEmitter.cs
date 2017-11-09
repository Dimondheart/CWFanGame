using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.CWFanGame
{
	/**<summary>Basic emitter which adds/generates depth into a single layer.</summary>*/
	public class BasicEmitter : MonoBehaviour
	{
		/**<summary>Time (in seconds) between when this emitter is enabled
		 * and the first emission.</summary>
		 */
		public float delay = 0.0f;
		/**<summary>The interval (in seconds) between emissions.</summary>*/
		public float interval = 1.0f;
		/**<summary>Depth/amount to generate each emission.</summary>*/
		public float amount = 20.0f;
		/**<summary>The layer group to emit into.</summary>*/
		public MapLayerGroup emitIntoGroup;
		/**<summary>The specific layer type to emit into.</summary>*/
		public MapLayerGroup.LayerType emitIntoLayer =
			MapLayerGroup.LayerType.Creeper;

		/**<summary>The time at which the last emission occurred.</summary>*/
		private float lastEmitTime = float.PositiveInfinity;

		/**<summary>The layer object to emit into.</summary>*/
		protected MapLayer EmitIntoLayer
		{
			get
			{
				switch (emitIntoLayer)
				{
					case MapLayerGroup.LayerType.Creeper:
						return emitIntoGroup.creeperLayer;
					case MapLayerGroup.LayerType.AntiCreeper:
						return emitIntoGroup.antiCreeperLayer;
					case MapLayerGroup.LayerType.Water:
						return emitIntoGroup.waterLayer;
					case MapLayerGroup.LayerType.Terrain:
						return emitIntoGroup.terrainLayer;
					default:
						Debug.LogError("Unhandled Layer Type:" + emitIntoLayer);
						return null;
				}
			}
		}
		/**<summary>The map x coordinate of this emitter.</summary>*/
		public int TileX
		{
			get
			{
				return Mathf.RoundToInt(transform.localPosition.x);
			}
			set
			{
				transform.localPosition = new Vector3(value, transform.localPosition.y, transform.localPosition.z);
			}
		}
		/**<summary>The map y coordinate of this emitter.</summary>*/
		public int TileY
		{
			get
			{
				return Mathf.RoundToInt(transform.localPosition.y);
			}
			set
			{
				transform.localPosition = new Vector3(transform.localPosition.x, value, transform.localPosition.z);
			}
		}

		public void Start()
		{
			lastEmitTime = Time.time - interval + delay - 0.00001f;
		}

		public void Update()
		{
			if (CanEmit(Time.time))
			{
				EmitIntoLayer.depth[TileX, TileY] +=
					CalculateEmissionAmount(Time.time - lastEmitTime);
				lastEmitTime = Time.time;
			}
		}

		/**<summary>Check if this emitter can/will emit at the given time,
		 * not accounting for the possibility of intermediate emissions.
		 * The result is always false if this script has not been fully
		 * initialized.</summary>
		 * <param name="atTime">The Unity Time.time to check/test</param>
		 */
		public bool CanEmit(float atTime)
		{
			return atTime - lastEmitTime >= interval;
		}

		/**<summary>Calculate the actual amount to be emitted for a given amount
		 * of time, ignoring interval time and delays but constraining to
		 * EmptySpaceForEmission().</summary>
		 * <param name="actualTimePassed">The amount of time passed</param>
		 */
		public float CalculateEmissionAmount(float actualTimePassed)
		{
			return Mathf.Min(actualTimePassed * amount, EmptySpaceForEmission());
		}

		/**<summary>The amount of empty space in the tile(s) the emitter puts
		 * the generated fluid into.</summary>
		 */
		public float EmptySpaceForEmission()
		{
			if (float.IsPositiveInfinity(emitIntoGroup.ceilingHeight))
			{
				return float.PositiveInfinity;
			}
			if (emitIntoGroup.terrainLayer.depth[TileX, TileY] >= emitIntoGroup.ceilingHeight
				|| Mathf.Approximately(
					emitIntoGroup.terrainLayer.depth[TileX, TileY],
					emitIntoGroup.ceilingHeight
					)
			)
			{
				return 0.0f;
			}
			float totalDepth =
				emitIntoGroup.terrainLayer.depth[TileX, TileY]
				+ emitIntoGroup.waterLayer.depth[TileX, TileY]
				+ Mathf.Max(
					emitIntoGroup.creeperLayer.depth[TileX, TileY],
					emitIntoGroup.antiCreeperLayer.depth[TileX, TileY]
					);
			if (totalDepth >= emitIntoGroup.ceilingHeight)
			{
				return 0.0f;
			}
			return emitIntoGroup.ceilingHeight - totalDepth;
		}
	}
}
