using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

    public class IPChange : EditorWindow
    {
        public  static string address;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


     //   [MenuItem("RosBridgeClient/IP Addres")]
        private static void Init()
        {
            IPChange editorWindow = GetWindow<IPChange>();
            editorWindow.minSize = new Vector2(500, 300);


            editorWindow.Show();

        }

        private void OnGUI()
        {
            GUILayout.Label("Change IP Addres", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            address = EditorGUILayout.TextField("Address", address);
            EditorGUILayout.EndHorizontal();
        }

        private void GetEditorPrefs()
        {


            address = (EditorPrefs.HasKey("Address") ?
                EditorPrefs.GetString("Address") :
                "ws://192.168.0.1:9090");
     
      


        }

        private void OnFocus()
        {
            GetEditorPrefs();
        }

        private void OnLostFocus()
        {
            SetEditorPrefs();
        }

        private void OnDestroy()
        {
            SetEditorPrefs();
        }

        private void SetEditorPrefs()
        {
            EditorPrefs.SetString("Address", address);
        }

    }

