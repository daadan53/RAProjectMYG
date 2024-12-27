using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class TestBDD
{
    private RetrieveTable retrieveTable;

    [SetUp]
    public void Setup()
    {
        // Cr�ation d'un GameObject pour attacher le script RetrieveTable
        GameObject gameObject = new GameObject("RetrieveTableTestObject");
        retrieveTable = gameObject.AddComponent<RetrieveTable>();
    }

    [UnityTest]
    public IEnumerator TestConnectionToDatabase()
    {
        bool isDataRetrieved = false;
        Product[] retrievedProducts = null;

        // S'abonner � l'�v�nement pour d�tecter lorsque les produits sont r�cup�r�s
        RetrieveTable.OnProductsRetrieved += products =>
        {
            isDataRetrieved = true;
            retrievedProducts = products;
        };

        // D�marrer la r�cup�ration des donn�es
        retrieveTable.StartCoroutine(retrieveTable.RetrieveDataFromInternet());

        // Attendre un maximum de 5 secondes pour la r�ponse
        float timeout = 5f;
        float elapsedTime = 0f;

        while (!isDataRetrieved && elapsedTime < timeout)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // V�rifier si les donn�es ont bien �t� r�cup�r�es
        Assert.IsTrue(isDataRetrieved, "La connexion � la base de donn�es a �chou� ou aucune donn�e n'a �t� r�cup�r�e.");

        // V�rifier que les produits ne sont pas null
        Assert.IsNotNull(retrievedProducts, "Aucun produit n'a �t� r�cup�r�.");
        Assert.IsTrue(retrievedProducts.Length > 0, "La liste des produits est vide.");

        foreach (var product in retrievedProducts)
        {
            Assert.IsNotNull(product.name, "Un produit r�cup�r� n'a pas de nom.");
            Assert.IsNotNull(product.description, "Un produit r�cup�r� n'a pas de description.");
            Assert.IsNotNull(product.dimensions, "Le pdt" + product.name + "n'a pas de dimension.");
            Assert.IsTrue(product.price > 0, "Un produit r�cup�r� a un prix non valide.");
        }

        RetrieveTable.OnProductsRetrieved -= null;
    }
}
