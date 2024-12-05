using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PostData : MonoBehaviour
{
    private string _uri = "http://localhost/tests/postData.php";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(CreateEmploye("Cassandra", 1400));
    }

    IEnumerator CreateEmploye(string nom, int salaire)
    {
        WWWForm form = new WWWForm();
        form.AddField("nomPost", nom);
        form.AddField("salairePost", salaire);

        //Requête
        using UnityWebRequest www = UnityWebRequest.Post(_uri, form);

        yield return www.SendWebRequest();

        if(www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(www.error);
        }
        else
        {
            Debug.Log("Form upload complete !");
        }
    }
}
