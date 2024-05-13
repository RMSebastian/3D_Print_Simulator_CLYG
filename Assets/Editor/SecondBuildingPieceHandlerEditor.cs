using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(SecondBuildingPieceHandler))]
public class SecondBuildingPiecehandlerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        SecondBuildingPieceHandler pieceHandler = (SecondBuildingPieceHandler)target;
       
        EditorGUILayout.Space(5);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create EmbedPosition"))pieceHandler.CreateTransform(pieceHandler.embedPiece.transform, ref pieceHandler.embedPosition, ref pieceHandler.embedRotation);
        if (GUILayout.Button("Set EmbedPosition")) pieceHandler.SetTransform(pieceHandler.embedPiece.transform, pieceHandler.embedPosition,pieceHandler.embedRotation);
        EditorGUILayout.EndHorizontal();
        
        if (GUILayout.Button("Reset EmbedPosition")) pieceHandler.ResetTransform(ref pieceHandler.embedPosition, ref pieceHandler.embedRotation);
       
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create LayPosition")) pieceHandler.CreateTransform(pieceHandler.layPiece.transform, ref pieceHandler.layPosition, ref pieceHandler.layRotation);
        if (GUILayout.Button("Set LayPosition")) pieceHandler.SetTransform(pieceHandler.layPiece.transform, pieceHandler.layPosition,pieceHandler.layRotation);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Reset LayPosition")) pieceHandler.ResetTransform(ref pieceHandler.layPosition, ref pieceHandler.layRotation);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Create cameraPosition")) pieceHandler.CreateTransform(pieceHandler.layPiece.transform, ref pieceHandler.CameraPosition, ref pieceHandler.CameraRotation);
        if (GUILayout.Button("Set cameraPosition")) pieceHandler.SetTransform(pieceHandler.layPiece.transform, pieceHandler.CameraPosition, pieceHandler.CameraRotation);
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Reset cameraPosition")) pieceHandler.ResetTransform(ref pieceHandler.CameraPosition, ref pieceHandler.CameraRotation);
    }
}
