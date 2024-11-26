using UnityEngine;

public class Menu : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] private GameObject threeDView;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject panelProduct;
    public GameObject panelDebugBis;

    private void Awake()
    {
        gameManager = GameManager.instance;

        gameManager.threeDView = threeDView;
        gameManager.mainCamera = mainCamera;
        gameManager.productPanel = panelProduct;
        if(gameManager.panelDebug == null)
        {
            gameManager.panelDebug = panelDebugBis;
        }
    }

    public void Play() => gameManager.OnGoNextScene("RAScene", gameManager.visualPrefab, gameManager.prefabModelList);
    public void GoBack() => gameManager.Return();
    public void ChargeButton(GameObject _clickedButton) 
    {
        gameManager.ChargeProduct(_clickedButton);
    }
    public void QuitApp()
    {
        Application.Quit();
    }
}
