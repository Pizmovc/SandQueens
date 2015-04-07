using UnityEngine;
using UnityEditor;
using System.Collections;

//Remember, also, that any variables or methods we add to ResourceManager will also need to be declared as static.

namespace ResourceManager {
	public static class RM {
		public static float ScrollSpeed { get { return 80; }}
		public static float MoveSpeed { get { return 60; }}
		public static int ScrollWidth { get { return 30; } }
		public static float MinCameraHeight { get { return 5; } }
		public static float MaxCameraHeight { get { return 50; } }

	}
}