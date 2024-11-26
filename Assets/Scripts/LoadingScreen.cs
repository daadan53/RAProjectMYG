using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen; // UI pour l'écran de chargement
    [SerializeField] private UnityEngine.UI.Slider progressBar; // Barre de progression

    private void Start()
    {
        StartCoroutine(LoadMainSceneAsync());
    }

    private IEnumerator LoadMainSceneAsync()
    {

        if (loadingScreen != null)
        {
            loadingScreen.SetActive(true);
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("CatalogueScene");

        // Empêche l'activation automatique de la scène une fois chargée
        asyncLoad.allowSceneActivation = false;

        // Attendre le chargement complet
        while (!asyncLoad.isDone)
        {
            // Mettre à jour la barre de progression
            if (progressBar != null)
            {
                progressBar.value = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            }

            // Vérifier si la scène est chargée à 90% (Unity considère cela comme prêt)
            if (asyncLoad.progress >= 0.9f)
            {
                // Optionnel : Ajouter un délai pour simuler un écran de chargement
                yield return new WaitForSeconds(1f);

                // Activer la scène
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }
}
