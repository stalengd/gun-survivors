using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Effects
{
    public class MaterialBlink : MonoBehaviour
    {
        [SerializeField] private Utils.Timer blinkTimer = new(0.1f);

        [Space]
        [SerializeField] private Material blinkMaterial;

        private new Renderer renderer;

        private bool isBlinking = false;
        private Material defaultMaterial;


        private void Awake()
        {
            renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            if (!isBlinking) return;

            if (blinkTimer.Update())
            {
                isBlinking = false;
                renderer.material = defaultMaterial;
            }
        }

        public void Blink()
        {
            blinkTimer.Restart();
            isBlinking = true;

            if (defaultMaterial == null)
                defaultMaterial = renderer.material;

            renderer.material = blinkMaterial;
        }
    }
}