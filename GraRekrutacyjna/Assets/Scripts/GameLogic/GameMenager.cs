using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMenager : MonoBehaviour
{
    public Text endGameText;
    public float restartDelay = 5f;
    private float destroyedBoxes = 0;

    private void Start()
    {
        if (endGameText != null)
        {
            endGameText.gameObject.SetActive(false);
        }
    }
    public void ShowCongratulations()
    {
        destroyedBoxes++;

        Debug.Log(destroyedBoxes);

        if (destroyedBoxes >= 5 )
        {
            if (endGameText != null)
            {
                endGameText.gameObject.SetActive(true);
                Debug.Log("pokazuje tekst");

                Invoke("RestartGame", restartDelay);
            }
        }
    }

    private void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }
}
