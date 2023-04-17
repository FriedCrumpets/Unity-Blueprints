using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UIElements;

namespace Editor.Logging
{
    
    /// <summary>
    /// <example>
    /// private static ViewLink SetupToolbar(VisualElement root)
    /// {
    ///     var toolbar = root.Q("ToolBar");
    ///     var view = root.Q("View");
    ///     var viewToolbar = new ViewLink(toolbar, view);
    ///     viewToolbar.LinkViewControllers(listOfClassNames);
    ///     
    ///     return viewToolbar;
    /// }
    /// </example>
    /// </summary>
    public class Toolbar
    {
        private const string _BUTTON_DISABLE_CLASS = "toolbar-button__disabled";
        
        public Toolbar(VisualElement toolbarRoot, VisualElement viewRoot)
        {
            Buttons = toolbarRoot.Query<Button>(className: "toolbar-button").ToList();
            Views = viewRoot.Query(className: "toolbar-view").ToList();
        }
        
        private List<Button> Buttons { get; }
        private List<VisualElement> Views { get; }
        private List<LinkedView> Link { get; } = new();

        public void LinkViewControllers(List<string> classNames, bool resetLinks = true)
        {
            if (resetLinks) { Unlink(); }
            
            foreach (var className in classNames)
            {
                var button = Buttons.FirstOrDefault(controller => controller.GetClasses().Contains(className));
                var view = Views.FirstOrDefault(controller => controller.GetClasses().Contains(className));
                
                if (view != default && button != default)
                {
                    var linkedView = new LinkedView(button, view, ToggleLinkedView);
                    linkedView.Disable();
                    Link.Add(linkedView);
                }
            }

            EnableFirstView(Link);
        }

        private void Unlink()
        {
            if (!Link.Any())
            {
                return;
            }

            foreach (var linkedView in Link)
            {
                linkedView.Dispose();
            }
            
            Link.Clear();
        }

        private void ToggleLinkedView()
        {
            foreach (var view in Link)
            {
                if (view.Clicked)
                {
                    view.Enable();
                }
                else
                {
                    view.Disable();
                }
            }
        }

        private static void EnableFirstView(List<LinkedView> linkedViews)
        {
            if (!linkedViews.Any()) { return; }

            foreach (var view in linkedViews) { view.Disable(); }
            
            linkedViews[0].Enable();
        }

        private class LinkedView : IDisposable
        {
            public LinkedView(Button button, VisualElement view, Action onButtonClicked = null)
            {
                Button = button;
                View = view;
                _onButtonClicked = onButtonClicked;

                button.clicked += OnButtonClicked;
            }

            private readonly Action _onButtonClicked;

            public Button Button { get; }

            public VisualElement View { get; }
            
            public bool Clicked { get; private set; }

            public DisplayStyle Display
            {
                get => View.style.display.value;
                set => View.style.display = value;
            }

            public void Enable()
            {
                Display = DisplayStyle.Flex;
                Button.AddToClassList(_BUTTON_DISABLE_CLASS);
                Button.SetEnabled(false);
            }

            public void Disable()
            {
                Display = DisplayStyle.None;
                Button.RemoveFromClassList(_BUTTON_DISABLE_CLASS);
                Button.SetEnabled(true);
            }
            
            public void Dispose() => UnLink();

            private void OnButtonClicked()
            {
                Clicked = true;
                _onButtonClicked?.Invoke();
                Clicked = false;
            }

            private void UnLink()
            {
                Button.clicked -= OnButtonClicked;
                Button.RemoveFromClassList(_BUTTON_DISABLE_CLASS);
                Button.SetEnabled(true);
                Display = DisplayStyle.Flex;
            }
        }
    }
}