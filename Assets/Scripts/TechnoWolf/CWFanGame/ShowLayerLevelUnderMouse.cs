using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TechnoWolf.CWFanGame
{
	/**<summary>Show the level of a layer under the mouse cursor, both the
	 * individual layer depth and the depth including lower layers.</summary>
	 */
	public class ShowLayerLevelUnderMouse : MonoBehaviour
	{
		/**<summary>The UI element to show the text in.</summary>*/
		public Text levelText;
		/**<summary>The layer to show depth for.</summary>*/
		public MapLayer layer;
		/**<summary>Layers below the layer this script is showing depth for,
		 * in order to calculate the total height.</summary>
		 */
		public MapLayer[] lowerLayers;

		/**<summary>Prefix string, taken from the initial text of the UI element.</summary>*/
		private string prefix;

		public void Start()
		{
			prefix = levelText.text;
		}

		public void Update()
		{
			Vector3 position = GameObject.FindGameObjectWithTag("MainCamera")
				.GetComponent<Camera>()
				.ScreenToWorldPoint(Input.mousePosition);
			int x = Mathf.RoundToInt(position.x);
			int y = Mathf.RoundToInt(position.y);
			if (x >= 0 && x < layer.Width && y >= 0 && y < layer.Height)
			{
				float totalDepth = layer.depth[x, y];
				foreach(MapLayer ll in lowerLayers)
				{
					totalDepth += ll.depth[x, y];
				}
				levelText.text = prefix
					+ string.Format("{0:0.00}", layer.depth[x, y])
					+ " ("
					+ string.Format("{0:0.00}", totalDepth)
					+ ")"
					;
			}
			else
			{
				levelText.text = prefix + "N/A";
			}
		}
	}
}
