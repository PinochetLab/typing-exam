using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.UI
{
    public class WindowSwitcher : MonoBehaviour
    {
        [SerializeField] private List<Window> windows;
        
        private Dictionary<WindowName, Window> _windows;

        private void Awake()
        {
            _windows = windows.ToDictionary(x => x.Name, x => x);
        }

        public void SwitchWindow(WindowName windowName)
        {
            windows.ForEach(x => x.SetActive(false));
            _windows[windowName].SetActive(true);
        }
        
        public void SwitchWindow(string windowName)
        {
            if (windowName == "MainMenu")
            {
                SwitchWindow(WindowName.MainMenu);
            }
            else
            {
                SwitchWindow(WindowName.Game);
            }
        }
    }
}