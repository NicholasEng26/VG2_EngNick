using UnityEngine;
using UnityEngine.SceneManagement;

namespace Q2
{
    public class KillZone : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<CharacterController>())
            {
                string currentScene = SceneManager.GetActiveScene().name;
                SceneManager.LoadScene(currentScene);
            }
        }
    }
}