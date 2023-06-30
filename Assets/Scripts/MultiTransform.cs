using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

using UnityEngine;

namespace Core
{
    public sealed class MultiTransform : MonoBehaviour
    {
        private List<Part> parts;

        private void LateUpdate()
        {
            UpdateTransform();
        }

        public Part AddPart()
        {
            parts ??= new List<Part>();

            var part = new Part(this);

            parts.Add(part);
            return part;
        }

        public void RemovePart(Part part)
        {
            parts?.Remove(part);
        }

        private void UpdateTransform()
        {
            if (parts == null || parts.Count == 0)
                return;

            var first = parts[0];

            Vector3 position = first.LocalPosition;
            Quaternion rotation = first.LocalRotation;
            Vector3 scale = first.LocalScale;

            for (int i = 1; i < parts.Count; i++)
            {
                var part = parts[i];
                position += part.LocalPosition;
                rotation *= part.LocalRotation; 
                var partScale = part.LocalScale;
                scale.x *= partScale.x;
                scale.y *= partScale.y;
                scale.z *= partScale.z;
            }

            transform.localPosition = position;
            transform.localRotation = rotation;
            transform.localScale = scale;
        }

        public sealed class Part
        {
            public Vector3 LocalPosition { get; set; }
            public Quaternion LocalRotation { get; set; }
            public Vector3 LocalScale { get; set; }
            private MultiTransform transform;

            public Part(MultiTransform transform)
            {
                LocalScale = Vector3.one;
                this.transform = transform;
            }

            public void Remove()
            {
                transform.RemovePart(this);
            }
        }
    }

    public static class MultiTransformExtensions
    {
        public static MultiTransform.Part AsMultiTransform(this Transform transform)
        {
            if (!transform.TryGetComponent<MultiTransform>(out var multiTransform))
            {
                multiTransform = transform.gameObject.AddComponent<MultiTransform>();
            }
            return multiTransform.AddPart();
        }
    }
}