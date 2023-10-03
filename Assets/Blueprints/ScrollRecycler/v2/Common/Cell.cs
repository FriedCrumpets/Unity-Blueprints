using UnityEngine;

namespace Blueprints.ScrollRecycler.v2.Common
{
    // the powerhouse of the cell
    public class Cell
    {
        public Component Component { get; }
        public RectTransform Rect { get; }
        public int CurrentDataIndex { get; set; }
        
        public Cell(Component component)
        {
            Component = component;
            Rect = component.transform as RectTransform;
        }
    }
}