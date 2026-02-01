using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScenesManager : SingletonPersistent<ScenesManager>
{
    private bool cutsceneStart = false;
    [SerializeField]
    private Image currentImage;
    [SerializeField]
    private List<CutSceneInfo> cutscenes = new List<CutSceneInfo>();
    [SerializeField]
    private List<string> sceneNames = new List<string>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Introduction());
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "CutsceneJunkyard" && cutsceneStart == false)
        {
            cutsceneStart = true;
            StartCoroutine(Junkyard());
        }
        if (SceneManager.GetActiveScene().name == "CutSceneCity" && cutsceneStart == false)
        {
            cutsceneStart = true;
            StartCoroutine(City());
        }
    }
    IEnumerator City()
    {
        currentImage = GameObject.FindAnyObjectByType<Image>();
        var scene = cutscenes[2];
        foreach (Sprite img in scene.SceneImages)
        {
            if (img != null)
            {
                currentImage.sprite = img;
            }
            yield return new WaitForSeconds(scene.speed);
        }

        SceneManager.LoadScene(sceneNames[2]);
        cutsceneStart = false;
    }
    IEnumerator Junkyard()
    {

        currentImage = GameObject.FindAnyObjectByType<Image>();
        var scene = cutscenes[1];
        foreach (Sprite img in scene.SceneImages)
        {
            if (img != null)
            {
                currentImage.sprite = img;
            }
            yield return new WaitForSeconds(scene.speed);
        }

        SceneManager.LoadScene(sceneNames[1]);
        cutsceneStart = false;
    }
    IEnumerator Introduction()
    {
        var scene = cutscenes[0];
        foreach (Sprite img in scene.SceneImages)
        {
            if (img != null)
            {
                currentImage.sprite = img;
            }
            yield return new WaitForSeconds(scene.speed);
        }
        yield return new WaitForSeconds(scene.speed);
        SceneManager.LoadScene(sceneNames[0]);
    }
}
[System.Serializable]
struct CutSceneInfo
{
    public float speed;
    [TextArea]
    public string SceneName;
    public List<Sprite> SceneImages;
}
