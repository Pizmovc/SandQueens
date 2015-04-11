using UnityEngine;
using UnityEditor;
using System.Collections;

namespace ResourceManager {
	public static class RM {
		public static class Camera{
			public static float scrollSpeed { get { return 100; }}
			public static float moveSpeed { get { return 60; }}
			public static int scrollWidth { get { return 30; } }
			public static float minCameraHeight { get { return RM.Terrarium.height; } }
			public static float maxCameraHeight { get { return 4*RM.Terrarium.height; } }
		}

		public static class Terrarium{
			public static float width { get { return 10; }}
			public static float length { get { return width; }}
			public static float height { get { return 20; }}
			public static float sandBaseHeight { get { return 10; }}
		}

	}
}