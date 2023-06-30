using UnityEngine;
using Core.Animations;

namespace Core
{
    [RequireComponent(typeof(Animator))]
    public sealed class CreatureAnimator : MonoBehaviour
    {
        public float MovingSpeed 
        { 
            set
            {
                BaseAnimator.SetBool("IsMoving", value != 0f);
                BaseAnimator.SetFloat("MovementSpeed", value);
            }
        }
        public Animator BaseAnimator => baseAnimator;

        private Animator baseAnimator;


        private void Awake()
        {
            baseAnimator = GetComponent<Animator>();
        }


        public void PlayHit()
        {
            baseAnimator.SetTrigger("Hit");
        }
        
        public void PlayDeath()
        {
            baseAnimator.SetBool("Dead", true);
        }
    }
}
