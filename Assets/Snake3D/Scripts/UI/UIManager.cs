using UnityEngine;
using UnityEngine.SceneManagement;

namespace Snake3D.UI
{
    public class UIManager : MonoBehaviour
    {
        public void LoadScene(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}