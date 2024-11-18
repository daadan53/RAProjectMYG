using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;

public class ObjectManager : MonoBehaviour
{
    [SerializeField] private RAScene raScene;

    /*private void Start()
    {
        // Récupère la référence au script RAScene
        raScene = FindObjectOfType<RAScene>();
    }*/

    private void OnMouseDown()
    {
        // Appelé lorsqu'on clique sur l'objet
        Debug.Log($"Prefab {gameObject.name} cliqué !");
        raScene.ShowPanel();
    }
}
