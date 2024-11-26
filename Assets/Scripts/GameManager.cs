using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {get; private set;}

    public List<GameObject> prefabModelList;
    public GameObject visualPrefab;
    public GameObject threeDView;
    private bool isVisualInstantiated = false;
    public int visualPrefabIndex = -1;
    public GameObject productPanel;

    public GameObject groundStage;

    public Camera mainCamera;

    private Touch firstTouch;
    private float startXPosition;
    private float startYPosition;
    [SerializeField] private float speedMove = 0.1f;

    [SerializeField] private float distanceField = 1.5f;
    private const string WINE_BOTTLE = "Product1";
    private const string CATALOGUE_SCENE = "CatalogueScene";

    public string sceneName;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private UnityEngine.UI.Slider progressBar;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);

            // Ajoute le listener pour détecter le chargement de nouvelles scènes
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(this.gameObject); // Détruit les instances supplémentaires
        }

        loadingScreen.SetActive(false);
    }

    void Start()
    {
        ChargePrefabToList(prefabModelList);
    }

    void Update()
    {
        if(sceneName == "RAScene")
        {

            if(Input.touchCount == 1 && groundStage.transform.childCount == 1 && !groundStage.transform.GetChild(0).gameObject.activeSelf)
            {
                groundStage.transform.GetChild(0).gameObject.SetActive(true);
            }

        }

        if(isVisualInstantiated && Input.touchCount == 1)
        {
            RotateView();
        }
    }

    public void ChargePrefabToList(List<GameObject> _prefabList)
    {
        prefabModelList = new List<GameObject>();
        //On récup les produits qui sont enfant du gameObject et on les mets dans une liste
        int i = 0; // Démarre avec "product0"

        while (true)
        {
            string expectedName = $"product{i}";
            GameObject foundProduct = null;

            foreach (Transform child in this.transform)
            {
                if (child.name.ToLower() == expectedName)
                {
                    foundProduct = child.gameObject;
                    break;
                }
            }

            if (foundProduct == null)
            {
                break;
            }

            prefabModelList.Add(foundProduct);
            foundProduct.SetActive(false);

            i++; // Passe au produit suivant
        }
    }

    public void ChargeProduct(GameObject _clickedButton)
    {
        string buttonName = _clickedButton.name;

        foreach (var prefab in prefabModelList)
        {
            if (prefab.name == buttonName)
            {
                visualPrefab = prefab;
                break;
            }
        }

        if (visualPrefab == null)
        {
            Debug.LogError($"Prefab avec le nom '{buttonName}' introuvable dans prefabModelList.");
            return;
        }

        visualPrefabIndex = SaveVisual(visualPrefab, prefabModelList);
        MovePrefabTo(visualPrefab, threeDView.transform, true);

        // Positionne et ajuste la distance
        threeDView.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 5f;
        threeDView.transform.LookAt(mainCamera.transform);
        AdjustDistance(visualPrefab);

        productPanel.SetActive(false);
        threeDView.transform.parent.gameObject.SetActive(true);
        isVisualInstantiated = true;
    }

    private void AdjustDistance(GameObject _visual)
    {
        Renderer renderer = _visual.GetComponentInChildren<Renderer>();
        if (renderer == null)
        {
            Transform childOfChild = _visual.transform.GetChild(0).GetChild(0);
            renderer = childOfChild.GetComponent<Renderer>();
        }
        
        // Calcule le plus grand côté de l'objet
        float maxSize = Mathf.Max(renderer.bounds.size.x, renderer.bounds.size.y, renderer.bounds.size.z);

        // Calcule la distance en fonction de la taille et du champ de vision de la caméra
        float distance = (maxSize / Mathf.Tan(Mathf.Deg2Rad * mainCamera.fieldOfView / 2)) * distanceField; // Le facteur 1.1f ajuste l'espacement

        // Place l'objet à la distance calculée face à la caméra
        threeDView.transform.position = mainCamera.transform.position + mainCamera.transform.forward * distance;
    }

    private void RotateView()
    {
        if (threeDView.transform.GetChild(0).name.Contains(WINE_BOTTLE))
        {
            firstTouch = Input.GetTouch(0);
            switch (firstTouch.phase)
            {
                case TouchPhase.Began:
                    startXPosition = firstTouch.position.x;
                    startYPosition = firstTouch.position.y;
                    break;

                case TouchPhase.Moved :
                    // Calcul la diff entre position de départ et actuelle
                    float diffX = firstTouch.position.x - startXPosition;

                    visualPrefab.transform.Rotate(0, -diffX * speedMove, 0, Space.Self);

                    // Met à jour les positions de départ pour la prochaine frame
                    startXPosition = firstTouch.position.x;
                    startYPosition = firstTouch.position.y;
                    break;
            }
        }
        else
        {
            firstTouch = Input.GetTouch(0);

            switch (firstTouch.phase)
            {
                case TouchPhase.Began:
                    startXPosition = firstTouch.position.x;
                    startYPosition = firstTouch.position.y;
                    break;

                case TouchPhase.Moved :
                    // Calcul la diff entre position de départ et actuelle
                    float diffX = firstTouch.position.x - startXPosition;
                    float diffY = firstTouch.position.y - startYPosition;

                    visualPrefab.transform.Rotate(diffY * speedMove, -diffX * speedMove, 0, Space.World);

                    // Met à jour les positions de départ pour la prochaine frame
                    startXPosition = firstTouch.position.x;
                    startYPosition = firstTouch.position.y;
                    break;
            }
        }
    }

    public void Return()
    {
        visualPrefab.transform.rotation = Quaternion.identity;
        isVisualInstantiated = false;
        MovePrefabTo(visualPrefab, this.gameObject.transform, false);
        threeDView.transform.parent.gameObject.SetActive(false);
        productPanel.SetActive(true);
    }

    public void MovePrefabTo(GameObject _prefab, Transform _newParent, bool _isVisible)
    {
        _prefab.transform.SetParent(_newParent);
        _prefab.transform.localPosition = Vector3.zero;
        _prefab.SetActive(_isVisible);
    }

    //Méthodes de chargement de scène
    public void OnGoNextScene(string _sceneName, GameObject _visualPrefab, List<GameObject> _prefabsList)
    {
        if (_visualPrefab != null)
        {
            visualPrefabIndex = SaveVisual(_visualPrefab, _prefabsList);
            MovePrefabTo(visualPrefab, this.gameObject.transform, false);
        }

        isVisualInstantiated = false;

        StartCoroutine(Loading(_sceneName));
    }
    private IEnumerator Loading(string _sceneName) 
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(_sceneName);

        //Tant que le chargement n'est pas finit on attend
        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); //Calcule pour mettre juste 0 à 1
            progressBar.value = progress;
            yield return null;
        }

        loadingScreen.SetActive(false);
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneName = scene.name;

        if(sceneName == CATALOGUE_SCENE)
        {
            productPanel.SetActive(true);
            threeDView.transform.parent.gameObject.SetActive(false);
        }
    }

    private int SaveVisual(GameObject _visualPrefab, List<GameObject> _prefabModelList)
    {
        int visualIndex = -1;

        for (int i = 0; i < _prefabModelList.Count; i++)
        {
            if (_visualPrefab.name.Contains(_prefabModelList[i].name))
            {
                visualIndex = i;
                return visualIndex;
            }
        }

        return visualIndex;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
