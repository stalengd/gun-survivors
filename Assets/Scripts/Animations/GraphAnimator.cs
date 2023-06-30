using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

namespace Core.Animations
{
    public class GraphAnimator : MonoBehaviour
    {
        [SerializeField] private Animator baseAnimator;
        [SerializeField] private AnimationClip defaultClip;

        public System.Func<AnimationRequest> StateGetter { get; set; }
        public float SpeedMultiplier
        {
            get => speedMultiplier;
            set
            {
                speedMultiplier = value;
                if (clipPlayable.IsValid())
                {
                    clipPlayable.SetSpeed(speedMultiplier);
                }
            }
        }
        public float CurrentClipDuration => currentClipDuration;

        private PlayableGraph graph;
        private AnimationPlayableOutput animationOutput;
        private AnimationClipPlayable clipPlayable;
        private AnimationClip currentClip;
        private float speedMultiplier = 1f;
        private float currentClipDuration;


        private void Start()
        {
            graph = PlayableGraph.Create(gameObject.name);
            graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);
            animationOutput = AnimationPlayableOutput.Create(graph, "Animation", baseAnimator);

            SetClip(GetCurrentClip());

            graph.Play();
        }

        private void OnEnable()
        {
            if (graph.IsValid())
            {
                graph.Play();
            }
        }

        private void OnDisable()
        {
            if (graph.IsValid())
            {
                graph.Stop();
            }
        }

        private void Update()
        {
            if (graph.IsDone())
            {
                RequestRefresh(true);
            }
        }

        private void OnDestroy()
        {
            if (graph.IsValid())
            {
                graph.Destroy();
            }
        }


        public void SetClip(AnimationRequest request, bool restartIfSame = false)
        {
            if (request.clip == null) return;
            if (request.clip == currentClip && !restartIfSame) return;

            Debug.Log(request.clip.name);

            speedMultiplier = request.speed;
            currentClip = request.clip;
            currentClipDuration = request.clip.averageDuration;

            if (!clipPlayable.IsNull())
            {
                clipPlayable.Destroy();
            }
            clipPlayable = AnimationClipPlayable.Create(graph, request.clip);
            clipPlayable.SetDuration(currentClipDuration);
            clipPlayable.SetSpeed(SpeedMultiplier);
            animationOutput.SetSourcePlayable(clipPlayable);
        }

        public void RequestRefresh(bool restartIfSame = false)
        {
            SetClip(GetCurrentClip(), restartIfSame);
        }


        private AnimationRequest GetCurrentClip()
        {
            if (StateGetter != null)
            {
                return StateGetter();
            }

            return defaultClip;
        }
    }

    public struct AnimationRequest
    {
        public AnimationClip clip;
        public float speed;

        public AnimationRequest(AnimationClip clip, float speed = 1f)
        {
            this.clip = clip;
            this.speed = speed;
        }

        public static implicit operator AnimationRequest(AnimationClip clip)
        {
            return new AnimationRequest(clip, 1f);
        }
    }
}