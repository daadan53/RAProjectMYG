using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

public class TestPlayMod : InputTestFixture
{
    Mouse mouse;

    private List<VisualElement> visualElements;
    private GameObject appManager;

    public override void Setup()
    {
        base.Setup();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Scenes/CatalogueScene", LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // S'assurer que la scène correcte est chargée
        if (scene.name == "CatalogueScene")
        {
            mouse = InputSystem.AddDevice<Mouse>();
            appManager = GameManager.instance.gameObject;

            if (appManager == null)
            {
                Assert.Fail("Aucun GameObject avec le tag 'GameManager' n'a été trouvé.");
                return;
            }

            TakeVisualElement();

            SceneManager.sceneLoaded -= OnSceneLoaded; // Désabonner une fois la scène chargée
        }
    }

    public void ClickUI(GameObject _uiObject, VisualElement _visualElement)
    {
        Camera camera;
        UnityEngine.Vector3 screenPos;

        if (_uiObject != null)
        {
            camera = Camera.main.GetComponent<Camera>(); // On récupère la camera 
            screenPos = camera.WorldToScreenPoint(_uiObject.transform.position); // On recup la position de l'élément vis à vis de la caméra
            Set(mouse.position, screenPos); //On positionne la souris sur l'élément choisis
        }
        else if (_visualElement != null)
        {
            camera = Camera.main.GetComponent<Camera>();
            UnityEngine.Vector2 screenPosBis = camera.WorldToScreenPoint(_visualElement.worldBound.center);
            Set(mouse.position, screenPosBis);
        }
        else
        {
            Assert.Fail("Aucun objet ou element visuel assigné");
            return;
        }

        Click(mouse.leftButton); // On clique avec notre souris precedemment créer
        //Debug.Log(mouse.position);
    }

    private void ClickVisualElement(VisualElement element)
    {
        // Vérifiez que l'élément est interactif
        if (!element.enabledInHierarchy || !element.resolvedStyle.visibility.Equals(Visibility.Visible))
        {
            Assert.Fail("L'élément n'est pas interactif ou visible.");
            return;
        }

        Assert.IsTrue(element.visible, "L'élément UI n'est pas visible.");
        Assert.IsTrue(element.enabledInHierarchy, "L'élément UI n'est pas actif.");

        UnityEngine.Vector2 globalPosition = element.worldBound.center; // Position globale approximative
        UnityEngine.Vector2 localMousePosition = globalPosition - element.worldBound.min; // Position locale relative à l'élément

        // Simuler un événement de clic
        var clickEvent = MouseDownEvent.GetPooled(globalPosition, 0, 1, localMousePosition, EventModifiers.None);
        element.SendEvent(clickEvent);
        Debug.Log(globalPosition);
        Debug.Log(localMousePosition);

        var upEvent = MouseUpEvent.GetPooled(globalPosition, 0, 1, localMousePosition, EventModifiers.None);
        element.SendEvent(upEvent);
    }

    private void SimulateClickWithRaycast(VisualElement _element)
    {
        UnityEngine.Vector2 screenPosition = _element.worldBound.center;

        if (mouse == null)
        {
            mouse = InputSystem.AddDevice<Mouse>();
        }
        Set(mouse.position, screenPosition);

        // Pour stocker les résultats du raycast
        var raycastResults = new List<RaycastResult>();
        var pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        // Effectue le raycast
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        // Vérifie si l'élément cible a été touché
        var targetResult = raycastResults.FirstOrDefault(result => result.gameObject != null && result.gameObject.name.ToLower() == _element.name.ToLower());
        Assert.IsNotNull(targetResult, $"Aucun élément UI interactif trouvé sous la souris pour l'élément {_element.name}.");

        // Simule le clic
        pointerEventData.pointerPress = targetResult.gameObject;
        ExecuteEvents.Execute(pointerEventData.pointerPress, pointerEventData, ExecuteEvents.pointerClickHandler);
    }

    private void TakeVisualElement()
    {
        if(appManager == null)
        {
            Assert.Fail("Aucun game object appelé AppManager n'a été trouvé");
            return;
        }
        var root = appManager.GetComponent<UIDocument>().rootVisualElement;

        // Récupère tous les VisualElements sous la forme d'une liste
        visualElements = new List<VisualElement>(root.Query<VisualElement>().ToList());
    }

    private void ClickOnElement(string _nameOfElement)
    {
        /*var element = visualElements.FirstOrDefault(el => el.name.ToLower() == _nameOfElement.ToLower());
        if (element == null)
        {
            Assert.Fail($"Aucun élément avec le nom {_nameOfElement} trouvé.");
            return;
        }*/

        // Assurez-vous que l'élément est interactif
        //Assert.IsTrue(element.enabledInHierarchy && element.resolvedStyle.visibility == Visibility.Visible, "L'élément n'est pas interactif.");
        if(visualElements == null)
        {
            Assert.Fail("Pas de visual element dans la liste");
            return;
        }
        for(int i = 0; i < visualElements.Count; i++)
        {
            if(visualElements[i].name == _nameOfElement)
            {
                SimulateClickWithRaycast(visualElements[i]);
            }
        }
        /*if(element is Button)
        {
            ClickVisualElementBis(element);
            //Debug.Log(element.lo)
        }*/
        
    }

    private void ClickVisualElementBis(VisualElement element)
    {
        // Vérifiez que l'élément est interactif
        if (!element.enabledInHierarchy || !element.resolvedStyle.visibility.Equals(Visibility.Visible))
        {
            Assert.Fail("L'élément n'est pas interactif ou visible.");
            return;
        }

        // Créez un événement de clic manuellement
        var clickEvent = ClickEvent.GetPooled();
        element.SendEvent(clickEvent);
        Debug.Log("JE CLIQUE !!");
    }

    [UnityTest]
    public IEnumerator TestPlayModWithEnumeratorPasses()
    {
        Assert.IsNotNull(visualElements, "Aucun visual element");

        yield return new WaitForSeconds(2f);
        //ClickVisualElement(button);
        //ClickOnMiddleOfScreen(button);
        ClickOnElement("Product1");

        yield return new WaitForSeconds(1f);
        //Assert.IsFalse(appManager.GetComponent<UIDocument>().enabled, "L'ui document n'a pas été desactivé donc rien n'a été cliqué");
    }
}
