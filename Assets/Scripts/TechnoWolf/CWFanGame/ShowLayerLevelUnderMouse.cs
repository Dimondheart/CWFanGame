using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TechnoWolf.CWFanGame
{
	/**<summary></summary>*/
	public class ShowLayerLevelUnderMouse : MonoBehaviour
	{
		public Text levelText;
		public MapLayer layer;
		public MapLayer[] lowerLayers;

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
