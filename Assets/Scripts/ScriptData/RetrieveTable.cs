using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class JSonHelper
{
    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }

    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }
}

//Crée une classe qui va accueillir nos données
[System.Serializable]
public class Product
{
    public int id;
    public string name;
    public string description;
    public string dimension;
    public int price;
}

public class RetrieveTable : MonoBehaviour
{
    //identifiant uniforme de ressource
    private string _uri = "http://localhost/ra_app/getTable.php";
    public static event System.Action<Product[]> OnProductsRetrieved;

    public Product[] products {get; private set;}

    void Start()
    {
        StartCoroutine(RetrieveDataFromInternet());
    }

    IEnumerator RetrieveDataFromInternet()
    {
        //La requête
        using(UnityWebRequest webRequest = UnityWebRequest.Get(_uri))
        {
            //On envoie la requête
            yield return webRequest.SendWebRequest();

            //On traite la réponse 
            switch(webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.LogError("Connection Error");
                    break;
                
                //Problème à la reception des données
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError($"Something went wrong {webRequest.error}");
                    break;

                case UnityWebRequest.Result.Success:
                    products = JSonHelper.FromJson<Product>(webRequest.downloadHandler.text); //On récup les données
                    OnProductsRetrieved?.Invoke(products);
                    /*foreach(Product product in products)
                    {
                        Debug.Log($"{product.id}, {product.name}, {product.description}, {product.dimension}, {product.price}");
                    }*/
                    break;
            }
        }
    }
}
