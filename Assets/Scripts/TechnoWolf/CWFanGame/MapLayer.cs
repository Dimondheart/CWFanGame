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
	}
}
