using System;
using UnityEngine;

namespace Features
{
    public class Testing : MonoBehaviour
    {
        private Solution _solution;

        private void Awake()
        {
            _solution = new Solution();
        }

        private void Start()
        {
            _solution.LengthOfLongestSubstring("dvdf");
            _solution.LengthOfLongestSubstring("abcabcbb");
        }
    }
}