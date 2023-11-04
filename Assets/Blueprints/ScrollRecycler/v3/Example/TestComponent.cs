using TMPro;
using UnityEngine;

namespace Blueprints.Scroller
{
    public class TestComponent : MonoBehaviour, IScrollCell
    {
        private TMP_Text text;
        
        public string DisplyedText { set => text.text = value; }

        public void Init(params object[] args)
        {
            text = GetComponentInChildren<TMP_Text>();
        }

        public void DeInit()
        {
            
        }
    }

    public class TestData
    {
        public string Text { get; }
        public TestData(string text)
        {
            Text = text;
        }
    }
}