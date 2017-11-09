using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using TechnoWolf.CWFanGame;

public class MapLayerTests
{
	/**<summary>Get a new non-mock MapLayer for testing, and attach it
	 * to the specified GameObject (creates a new GameObject if one is not
	 * specified.)</summary>
	 * <param name="attachTo">Optional:GameObject to attach the component
	 * to</param>
	 */
	public MapLayer GetTestMapLayer(GameObject attachTo = null)
	{
		if (attachTo == null)
		{
			attachTo = new GameObject();
		}
		return attachTo.AddComponent<MapLayer>();
	}

	[Test]
	public void UninitializedWidth()
	{
		MapLayer mapLayer = GetTestMapLayer();

		Assert.AreEqual(-1, mapLayer.Width);
	}

	[Test]
	public void UninitializedHeight()
	{
		MapLayer mapLayer = GetTestMapLayer();

		Assert.AreEqual(-1, mapLayer.Height);
	}

	[Test]
	public void ResizeAndClear_Width()
	{
		int width = 12;
		int height = 15;
		MapLayer mapLayer = GetTestMapLayer();

		mapLayer.ResizeAndClear(width, height);

		Assert.AreEqual(width, mapLayer.Width);
	}

	[Test]
	public void ResizeAndClear_Height()
	{
		int width = 12;
		int height = 15;
		MapLayer mapLayer = GetTestMapLayer();

		mapLayer.ResizeAndClear(width, height);

		Assert.AreEqual(height, mapLayer.Height);
	}

