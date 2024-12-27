using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

public class TestPlayMod : InputTestFixture
{
    Mouse mouse;

    private List<VisualElement> visualElements;
    private GameObject appManager;
    private GameManager gameManager;
    private GameObject buttonBack;

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
            gameManager = appManager.GetComponent<GameManager>();

            if (appManager == null)
            {
                Assert.Fail("Aucun GameObject avec le tag 'GameManager' n'a été trouvé.");
                return;
            }

            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public void ClickUI(GameObject _element)
    {
        Camera camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        Vector3 screenPos = camera.WorldToScreenPoint(_element.transform.position);
        Set(mouse.position, screenPos);
        Click(mouse.leftButton);
    }

    [UnityTest]
    public IEnumerator TestPlayModWithEnumeratorPasses()
    {
        Assert.IsNotNull(gameManager, "Game manager est null");
        yield return new WaitForSeconds(2f);

        gameManager.OnProductClicked(1);

        yield return new WaitForSeconds(1f);

        Assert.IsFalse(gameManager.GetComponent<UIDocument>().enabled, "L'UI document n'a pas été désactivé.");
        Assert.IsTrue(gameManager.threeDView.transform.childCount > 0, "Le prefab n'a pas été placé dans la vue 3D");
        gameManager.OnGoNextScene("RAScene", gameManager.visualPrefab, gameManager.prefabModelList);

        yield return new WaitForSeconds(2f);
        Assert.IsTrue(gameManager.sceneName == "RAScene", "La scène n'a pas changer");
        Assert.IsTrue(gameManager.groundStage.transform.childCount > 0, "Le prefab n'a pas été placé dans le ground stage");
        //On vérifie la connexion à la bdd


        buttonBack = GameObject.Find("ButtonBack");
        ClickUI(buttonBack);
        
        yield return new WaitForSeconds(2f);
        Assert.IsTrue(gameManager.sceneName == "CatalogueScene", "La scène n'a pas changer");
        Assert.IsTrue(gameManager.GetComponent<UIDocument>().enabled, "Le catalogue n'a pas été réactivé");

    }
}
