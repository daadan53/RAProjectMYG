using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Accessibility;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance {private set;get;}

    private List<VisualElement> visualElements;
    VisualElement productCharged;
    [SerializeField] private GameObject[] prefabModel;
    private GameObject visualPrefab;
    public GameObject threeDView;
    private bool isVisualInstantiated = false;
    public int visualPrefabIndex = -1;

    public GameObject groundStage;

    public Camera mainCamera;

    private Touch firstTouch;
    private float startXPosition;
    private float startYPosition;
    [SerializeField] private float speedMove = 0.1f;

    [SerializeField] private float distanceField = 1.5f;
    private const string wineBottle = "WineBottle";

    private string sceneName;

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
        // Ajouter un listener pour le clic sur chaque visual element qui s'appel product
        ChargeProductCatalogue();
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

    private void ChargeProductCatalogue()
    {
        // Récupérer le VisualElement
        var root = GetComponent<UIDocument>().rootVisualElement;
        int j = 0;
        

        // Récupère tous les VisualElements sous la forme d'une liste
        visualElements = new List<VisualElement>(root.Query<VisualElement>().ToList());

        for (int i = 0; i < visualElements.Count; i++)
        {
            if (visualElements[i].name.ToLower() == "product" + j)
            {
                productCharged = visualElements[i];

                // Vérifie si j est dans les limites de prefabModel
                int index = j; // Sauvegarde l'index actuel pour l'utiliser dans le callback
                if (index < prefabModel.Length)
                {
                    productCharged.RegisterCallback<ClickEvent>(ev => 
                    { 
                        if(threeDView.transform.childCount >= 1)
                        {
                            Destroy(threeDView.transform.GetChild(0).gameObject);
                        }
                        //Instancie le prefab au au niveau du canva
                        visualPrefab = Instantiate(prefabModel[index]);
                        visualPrefab.transform.SetParent(threeDView.transform);
                        visualPrefab.transform.localPosition = Vector3.zero;

                        threeDView.transform.position = mainCamera.transform.position + mainCamera.transform.forward * 5f;
                        threeDView.transform.LookAt(mainCamera.transform);

                        AdjustDistance(visualPrefab);

                        this.gameObject.GetComponent<UIDocument>().enabled = false;
                        isVisualInstantiated = true;
                    });

                    j++;
                }
                else
                {
                    Debug.LogWarning("Index " + j + " est en dehors de la liste de prefab.");
                }
            }
        }
    }

    private void AdjustDistance(GameObject visual)
    {
        Renderer renderer = visual.GetComponent<Renderer>();
        if (renderer == null)
        {
            renderer = visual.GetComponentInChildren<Renderer>();
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
        if(!threeDView.transform.GetChild(0).name.Contains(wineBottle))
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
        else if (threeDView.transform.GetChild(0).name.Contains(wineBottle))
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

                    visualPrefab.transform.Rotate(0, 0, -diffX * speedMove, Space.Self);

                    // Met à jour les positions de départ pour la prochaine frame
                    startXPosition = firstTouch.position.x;
                    startYPosition = firstTouch.position.y;
                    break;
            }
        }
    }

    public void OnGoBack(string _sceneName)
    {
        isVisualInstantiated = false;

        if(sceneName == "RAScene")
        {
            StartCoroutine(Loading(_sceneName));
        }

        this.gameObject.GetComponent<UIDocument>().enabled = true;

        ChargeProductCatalogue();
    }

    //Méthodes de chargement de scène
    public void OnGoNextScene(string sceneName)
    {
        if (visualPrefab != null)
        {
            SaveVisual();
        }

        isVisualInstantiated = false;

        StartCoroutine(Loading(sceneName));
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
        // Met à jour le nom de la scène actuelle lorsqu'une nouvelle scène est chargée
        sceneName = scene.name;

        /*if (visualPrefabIndex >= 0 && groundStage != null)
        {
            InstantiateSavedVisual();
        }*/
    }

    private void SaveVisual()
    {
        for (int i = 0; i < prefabModel.Length; i++)
        {
            if (visualPrefab.name.Contains(prefabModel[i].name))
            {
                visualPrefabIndex = i;
                break;
            }
        }
    }

    /*private void InstantiateSavedVisual()
    {
        if (visualPrefabIndex >= 0 && visualPrefabIndex < prefabModel.Length)
        {
            visualPrefab = Instantiate(prefabModel[visualPrefabIndex]);
            visualPrefab.transform.SetParent(groundStage.transform);
            visualPrefab.transform.localPosition = Vector3.zero;
            visualPrefab.SetActive(false);
        }
        else
        {
            Debug.Log("Surement le groundstage qui manque");
        }
    }*/

    private void OnDestroy()
    {
        // Retire le listener quand le script est détruit
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /*public void OnTargetFound()
    { 
        groundStage.transform.GetChild(0).gameObject.SetActive(true);
        //canvasDebug.SetActive(true);
    }

    public void OnTargetLost()
    {
        if(groundStage.transform.childCount == 1)
        {
            groundStage.transform.GetChild(0).gameObject.SetActive(false);
        }
    }  */
}
