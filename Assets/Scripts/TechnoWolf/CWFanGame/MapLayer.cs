using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.CWFanGame
{
	/**<summary>Per tile depth for terrain, fluids, and anything else that
	 * covers the entire map and has a volatile per-tile depth.</summary>
	 */
	public class MapLayer : MonoBehaviour
	{
		/**<summary>The depth/thickness of this layer for each tile.</summary>*/
		public float[,] depth { get; private set; }
		/**<summary>The change in depth of this layer for each tile for the current
		 * cycle. This is applied and reset at the end of each tile update
		 * cycle.</summary>
		 */
		public float[,] deltaDepth { get; private set; }
		/**<summary>The width (in tiles) of this layer. Has a value of
		 * -1 if the layer has not been initialized.</summary>
		 */
		public int Width
		{
			get
			{
				return depth == null ? -1 : depth.GetLength(0);
			}
		}
		/**<summary>The height (in tiles) of this layer. Has a value of
		 * -1 if the layer has not been initialized.</summary>
		 */
		public int Height
		{
			get
			{
				return depth == null ? -1 : depth.GetLength(1);
			}
		}

		/**<summary>Calculate the sum of depths for the given layers for each
		 * tile.</summary>
		 * <param name="layers">The layers to calculate the total depth of</param>
		 * <returns>The per tile sums of the depths of the layers</returns>
		 */
		public static float[,] CalculateTotalDepths(params MapLayer[] layers)
		{
			float[,] result = new float[layers[0].Width, layers[0].Height];
			for (int x = 0; x < layers[0].Width; x++)
			{
				for (int y = 0; y < layers[0].Height; y++)
				{
					float val = layers[0].depth[x, y];
					for (int ln = 1; ln < layers.Length; ln++)
					{
						val += layers[ln].depth[x, y];
					}
					result[x, y] = val;
				}
			}
			return result;
		}

		public void FixedUpdate()
		{
			ApplyDeltaDepths();
		}

		/**<summary>Clear the layer, setting all tile depths to the same
		 * value and all depth deltas to 0.</summary>
		 * <param name="value">The value to set all tile depths to</param>
		 */
		public void Clear(float value = 0.0f)
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					depth[x, y] = value;
					deltaDepth[x, y] = 0.0f;
				}
			}
		}

		/**<summary>Resize and clear the layer.</summary>
		 * <param name="width">New width, in tiles</param>
		 * <param name="height">New height, in tiles</param>
		 * <param name="value">Clear depth to this value for each tile</param>
		 */
		public void ResizeAndClear(int width, int height, float value = 0.0f)
		{
			if (width != Width || height != Height)
			{
				depth = new float[width, height];
				deltaDepth = new float[width, height];
			}
			Clear(value);
		}

		/**<summary>Applies the depth deltas and resets all depth deltas to 0.</summary>*/
		public void ApplyDeltaDepths()
		{
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					if (deltaDepth[x, y] != 0.0f)
					{
						depth[x, y] += deltaDepth[x, y];
					}
					deltaDepth[x, y] = 0.0f;
				}
			}
		}

		/**<summary>Calculates depth deltas due to fluid movement and adds them to
		 * the current depth deltas.</summary>
		 * <param name="totalDepth">The depth of this layer plus all lower layers</param>
		 * <param name="lossRate">Fration of a tiles depth that can flow out of the tile
		 * per second (lossRate * depth = max depth loss due to fluid motion per
		 * second)</param>
		 * <param name="deltaTime">Time passed since the last fluid update</param>
		 */
		public void SimulateFluidMotion(MapLayer terrain, float[,] totalDepth, float lossRate, float deltaTime)
		{
			FluidMovementData data = new FluidMovementData();
			data.terrainLayer = terrain;
			data.totalDepth = totalDepth;
			data.lossRate = lossRate;
			data.deltaTime = deltaTime;
			for (int x = 0; x < Width; x++)
			{
				for (int y = 0; y < Height; y++)
				{
					if (depth[x, y] < 0.02f)
					{
						continue;
					}
					float[,] fluidDeltas = CalculateFluidMotion(x, y, data);
					deltaDepth[x, y] += fluidDeltas[1, 1];
					if (x > 0 && fluidDeltas[0, 1] != 0.0f)
					{
						deltaDepth[x - 1, y] += fluidDeltas[0, 1];
					}
					if (x < Width - 1 && fluidDeltas[2, 1] != 0.0f)
					{
						deltaDepth[x + 1, y] += fluidDeltas[2, 1];
					}
					if (y > 0 && fluidDeltas[1, 0] != 0.0f)
					{
						deltaDepth[x, y - 1] += fluidDeltas[1, 0];
					}
					if (y < Height - 1 && fluidDeltas[1, 2] != 0.0f)
					{
						deltaDepth[x, y + 1] += fluidDeltas[1, 2];
					}
				}
			}
		}

		/**<summary>Calculate the amount of fluid movement at and around a
		 * tile.</summary>
		 * <param name="x">The x coordinate of the tile</param>
		 * <param name="y">The y coordinate of the tile</param>
		 * <param name="data">The information needed to calculate the values</param>
		 * <returns>A 3x3 array of the deltas, with the center tile being at
		 * [1, 1]</returns>
		 */
		public float[,] CalculateFluidMotion(int x, int y, FluidMovementData data)
		{
			float[,] deltas = new float[3, 3];
			System.Array.Clear(deltas, 0, deltas.Length);
			float totalChange = 0.0f;
			float maxLoss = depth[x, y] * data.lossRate;
			float centerTD = data.totalDepth[x, y];
			float otherTD = 0.0f;
			if (x > 0 && !Mathf.Approximately(0.0f, data.terrainLayer.depth[x - 1, y]))
			{
				otherTD = data.totalDepth[x - 1, y];
				if (centerTD > otherTD)
				{
					totalChange += centerTD - otherTD;
					deltas[0, 1] = float.PositiveInfinity;
				}
			}
			if (x < Width - 1 && !Mathf.Approximately(0.0f, data.terrainLayer.depth[x + 1, y]))
			{
				otherTD = data.totalDepth[x + 1, y];
				if (centerTD > otherTD)
				{
					totalChange += centerTD - otherTD;
					deltas[2, 1] = float.PositiveInfinity;
				}
			}
			if (y > 0 && !Mathf.Approximately(0.0f, data.terrainLayer.depth[x, y - 1]))
			{
				otherTD = data.totalDepth[x, y - 1];
				if (centerTD > otherTD)
				{
					totalChange += centerTD - otherTD;
					deltas[1, 0] = float.PositiveInfinity;
				}
			}
			if (y < Height - 1 && !Mathf.Approximately(0.0f, data.terrainLayer.depth[x, y + 1]))
			{
				otherTD = data.totalDepth[x, y + 1];
				if (centerTD > otherTD)
				{
					totalChange += centerTD - otherTD;
					deltas[1, 2] = float.PositiveInfinity;
				}
			}
			float centerLoss = 0.0f;
			if (maxLoss >= totalChange)
			{
				if (deltas[0, 1] > 1.0f)
				{
					deltas[0, 1] = centerTD - data.totalDepth[x - 1, y];
					centerLoss += deltas[0, 1];
				}
				if (deltas[2, 1] > 1.0f)
				{
					deltas[2, 1] = centerTD - data.totalDepth[x + 1, y];
					centerLoss += deltas[2, 1];
				}
				if (deltas[1, 0] > 1.0f)
				{
					deltas[1, 0] = centerTD - data.totalDepth[x, y - 1];
					centerLoss += deltas[1, 0];
				}
				if (deltas[1, 2] > 1.0f)
				{
					deltas[1, 2] = centerTD - data.totalDepth[x, y + 1];
					centerLoss += deltas[1, 2];
				}
			}
			else
			{
				if (deltas[0, 1] > 1.0f)
				{
					deltas[0, 1] = (centerTD - data.totalDepth[x - 1, y]) / totalChange * maxLoss;
					centerLoss += deltas[0, 1];
				}
				if (deltas[2, 1] > 1.0f)
				{
					deltas[2, 1] = (centerTD - data.totalDepth[x + 1, y]) / totalChange * maxLoss;
					centerLoss += deltas[2, 1];
				}
				if (deltas[1, 0] > 1.0f)
				{
					deltas[1, 0] = (centerTD - data.totalDepth[x, y - 1]) / totalChange * maxLoss;
					centerLoss += deltas[1, 0];
				}
				if (deltas[1, 2] > 1.0f)
				{
					deltas[1, 2] = (centerTD - data.totalDepth[x, y + 1]) / totalChange * maxLoss;
					centerLoss += deltas[1, 2];
				}
			}
			deltas[1, 1] = -centerLoss;
			for (int xx = 0; xx < 3; xx++)
			{
				for (int yy = 0; yy < 3; yy++)
				{
					if (deltas[xx, yy] != 0.0f)
					{
						deltas[xx, yy] *= data.deltaTime;
					}
				}
			}
			return deltas;
		}

		/**<summary>Data used in fluid movement calculations.</summary>*/
		public class FluidMovementData
		{
			/**<summary>The terrain layer in the same group as the layer this data
			 * applies to.</summary>
			 */
			public MapLayer terrainLayer;
			/**<summary>The total depth of the layer group up to the top of the layer
			 * this data applies to.</summary>
			 */
			public float[,] totalDepth;
			/**<summary>The length of time the fluid moved/the time passed since the
			 * last fluid update for this layer.</summary>
			 */
			public float deltaTime;
			/**<summary>The fraction of the depth of a tile in this layer that is the
			 * max amount of fluid that can move out of the tile. This value is
			 * independent of the amount of time passed.</summary>
			 */
			public float lossRate;
		}
	}
}
