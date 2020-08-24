using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;
using static Unity.Mathematics.math;

public class ABDraw : MonoBehaviour
{
	struct LineDrawData {
		public float3 from;
		public float3 to;
		public Color color;
		public float lifetime;
		public bool depthTest;
	}


	static LineDrawData[] lines = new LineDrawData[200];
	public static int numLines;
	public static int[] freeLineIndices = new int[200];
	public static int numFreeLines;

	public static void Line(float3 from, float3 to, Color color, float lifetime = 0, bool depthTest = false) {
		if (numFreeLines > 0) {
			ref var line = ref lines[freeLineIndices[numFreeLines-1]];
			line.from = from;
			line.to = to;
			line.color = color;
			line.lifetime = lifetime;
			line.depthTest = depthTest;
			numFreeLines -= 1;
		} else {
			ref var line = ref lines[numLines];
			numLines += 1;
			line.from = from;
			line.to = to;
			line.color = color;
			line.lifetime = lifetime;
			line.depthTest = depthTest;
		}
	}

	public static void TickLines(float deltaTime) {
		for (int idx = 0; idx < numLines; ++idx) {
			ref var line = ref lines[idx];
			if (line.lifetime >= 0) {
				line.lifetime -= deltaTime;
				if (line.lifetime < 0) {
					freeLineIndices[numFreeLines] = idx;
					numFreeLines += 1;
				}
			}
		}
	}

	public static void BatchDraw() {
		for (int idx = 0; idx < numLines; ++idx) {
			ref var line = ref lines[idx];
			if (line.lifetime < 0) continue;
			Debug.DrawLine(line.from, line.to, line.color, 0, line.depthTest);
		}
	}
}
