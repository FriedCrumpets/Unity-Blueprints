using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Blueprints.Scroller
{
    public class TESTER : MonoBehaviour
    {
        [SerializeField] private GameObject template;
        private ScrollDataSource<TestComponent,TestData> _data;
        private InfiniteScroll _infiniteScroll;
        private ScrollRect _scrollRect;

        private void Awake()
        {
            _scrollRect = GetComponent<ScrollRect>();
            _infiniteScroll = GetComponent<InfiniteScroll>();
            _data = new ScrollDataSource<TestComponent, TestData>((component, data) => component.DisplyedText = data.Text);
        }

        private void Start()
        {
            _infiniteScroll.Init<TestComponent>(_scrollRect, template, _data);
        }

        [ContextMenu("Add")]
        private void AddData()
        {
            var random = new System.Random();
            _data.AddData(new TestData(new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", Random.Range(1, 10)).Select(s => s[random.Next(s.Length)]).ToArray())));
        }

        [ContextMenu("Remove")]
        private void RemoveData()
        {
            _data.RemoveDataAtIndex(0);
        }

        [ContextMenu("Clear")]
        private void ClearData()
        {
            _data.ClearData();
        }
        
        [ContextMenu("Update")]
        private void UpdateData()
        {
            var random = new System.Random();
            _data.UpdateDataAtIndex(Random.Range(0, _data.MaxIndex), new TestData(new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", Random.Range(1, 10)).Select(s => s[random.Next(s.Length)]).ToArray())));
        }

        [ContextMenu("Refresh")]
        private void Refresh()
        {
            ClearData();
            for(var i= 0; i < 20; i++)
                AddData();
        }
    }
}