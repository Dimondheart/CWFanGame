using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.CWFanGame
{
	/**<summary>Everything specific to the current map.</summary>*/
	public class Map : MonoBehaviour
	{
		/**<summary>The layer group for the surface.</summary>*/
		public MapLayerGroup surfaceLayerGroup;

		public void Awake()
		{
			surfaceLayerGroup.Setup(21, 21, 1);
		}
	}
}
