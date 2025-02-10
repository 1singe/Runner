using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace ProcGen
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(Camera))]
    public class EditorCamera : MonoBehaviour
    {
        [SerializeField] float _idleRotationSpeed = 30f;
        [SerializeField] private EditorGenerationManager _generationManager;
        [SerializeField] private Camera _gameCamera;
        [SerializeField] private Camera _renderCamera;
        [SerializeField] private float _ortographicScaling = 1f;

        private async void OnEnable()
        {
            _generationManager = FindFirstObjectByType<EditorGenerationManager>();
            _gameCamera = GetComponent<Camera>();
            _renderCamera = transform.GetChild(0).GetComponent<Camera>();

            Assert.IsNotNull(_generationManager);
            float halfChunkSize = (_generationManager.ChunkSize - 1f) / 2f;

            while (true)
            {
                Vector3 center = new Vector3(halfChunkSize, 0f, halfChunkSize);
                float orthographicSize = _generationManager.ChunkSize * _ortographicScaling * (_generationManager.GenerationHalfSize * 2 + 1f);
                Application.targetFrameRate = -1;
                transform.RotateAround(center, Vector3.up, _idleRotationSpeed * (1 / 60f));
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation((center - transform.position).normalized, Vector3.up), 0.1f);
                _renderCamera.orthographicSize = Mathf.Lerp(_renderCamera.orthographicSize, orthographicSize, 0.1f);
                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                EditorApplication.update.Invoke();
                await Task.Delay(16);
            }
        }
    }
}