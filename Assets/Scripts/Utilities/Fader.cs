using System.Collections;
using UnityEngine;

namespace Utils
{
    public static class Fader
    {
        public static IEnumerator Fade(float current, float target, float duration)
        {
            var elapsed = 0.0f;
            
            while (!current.Equals(target))
            {
                current = Mathf.Lerp( current, target, elapsed / duration );
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
    }
}