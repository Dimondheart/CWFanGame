using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using TechnoWolf.CWFanGame;

public class MapLayerTests
{
	/**<summary>Get a new non-mock MapLayer for testing.</summary>*/
	private MapLayer GetTestMapLayer()
	{
		return new GameObject().AddComponent<MapLayer>();
	}

	[Test]
	public void MapLayer_UninitializedWidth()
	{
		MapLayer mapLayer = GetTestMapLayer();

		Assert.AreEqual(-1, mapLayer.Width);
	}

	[Test]
	public void MapLayer_UninitializedHeight()
	{
		MapLayer mapLayer = GetTestMapLayer();

		Assert.AreEqual(-1, mapLayer.Height);
	}

	[Test]
	public void MapLayer_ResizeAndClear_Width()
	{
		int width = 12;
		int height = 15;
		MapLayer mapLayer = GetTestMapLayer();

		mapLayer.ResizeAndClear(width, height);

		Assert.AreEqual(width, mapLayer.Width);
	}

	[Test]
	public void MapLayer_ResizeAndClear_Height()
	{
		int width = 12;
		int height = 15;
		MapLayer mapLayer = GetTestMapLayer();

		mapLayer.ResizeAndClear(width, height);

		Assert.AreEqual(height, mapLayer.Height);
	}

	[Test]
	public void MapLayer_ResizeAndClear_Depth()
	{
		int width = 12;
		int height = 15;
		float depth = 4.0f;
		MapLayer mapLayer = GetTestMapLayer();

		mapLayer.ResizeAndClear(width, height, depth);

		foreach (float d in mapLayer.depth)
		{
			Assert.AreEqual(depth, d);
		}
	}

	[Test]
	public void MapLayer_ResizeAndClear_DeltaDepth()
	{
		int width = 12;
		int height = 15;
		float depth = 4.0f;
		MapLayer mapLayer = GetTestMapLayer();

		mapLayer.ResizeAndClear(width, height, depth);

		foreach (float d in mapLayer.deltaDepth)
		{
			Assert.AreEqual(0.0f, d);
		}
	}

	[Test]
	public void MapLayer_Clear_Width()
	{
		int width = 12;
		int height = 15;
		MapLayer mapLayer = GetTestMapLayer();
		mapLayer.ResizeAndClear(width, height);

		mapLayer.Clear();

		Assert.AreEqual(width, mapLayer.Width);
	}

	[Test]
	public void MapLayer_Clear_Height()
	{
		int width = 12;
		int height = 15;
		MapLayer mapLayer = GetTestMapLayer();
		mapLayer.ResizeAndClear(width, height);

		mapLayer.Clear();

		Assert.AreEqual(height, mapLayer.Height);
	}

	[Test]
	public void MapLayer_Clear_Depth()
	{
		int width = 12;
		int height = 15;
		float depth = 4.0f;
		MapLayer mapLayer = GetTestMapLayer();
		mapLayer.ResizeAndClear(width, height);

		mapLayer.Clear(depth);

		foreach (float d in mapLayer.depth)
		{
			Assert.AreEqual(depth, d);
		}
	}

	[Test]
	public void MapLayer_Clear_DeltaDepth()
	{
		int width = 12;
		int height = 15;
		float depth = 4.0f;
		MapLayer mapLayer = GetTestMapLayer();
		mapLayer.ResizeAndClear(width, height);

		mapLayer.Clear(depth);

		foreach (float d in mapLayer.deltaDepth)
		{
			Assert.AreEqual(0.0f, d);
		}
	}

	[Test]
	public void MapLayer_ApplyDeltaDepths_Width()
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
	public void MapLayer_ApplyDeltaDepths_Height()
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
	public void MapLayer_ApplyDeltaDepths_Depth()
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
				Assert.AreEqual(expectedDepth[x, y], mapLayer.depth[x, y]);
			}
		}
	}

	[Test]
	public void MapLayer_ApplyDeltaDepths_DeltaDepth()
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
				Assert.AreEqual(0.0f, mapLayer.deltaDepth[x, y]);
			}
		}
	}
}
