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
		/**<summary>The layer group for the subterrain.</summary>*/
		public MapLayerGroup subterrainLayerGroup;

		public void Awake()
		{
			surfaceLayerGroup.Setup(21, 21, 1);
			subterrainLayerGroup.Setup(21, 21, 0);
			SetLayerGroupVisibility(true);
		}

		public void Update()
		{
			if (Input.GetKeyDown(KeyCode.LeftControl))
			{
				SetLayerGroupVisibility(!surfaceLayerGroup.GetComponentInChildren<SpriteRenderer>().enabled);
			}
		}

		private void SetLayerGroupVisibility(bool setSurfaceVisible)
		{
			SpriteRenderer[] surfSprites = surfaceLayerGroup.GetComponentsInChildren<SpriteRenderer>();
			SpriteRenderer[] subtSprites = subterrainLayerGroup.GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer sr in surfSprites)
			{
				sr.enabled = setSurfaceVisible;
			}
			foreach (SpriteRenderer str in subtSprites)
			{
				str.enabled = !setSurfaceVisible;
			}
		}
	}
}
