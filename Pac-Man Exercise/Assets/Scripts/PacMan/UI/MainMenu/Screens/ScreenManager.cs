using PacMan.Utility.DependencyInjection;
using UnityEngine;

namespace PacMan.UI
{
    /*
     * A screen manager class to manage the main screens of the menu
     */
    public class ScreenManager : MonoBehaviour
    {
        private Screen _currentScreen;
        
        private void Awake()
        {
            DependencyInjection.InjectAsType(this);
        }

        private void OnDestroy()
        {
            DependencyInjection.DeleteDependency<ScreenManager>();
        }

        public void OpenScreen(Screen screen)
        {
            if (_currentScreen != null)
            {
                _currentScreen.Close();    
            } 
            
            _currentScreen = screen;
            _currentScreen.Open();
        }
    }
}