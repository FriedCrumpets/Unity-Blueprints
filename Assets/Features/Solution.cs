using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Features
{
    public class Solution
    {
        // two Numbers add up to Equal the target from the provided Array
        public int[] TwoSum(int[] nums, int target)
        {
            // create dict map
            var dict = new Dictionary<int, int>();
            
            // loop through array
            for (var i = 0; i < nums.Length; i++)
            {
                // If the dictionary contains the key for the target minus the current number then those two 
                // numbers sum up to equal the target
                if (dict.TryGetValue(target - nums[i], out var value))
                    return new[] { value, i };
                
                // otherwise we pop this number in the dictionary to be tested with future numbers
                dict.TryAdd(nums[i], i);
            }

            // If there isn't a result; default is returned
            return default;
        }
        
        public int LengthOfLongestSubstring(string s)
        {
            var sub = new HashSet<char>();
            var max = 0;

            for (var i = 0; i < s.Length; i++)
            {
                var c = s[i];
                if (!sub.Contains(c))
                {
                    sub.Add(c);
                    max = sub.Count < max ? max : sub.Count;
                    continue;
                }
                
                i -= sub.Count;
                sub.Clear();
            }        

            return max;
        }
        
        /// <summary>
        /// Returns true if the provided string has a working bracket structure
        /// <example>
        /// "[]" == true
        /// "{}" == true
        /// "()" == true
        /// "(]" == false
        /// "(" == false
        /// "int[] ppooo() { }" = true
        /// </example>
        /// The same bracket that opens must also close.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public bool IsValid(string s) 
        {
            var stack = new Stack<char>();

            foreach (var c in s)
            {
                if(c.Equals('('))
                    stack.Push(')');
                if(c.Equals('['))
                    stack.Push(']');
                if(c.Equals('{'))
                    stack.Push('}');
                if (c.Equals(stack.Peek()))
                    stack.Pop();
            }

            return stack.Count == 0;
        }
    }
}