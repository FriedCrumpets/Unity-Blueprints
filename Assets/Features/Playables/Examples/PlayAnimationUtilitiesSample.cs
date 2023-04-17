using UnityEngine;
using UnityEngine.Playables;

namespace Features.Playables
{

    [RequireComponent(typeof(Animator))]
    public class PlayAnimationUtilitiesSample : MonoBehaviour
    {
        public AnimationClip clip;

        PlayableGraph playableGraph;

        void Start()
            => AnimationPlayableUtilities.PlayClip(GetComponent<Animator>(), clip, out playableGraph);
        
        // Destroys all Playables and Outputs created by the graph.
        void OnDisable()
            => playableGraph.Destroy();
    }
}