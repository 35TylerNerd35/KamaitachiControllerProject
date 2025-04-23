using UnityEngine ;
using UnityEditor ;

[ CustomEditor ( typeof ( MonoBehaviour ) , true ) ]
public class DefaultInspector : Editor
{
    public override void OnInspectorGUI ( )
    {
        this . DrawDefaultInspectorWithoutScriptField ( ) ;
    }
}

public static class DefaultInspector_EditorExtension
{
    public static bool DrawDefaultInspectorWithoutScriptField ( this Editor Inspector )
    {
        EditorGUI . BeginChangeCheck ( ) ;
       
        Inspector . serializedObject . Update ( ) ;
       
        SerializedProperty Iterator = Inspector . serializedObject . GetIterator ( ) ;
       
        Iterator . NextVisible ( true ) ;
       
        while ( Iterator . NextVisible ( false ) )
        {
            EditorGUILayout . PropertyField ( Iterator , true ) ;
        }
       
        Inspector . serializedObject . ApplyModifiedProperties ( ) ;
       
        return ( EditorGUI . EndChangeCheck ( ) ) ;
    }
}
