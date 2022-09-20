using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingManager : MonoBehaviour
{

    [SerializeField] List<GameObject> loadImages = new List<GameObject>();
    [SerializeField] string SceneName;
    [SerializeField] float progress;

    private GameObject currLoadImage = null;
    private Vector3 loadImagePos = new Vector3(-3, 0, -2.5f);
    private Vector3 loadWeaponScale = new Vector3(3, 3, 3);
    private Vector3 loadWeaponRot = new Vector3(45, 90, 0);
    private Scene scene;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        BeginLoading(SceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void BeginLoading(string sceneName)
    {
        CreateLoadImage();
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(string sceneName)
    {
        

        yield return null;
        
        if (SceneManager.GetSceneByName(sceneName) != null)
        {
            //Begin to load the Scene you specify
            scene = SceneManager.GetSceneByName(SceneName);
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneName);
            asyncOperation.allowSceneActivation = true;

            while (!asyncOperation.isDone)
            {
                progress = asyncOperation.progress;
                Debug.Log(progress);
                currLoadImage.transform.Rotate(Vector3.up, 1);
                
                yield return new WaitForEndOfFrame();
            }
            

            StopAllCoroutines();

        }
        else
        {
            Debug.Log("scene name not recognized");
        }
    }

    private void CreateLoadImage()
    {
        if (currLoadImage != null)
        {
            Destroy(currLoadImage);
        }

        currLoadImage = Instantiate(loadImages[Random.Range(0, loadImages.Count)]);
        currLoadImage.transform.position = loadImagePos;
        if (currLoadImage.layer != 6)
        {
            
            currLoadImage.transform.localScale = loadWeaponScale;
        }
        StartCoroutine(Delay(10));
    }
    
    IEnumerator Delay(float time)
    {
        yield return new WaitForSeconds(time);
        CreateLoadImage();
    }
}
