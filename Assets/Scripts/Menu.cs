using UnityEngine;

public class Menu : MonoBehaviour
{
    public void Play() => GameManager.instance.OnGoNextScene("RAScene");
}
