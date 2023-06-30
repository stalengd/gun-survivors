using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Effects
{
    public class Dissolve : MonoBehaviour
    {
        [SerializeField] private new Renderer renderer;
        [SerializeField] private Material material;
        [SerializeField] private float duration = 0.5f;

        private int parameterId;
        private float timer = 0f;
        private MaterialPropertyBlock propertyBlock;


        private void Awake()
        {
            parameterId = Shader.PropertyToID("_Dissolve");
        }

        private void OnEnable()
        {
            renderer.sharedMaterial = material;
            propertyBlock = new();
            renderer.GetPropertyBlock(propertyBlock);
        }

        private void Update()
        {
            if (timer < duration)
            {
                timer += Time.deltaTime;
                propertyBlock.SetFloat(parameterId, timer / duration);
                renderer.SetPropertyBlock(propertyBlock);
            }
        }
    }
}