using UnityEditor;

namespace Poly.Name.Editor
{
    [CustomEditor(typeof(OPolyRegistryAsset), true)]
    public sealed class FPolyNameRegistryAssetInspector : UnityEditor.Editor
    {
	    public override void OnInspectorGUI()
	    {
		    EditorGUILayout.HelpBox(
			    "AUTO-GENERATED. Do not edit.\nEdit Name fields on components; the registry is maintained by code.",
			    MessageType.Warning);

		    using (new EditorGUI.DisabledScope(true))
		    {
			    DrawDefaultInspector();
		    }
	    }
    }
}
