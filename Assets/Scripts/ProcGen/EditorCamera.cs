using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace ProcGen
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class EditorCamera : MonoBehaviour
    {
        [SerializeField] float _idleRotationSpeed = 30f;


        private async void OnEnable()
        {
            while (true)
            {
                Application.targetFrameRate = -1;
                transform.RotateAround(new Vector3(120f, 0f, 120f), Vector3.up, _idleRotationSpeed * (1 / 60f));
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                EditorApplication.update.Invoke();
                await Task.Delay(16);
            }
        }
    }
}