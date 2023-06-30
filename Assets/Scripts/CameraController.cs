using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Stalen.DI;

namespace Core
{
    public sealed class CameraController : MonoService
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private float smoothing = 5f;
        [SerializeField] private bool limitView;
        [SerializeField] private Rect boundaries;

        public Camera Camera => camera;

        private List<List<Transform>> targetsStack = new();

        private Vector2 targetPosition;



        private void Start()
        {
            targetPosition = transform.position;
            targetPosition = CalculateTargetPosition();
            var pos = (Vector3)targetPosition;
            pos.z = transform.position.z;
            transform.position = pos;
        }

        private void FixedUpdate()
        {
            var newTargetPosition = CalculateTargetPosition();
            targetPosition = Vector2.Lerp(targetPosition, (Vector2)newTargetPosition, Time.fixedDeltaTime * smoothing);

            Vector3 pos = targetPosition;
            var cameraSize = GetViewSize();

            if (limitView)
            {
                var finalBoundaries = new Rect(boundaries.position + cameraSize, boundaries.size - cameraSize * 2f);
                if (pos.x < finalBoundaries.xMin)
                    pos.x = finalBoundaries.xMin;
                if (pos.x > finalBoundaries.xMax)
                    pos.x = finalBoundaries.xMax;
                if (pos.y < finalBoundaries.yMin)
                    pos.y = finalBoundaries.yMin;
                if (pos.y > finalBoundaries.yMax)
                    pos.y = finalBoundaries.yMax;
            }

            pos.z = transform.position.z;
            transform.position = pos;
        }

        private void OnDrawGizmos()
        {
            if (limitView)
            {
                Gizmos.DrawWireCube(boundaries.center, boundaries.size);
            }
        }


        public override void ConfigureDependencies()
        {
            Track(Inject.Here<Player>().transform);
        }

        public Vector3 CalculateTargetPosition()
        {
            var targets = targetsStack.LastOrDefault();
            if (targets == null) return targetPosition;

            var bounds = new Bounds(targets.First().position, Vector3.zero);
            for (int i = 1; i < targets.Count; i++)
            {
                bounds.Encapsulate(targets[i].position);
            }
            return bounds.center;
        }

        public void Track(List<Transform> transforms)
        {
            if (transforms == null || !transforms.Any()) return;

            targetsStack.Add(transforms);
        }

        public void Track(Transform transform)
        {
            Track(new List<Transform>() { transform });
        }

        public void Untrack(List<Transform> transforms)
        {
            targetsStack.Remove(transforms);
        }

        public void Untrack(Transform transform)
        {
            for (int i = targetsStack.Count - 1; i >= 0; i--)
            {
                if (targetsStack[i].Contains(transform))
                {
                    targetsStack.RemoveAt(i);
                    return;
                }
            }
        }

        public Vector2 GetViewSize()
        {
            return new Vector2(camera.orthographicSize * (Screen.width / (float)Screen.height), camera.orthographicSize);
        }
    }
}