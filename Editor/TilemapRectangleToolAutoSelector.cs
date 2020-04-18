using System;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.Tilemaps;

namespace UniTilemapRectangleToolAutoSelector
{
	internal static class GridPaintPaletteWindowUtils
	{
		private const BindingFlags BINDING_ATTR = BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic;

		private static readonly Assembly   m_assembly         = typeof( AssetDatabase ).Assembly;
		private static readonly Type       m_type             = m_assembly.GetType( "UnityEditor.GridPaintPaletteWindow" );
		private static readonly MethodInfo m_gridRectangleKey = m_type.GetMethod( "GridRectangleKey", BINDING_ATTR );

		public static void GridRectangleKey() => m_gridRectangleKey.Invoke( null, new object[0] );
	}

	[InitializeOnLoad]
	internal static class TilemapRectangleToolAutoSelector
	{
		static TilemapRectangleToolAutoSelector()
		{
			Selection.selectionChanged += SelectionChanged;
		}

		private static void SelectionChanged()
		{
			var gameObject = Selection.activeGameObject;

			if ( gameObject == null ) return;

			var tilemap   = gameObject.GetComponent<Tilemap>();
			var isTilemap = tilemap != null;

			if ( !isTilemap ) return;
			if ( EditMode.editMode == EditMode.SceneViewEditMode.GridBox ) return;

			GridPaintPaletteWindowUtils.GridRectangleKey();
		}
	}
}