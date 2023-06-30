using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBox;

namespace Effects 
{ 
    public class BouncyShell : MonoBehaviour
    {
        [SerializeField] private new SpriteRenderer renderer;

        [Range(0, 1)]
        [SerializeField] private float bounciness = 0.5f;
        [SerializeField] private float gravity = 10f;
        [SerializeField] private float zPosition = 5f;
        [SerializeField] private MinMaxFloat rotationSpeedRange;
        [SerializeField] private MinMaxFloat startVerticalSpeed;
        [SerializeField] private MinMaxFloat startHorizontalSpeed;

        private Vector3 velocity; // Z is for imaginary height
        private float rotationVelocity;


        private void Awake()
        {
            rotationVelocity = rotationSpeedRange.RandomInRange();

            var horizontalVelocity = Vector2.right.Rotate(Random.Range(0f, 360f)) * startHorizontalSpeed.RandomInRange();

            velocity = new Vector3(
                horizontalVelocity.x,
                horizontalVelocity.y,
                startVerticalSpeed.RandomInRange());
        }

        private void Start()
        {

        }

        private void FixedUpdate()
        {
            FreeFly();
        }


        public void SetHorizontalVelocity(Vector2 horizontalVelocity)
        {
            velocity = new Vector3(
                horizontalVelocity.x,
                horizontalVelocity.y,
                velocity.z);
        }


        private void FreeFly()
        {
            if (velocity.sqrMagnitude <= 0.1f)
            {
                return;
            }

            var newPos = Vector3.MoveTowards(
                transform.position,
                transform.position + velocity,
                velocity.magnitude * Time.fixedDeltaTime);

            var delta = newPos - transform.position;

            var oldZPos = zPosition;
            zPosition += delta.z;

            newPos.y = newPos.y - oldZPos + zPosition;
            newPos.z = 0f;
            transform.position = newPos;


            transform.Rotate(0f, 0f, rotationVelocity * Time.fixedDeltaTime);

            velocity.z -= gravity * Time.fixedDeltaTime;

            if (GetMinPoint() <= 0f && velocity.z < 0f)
            {
                velocity.z *= -1f;
                velocity *= bounciness;

                if (velocity.z < gravity * Time.fixedDeltaTime * 2f)
                {
                    Landing();
                }
            }
        }

        private void Landing()
        {
            velocity = Vector3.zero;
            //shadowRenderer.enabled = false;

            enabled = false;
        }


        private float GetMinPoint()
        {
            var bounds = renderer.bounds;
            var min = bounds.min.y;
            min += zPosition - transform.position.y;
            return min;
        }

        private void OnMagnitized()
        {
            enabled = true;
            //shadowRenderer.enabled = true;
        }
    }
}