using System.Collections;
using UnityEngine;
namespace Base
{
    public class LoadScene : MonoBehaviour
    {
        private const string MAIN_SCENE = "SampleScene";

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(2); 
            Load(MAIN_SCENE);
        }

        private void Load(string nameScene)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(nameScene);
        }
    }
}
