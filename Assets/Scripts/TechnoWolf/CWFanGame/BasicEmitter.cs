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
		/**<summary>The layer to emit into.</summary>*/
		public MapLayer emitIntoLayer;

		/**<summary>The time at which the last emission occurred.</summary>*/
		private float lastEmitTime = float.PositiveInfinity;

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
				emitIntoLayer.deltaDepth[TileX, TileY] +=
					CalculateEmissionAmount(Time.time - lastEmitTime);
				lastEmitTime = Time.time;
			}
		}

		/**<summary>Check if this emitter can/will emit at the given time.
		 * The result is always false if this script has not been fully
		 * initialized.</summary>
		 * <param name="atTime">The Unity Time.time to check/test</param>
		 */
		public bool CanEmit(float atTime)
		{
			return atTime - lastEmitTime >= interval;
		}

		/**<summary>Calculate the actual amount to be emitted for a given amount
		 * of time, ignoring interval time and delays.</summary>
		 * <param name="actualTimePassed">The amount of time passed</param>
		 */
		public float CalculateEmissionAmount(float actualTimePassed)
		{
			return actualTimePassed * amount;
		}
	}
}