	[Test]
	public void ResizeAndClear_Depth()
	{
		int width = 12;
		int height = 15;
		float depth = 4.0f;
		MapLayer mapLayer = GetTestMapLayer();

		mapLayer.ResizeAndClear(width, height, depth);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Assert.AreEqual(depth, mapLayer.depth[x, y], "First Fail At (" + x + "," + y + ")");
			}
		}
	}

	[Test]
	public void ResizeAndClear_DeltaDepth()
	{
		int width = 12;
		int height = 15;
		float depth = 4.0f;
		MapLayer mapLayer = GetTestMapLayer();

		mapLayer.ResizeAndClear(width, height, depth);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Assert.AreEqual(0.0f, mapLayer.deltaDepth[x, y], "First Fail At (" + x + "," + y + ")");
			}
		}
	}

	[Test]
	public void Clear_Width()
	{
		int width = 12;
		int height = 15;
		MapLayer mapLayer = GetTestMapLayer();
		mapLayer.ResizeAndClear(width, height);

		mapLayer.Clear();

		Assert.AreEqual(width, mapLayer.Width);
	}

	[Test]
	public void Clear_Height()
	{
		int width = 12;
		int height = 15;
		MapLayer mapLayer = GetTestMapLayer();
		mapLayer.ResizeAndClear(width, height);

		mapLayer.Clear();

		Assert.AreEqual(height, mapLayer.Height);
	}

	[Test]
	public void Clear_Depth()
	{
		int width = 12;
		int height = 15;
		float depth = 4.0f;
		MapLayer mapLayer = GetTestMapLayer();
		mapLayer.ResizeAndClear(width, height);

		mapLayer.Clear(depth);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Assert.AreEqual(depth, mapLayer.depth[x, y], "First Fail At (" + x + "," + y + ")");
			}
		}
	}

	[Test]
	public void Clear_DeltaDepth()
	{
		int width = 12;
		int height = 15;
		float depth = 4.0f;
		MapLayer mapLayer = GetTestMapLayer();
		mapLayer.ResizeAndClear(width, height);

		mapLayer.Clear(depth);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Assert.AreEqual(0.0f, mapLayer.deltaDepth[x, y], "First Fail At (" + x + "," + y + ")");
			}
		}
	}

	[Test]
	public void ApplyDeltaDepths_Width()
	{
		int width = 12;
		int height = 15;
		MapLayer mapLayer = GetTestMapLayer();
		mapLayer.ResizeAndClear(width, height);
		mapLayer.deltaDepth[width - 1, height - 1] = 1.0f;

		mapLayer.ApplyDeltaDepths();

		Assert.AreEqual(width, mapLayer.Width);
	}

	[Test]
	public void ApplyDeltaDepths_Height()
	{
		int width = 12;
		int height = 15;
		MapLayer mapLayer = GetTestMapLayer();
		mapLayer.ResizeAndClear(width, height);
		mapLayer.deltaDepth[width - 1, height - 1] = 1.0f;

		mapLayer.ApplyDeltaDepths();

		Assert.AreEqual(height, mapLayer.Height);
	}

	[Test]
	public void ApplyDeltaDepths_Depth()
	{
		int width = 12;
		int height = 15;
		MapLayer mapLayer = GetTestMapLayer();
		mapLayer.ResizeAndClear(width, height);

		float[,] initialDepth = new float[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				initialDepth[x, y] = (float)(x + y);
				mapLayer.depth[x, y] = (float)(x + y);
			}
		}
		float[,] deltaDepth = new float[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				deltaDepth[x, y] = (float)(x);
				mapLayer.deltaDepth[x, y] = (float)(x);
			}
		}
		float[,] expectedDepth = new float[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				expectedDepth[x, y] = initialDepth[x, y] + deltaDepth[x, y];
			}
		}
		mapLayer.ApplyDeltaDepths();

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Assert.AreEqual(expectedDepth[x, y], mapLayer.depth[x, y], "First Fail At (" + x + "," + y + ")");
			}
		}
	}

	[Test]
	public void ApplyDeltaDepths_DeltaDepth()
	{
		int width = 12;
		int height = 15;
		MapLayer mapLayer = GetTestMapLayer();
		mapLayer.ResizeAndClear(width, height);

		float[,] initialDepth = new float[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				initialDepth[x, y] = (float)(x + y);
				mapLayer.depth[x, y] = (float)(x + y);
			}
		}
		float[,] deltaDepth = new float[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				deltaDepth[x, y] = (float)(x);
				mapLayer.deltaDepth[x, y] = (float)(x);
			}
		}
		float[,] expectedDepth = new float[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				expectedDepth[x, y] = initialDepth[x, y] + deltaDepth[x, y];
			}
		}
		mapLayer.ApplyDeltaDepths();

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Assert.AreEqual(0.0f, mapLayer.deltaDepth[x, y], "First Fail At (" + x + "," + y + ")");
			}
		}
	}

	[Test]
	public void CalculateTotalDepths_OneLayer()
	{
		int width = 14;
		int height = 12;
		MapLayer layer1 = GetTestMapLayer();
		float[,] expected = new float[width, height];
		float[,] actual = null;
		layer1.ResizeAndClear(width, height);
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				layer1.depth[x, y] = x + y + 1;
				expected[x, y] = layer1.depth[x, y];
			}
		}

		actual = MapLayer.CalculateTotalDepths(layer1);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Assert.AreEqual(expected[x, y], actual[x, y], "First Fail At (" + x + "," + y + ")");
			}
		}
	}

	[Test]
	public void CalculateTotalDepths_TwoLayers()
	{
		int width = 14;
		int height = 12;
		MapLayer layer1 = GetTestMapLayer();
		MapLayer layer2 = GetTestMapLayer();
		float[,] expected = new float[width, height];
		float[,] actual = null;
		layer1.ResizeAndClear(width, height);
		layer2.ResizeAndClear(width, height);
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				layer1.depth[x, y] = x + y + 1;
				layer2.depth[x, y] = (x + y + 1) * 2.1f;
				expected[x, y] = layer1.depth[x, y] + layer2.depth[x, y];
			}
		}

		actual = MapLayer.CalculateTotalDepths(layer1, layer2);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Assert.AreEqual(expected[x, y], actual[x, y], "First Fail At (" + x + "," + y + ")");
			}
		}
	}

	[Test]
	public void CalculateTotalDepths_ThreeLayers()
	{
		int width = 14;
		int height = 12;
		MapLayer layer1 = GetTestMapLayer();
		MapLayer layer2 = GetTestMapLayer();
		MapLayer layer3 = GetTestMapLayer();
		float[,] expected = new float[width, height];
		float[,] actual = null;
		layer1.ResizeAndClear(width, height);
		layer2.ResizeAndClear(width, height);
		layer3.ResizeAndClear(width, height);
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				layer1.depth[x, y] = x + y + 1;
				layer2.depth[x, y] = (x + y + 1) * 2.1f;
				layer3.depth[x, y] = (x + y + 2) * 4.0f;
				expected[x, y] = layer1.depth[x, y] + layer2.depth[x, y] + layer3.depth[x, y];
			}
		}

		actual = MapLayer.CalculateTotalDepths(layer1, layer2, layer3);

		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				Assert.AreEqual(expected[x, y], actual[x, y], "First Fail At (" + x + "," + y + ")");
			}
		}
	}

	// TODO test fluid motion simulation
}
