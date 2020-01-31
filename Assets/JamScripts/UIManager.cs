using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
    
	[HideInInspector]
	public bool InGame = false;

	private ShowPanels showPanels;										//Reference to ShowPanels script on UI GameObject, to show and hide panels
    private CanvasGroup[] menuCanvasGroup;


    void Awake()
	{
        Service.Provide(this);

        //Get a reference to ShowPanels attached to UI object
        showPanels = GetComponent<ShowPanels> ();

		//Get all canvas grounds in my childen, we want to fade them all out at the same time
        menuCanvasGroup = GetComponentsInChildren<CanvasGroup>();
    }


	public void ExitMenuOverlay()
    {
        StartCoroutine(FadeCanvasGroupAlpha(1f, 0f));
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += SceneWasLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= SceneWasLoaded;
    }

    //Once the level has loaded, check if we want to call PlayLevelMusic
    void SceneWasLoaded(Scene scene, LoadSceneMode mode)
    {
		
	}


	public void LoadDelayed()
	{
		//Pause button now works if escape is pressed since we are no longer in Main menu.
        InGame = true;

		//Hide the main menu UI element
		showPanels.HideMenu ();

	}

	public void HideDelayed()
	{
		//Hide the main menu UI element after fading out menu for start game in scene
		showPanels.HideMenu();
	}


    public IEnumerator FadeCanvasGroupAlpha(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        float totalDuration = 1.0f;

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalDuration);

            foreach (var canvasGroup in menuCanvasGroup)
            {
                canvasGroup.alpha = currentAlpha;

            }

            yield return null;
        }

        HideDelayed();
        LoadDelayed();
    }

}
