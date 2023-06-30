using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Stalen.DI;

namespace Core
{
    public sealed class Crosshair : MonoService
    {
        [SerializeField] private bool autoAim = false;

        [Header("Line")]
        [SerializeField] private Transform line;
        [SerializeField] private SpriteRenderer lineRenderer;
        [SerializeField] private float lineStartOffset = 1f;
        [SerializeField] private float lineEndOffset = 1f;
        [SerializeField] private Color lineActiveColor;
        [SerializeField] private Color lineDisableColor;

        public Vector3 GunPosition { get; set; }

        private Vector3 position;

        private new CameraController camera;


        private void OnEnable()
        {
            Cursor.visible = false;
        }

        private void Update()
        {
            RefreshPosition();
        }

        private void OnDisable()
        {
            Cursor.visible = true;
        }

        private void OnApplicationFocus(bool focus)
        {
            if (enabled)
            {
                Cursor.visible = false;
            }
        }


        public override void ConfigureDependencies()
        {
            Inject.Out(out camera);
        }

        public void SetLineActive(bool active)
        {
            lineRenderer.color = active ? lineActiveColor : lineDisableColor;
        }


        private void RefreshPosition()
        {
            if (autoAim)
            {
                // todo
            }
            else
            {
                position = GetWorldPointerPosition();
            }
            transform.position = position;

            var lineDir = position - GunPosition;
            line.position = GunPosition + lineDir.normalized * lineStartOffset;
            line.right = lineDir;
            var lineLen = lineDir.magnitude;
            lineLen -= lineEndOffset;
            lineLen -= lineStartOffset;
            var lineScale = line.localScale;
            lineScale.x = lineLen > 0f ? lineLen : 0f;
            line.localScale = lineScale;
        }

        private Vector3 GetWorldPointerPosition()
        {
            var screenPos = Input.mousePosition;
            var worldPos = camera.Camera.ScreenToWorldPoint(screenPos);
            worldPos.z = 0f;
            return worldPos;
        }
    }
}