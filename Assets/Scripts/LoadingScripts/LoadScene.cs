using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public TextMeshProUGUI beosText;
    public string sceneToLoad;

    // Start is called before the first frame update
    void Start()
    {
        sceneToLoad = LoadingData.sceneToLoad;
        StartCoroutine(LoadUpScene());
    }

    IEnumerator LoadUpScene()
    {
        beosText.enabled = true;

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        yield return new WaitForSeconds(10f);

        operation.allowSceneActivation = true;

        while (true)
        {
            yield return null;
        }
    }
}
