using System.Collections.Generic;
using UnityEngine;

namespace Core.Effects
{
    [ExecuteInEditMode]
    public class SpriteOutline : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private List<SpriteRenderer> spriteRenderers;
        [SerializeField] private Material defaultMaterial;
        [SerializeField] private Material outlineMaterial;
        [SerializeField] private bool usePerRendererOutline;

        [Header("Per Renderer Outline Options")]
        public Color color = Color.white;
        [Range(0, 16)]
        public int outlineSize = 1;


        private void OnEnable()
        {
            UpdateOutline(true);
        }

        private void OnDisable()
        {
            UpdateOutline(false);
        }


        private void UpdateOutline(bool enabled)
        {
            UpdateOutlineForRenderer(spriteRenderer, enabled);
            foreach (var renderer in spriteRenderers)
            {
                UpdateOutlineForRenderer(renderer, enabled);
            }
        }

        private void UpdateOutlineForRenderer(SpriteRenderer renderer, bool enabled)
        {
            if (renderer == null)
                return;

            renderer.sharedMaterial = enabled ? outlineMaterial : defaultMaterial;

            if (usePerRendererOutline)
            {
                MaterialPropertyBlock mpb = new MaterialPropertyBlock();
                renderer.GetPropertyBlock(mpb);
                mpb.SetFloat("_Outline", enabled ? 1f : 0);
                mpb.SetColor("_OutlineColor", color);
                mpb.SetFloat("_OutlineSize", outlineSize);
                renderer.SetPropertyBlock(mpb);
            }
        }
    }
}