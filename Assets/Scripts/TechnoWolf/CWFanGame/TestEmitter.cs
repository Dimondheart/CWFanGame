using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TechnoWolf.CWFanGame
{
	/**<summary></summary>*/
	public class TestEmitter : MonoBehaviour
	{
		public float delay = 0.0f;
		public float interval = 1.0f;
		public float amount = 20.0f;
		public MapLayer emitIntoLayer;

		private float lastEmitTime = 0.0f;

		public void Start()
		{
			lastEmitTime = Time.time - interval + delay - 0.00001f;
		}

		public void Update()
		{
			if (Time.time - lastEmitTime >= interval)
			{
				emitIntoLayer.deltaDepth[
					Mathf.RoundToInt(transform.localPosition.x),
					Mathf.RoundToInt(transform.localPosition.y)
					] += (Time.time - lastEmitTime) * amount;
				lastEmitTime = Time.time;
			}
		}
	}
}
