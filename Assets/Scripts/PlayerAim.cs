using UnityEngine;
using Stalen.DI;

namespace Core
{
    public sealed class PlayerAim : MonoBehaviour
    {
        [SerializeField] private bool autoAim = false;

        public Vector3 AimTarget { get; private set; }

        private new CameraController camera;


        private void Start()
        {
            ConfigureDependencies();
        }

        private void Update()
        {
            RefreshAimTarget();
        }


        private void ConfigureDependencies()
        {
            Inject.Out(out camera);
        }

        private void RefreshAimTarget()
        {
            if (autoAim)
            {
                // todo
            }
            else
            {
                AimTarget = GetWorldPointerPosition();
            }
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
