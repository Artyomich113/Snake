using System.IO;
using Artyomich;
using UnityEditor;
using UnityEngine;
public class LevelEditor : EditorWindow
{
    int x;
    int y;

    float Scale = 1;

    Material mainTextureMaterial;
    Rect mainTextureRect;
    Texture2D mainTexture;

    Texture2D BufferedTexture;
    Color color = Color.white;
    string folderPath = "Assets/";
    string fileName;

    [MenuItem ("Snake/Level")]
    private static void ShowWindow ()
    {
        var window = GetWindow<LevelEditor> ();
        window.titleContent = new GUIContent ("Level");
        window.Show ();
    }

    [System.Obsolete]
    private void OnGUI ()
    {
        if ((Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag) && Event.current.button == 0)
        {
            //Debug.Log (GUIUtility.GUIToScreenPoint (Event.current.mousePosition) - position.min);
            Vector2 mousePos = GUIUtility.GUIToScreenPoint (Event.current.mousePosition) - position.min;
            //Debug.Log ("input " + mousePos);
            if (mousePos.IsInside (mainTextureRect))
            {
                Vector2 inRectPos = mousePos - mainTextureRect.position;
                // Debug.Log ("in rect " + inRectPos);
                int x = (int) (inRectPos.x / Scale);
                int y = (int) (inRectPos.y / Scale);
                // Debug.Log ("Texture cords " + x + " " + y + " " + color);
                // Debug.Log ("color " + mainTexture.GetPixel (x, y, 0));
                mainTexture.SetPixel (x, y, color, 0);
                // Debug.Log ("color " + mainTexture.GetPixel (x, y, 0));
            }
        }

        //mainTextureMaterial = (Material) EditorGUILayout.ObjectField ("Material", mainTextureMaterial, typeof (Material));

        if (BufferedTexture = (Texture2D) EditorGUILayout.ObjectField ("Select Texture:", BufferedTexture, typeof (Texture2D)))
        {
            //Debug.Log ("selected");
        }

        folderPath = EditorGUILayout.TextField ("FolderPath", folderPath);
        fileName = EditorGUILayout.TextField ("FileName", fileName);
        x = EditorGUILayout.IntField ("Xcord", x);
        y = EditorGUILayout.IntField ("Ycord", y);

        Scale = EditorGUILayout.Slider ("Scale", Scale, 0.1f, 10f);

        color = EditorGUILayout.ColorField ("Color", color);

        GUILayout.BeginHorizontal ();
        if (GUILayout.Button ("Save"))
        {
            Savetexture ();
        }
        if (GUILayout.Button ("CreateTexture"))
        {
            CreateTexture ();
        }
        if (GUILayout.Button ("SetBuf"))
        {
            mainTexture = BufferedTexture;
        }
        GUILayout.EndHorizontal ();

        if (mainTexture)
        {
            mainTextureRect = new Rect (20, 220, mainTexture.width * Scale, mainTexture.height * Scale);
            mainTexture.filterMode = FilterMode.Point;
            EditorGUI.DrawPreviewTexture (mainTextureRect, mainTexture);
        }

    }

    private void OnMouseDown ()
    {
        Debug.Log ("Input");
    }

    public void CreateTexture ()
    {
        mainTexture = new Texture2D (x, y);
        mainTexture.filterMode = FilterMode.Point;
    }

    public void Savetexture ()
    {
        string path = folderPath + fileName;

        Debug.Log ("Saved To " + path);
        Debug.Log ("Color " + color);

        //UnityEditor.AssetDatabase.CreateAsset (mainTexture, path);

        byte[] bytes = mainTexture.EncodeToPNG ();
        FileStream stream = new FileStream (path, FileMode.OpenOrCreate, FileAccess.Write);
        BinaryWriter writer = new BinaryWriter (stream);
        for (int i = 0; i < bytes.Length; i++)
        {
            writer.Write (bytes[i]);
        }
        writer.Close ();
        stream.Close ();

        AssetDatabase.ImportAsset (path, ImportAssetOptions.ForceUpdate);
    }

    public static void LoadTexture ()
    {

    }
}