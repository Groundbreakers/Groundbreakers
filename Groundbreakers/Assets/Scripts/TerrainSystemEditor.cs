using UnityEditor;

namespace Assets.Scripts
{
    [CustomEditor(typeof(TerrainSystem))]
    public class TerrainSystemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspector();

            EditorGUILayout.HelpBox("This is a help box", MessageType.Info);
        }
    }
}