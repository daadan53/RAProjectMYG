using UnityEngine;

public class Menu : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] private GameObject threeDView;
    [SerializeField] private Camera mainCamera;

    private void Awake()
    {
        gameManager = GameManager.instance;

        gameManager.threeDView = threeDView;
        gameManager.mainCamera = mainCamera;
    }

    public void Play() => gameManager.OnGoNextScene("RAScene", gameManager.visualPrefab, gameManager.prefabModelList);
    public void GoBack() => gameManager.Return();
}
