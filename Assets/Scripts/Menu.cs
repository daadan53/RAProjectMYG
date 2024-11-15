using UnityEngine;

public class Menu : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] private GameObject threeDView;
    [SerializeField] private Camera mainCamera;
    public void Play() => GameManager.instance.OnGoNextScene("RAScene");
    public void GoBack() => GameManager.instance.OnGoBack("CatalogueScene");

    private void Awake()
    {
        gameManager = GameManager.instance;

        gameManager.threeDView = threeDView;
        gameManager.mainCamera = mainCamera;
    }
}
