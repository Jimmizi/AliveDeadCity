using System;
using UnityEngine;
using System.Collections;
using com.ootii.Utilities.Debug;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UiManager : MonoBehaviour
{
    private enum Positioning
    {
        StartToIdle,
        Idling,
        IdleToEnd,
        Fade,
        BackToOrigin,
        Done
    }

    public float TitleScreenStartYPos;
    public float TitleScreenIdleYPos;
    public float TitleScreenEndYPos;

    public float StartToIdleTime = 3.0f;
    
    public CanvasGroup FaderForBackToOrigin;

    [HideInInspector]
	public bool InGame = false;

    private Positioning mCurrentPosition = Positioning.StartToIdle;

    private bool mTitleIdlingFlipDir = false;

    private ShowPanels showPanels;										//Reference to ShowPanels script on UI GameObject, to show and hide panels
    private CanvasGroup[] menuCanvasGroup;


    void Awake()
	{
        Service.Provide(this);

        //Get a reference to ShowPanels attached to UI object
        showPanels = GetComponent<ShowPanels> ();

		//Get all canvas grounds in my childen, we want to fade them all out at the same time
        menuCanvasGroup = GetComponentsInChildren<CanvasGroup>();

        var pos = Camera.main.transform.position;
        pos.y = TitleScreenStartYPos;
        Camera.main.transform.position = pos;
    }

    void Start()
    {
        if (Service.Test().SkipTitle)
        {
            SceneManager.UnloadSceneAsync("TitleScreen");
            Camera.main.transform.position = new Vector3(0, 0, -10);
            InGame = true;
        }
    }

    void Update()
    {
        if (InGame)
        {
            return;
        }

        if (mCurrentPosition < Positioning.IdleToEnd)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ExitMenuOverlay();
            }
        }

        //Hacky af but whatever
        switch (mCurrentPosition)
        {
            case Positioning.StartToIdle:
            {
                var pos = Camera.main.transform.position;
                Camera.main.transform.position = new Vector3(pos.x, Mathf.Lerp(pos.y, TitleScreenIdleYPos, Time.deltaTime / StartToIdleTime), pos.z);

                if (Camera.main.transform.position.y <= TitleScreenIdleYPos + 0.5f)
                {
                    mCurrentPosition = Positioning.Idling;
                }

                break;
            }
                
            case Positioning.Idling:
            {
                if (mTitleIdlingFlipDir) //up
                {
                    var pos = Camera.main.transform.position;
                    Camera.main.transform.position = new Vector3(pos.x, Mathf.Lerp(pos.y, TitleScreenIdleYPos + 0.2f, Time.deltaTime / (StartToIdleTime * 2)), pos.z);

                    if (Camera.main.transform.position.y >= TitleScreenIdleYPos + 0.1f)
                    {
                        mTitleIdlingFlipDir = false;
                    }
                }
                else
                {
                    var pos = Camera.main.transform.position;
                    Camera.main.transform.position = new Vector3(pos.x, Mathf.Lerp(pos.y, TitleScreenIdleYPos - 0.2f, Time.deltaTime / (StartToIdleTime * 2)), pos.z);

                    if (Camera.main.transform.position.y <= TitleScreenIdleYPos - 0.1f)
                    {
                        mTitleIdlingFlipDir = true;
                    }
                }
                break;
            }
            case Positioning.IdleToEnd:
            {
                var pos = Camera.main.transform.position;
                Camera.main.transform.position = new Vector3(pos.x, Mathf.Lerp(pos.y, TitleScreenEndYPos, Time.deltaTime), pos.z);

                if (Camera.main.transform.position.y <= TitleScreenEndYPos + 0.5f)
                {
                    mCurrentPosition = Positioning.Fade;
                }
                
                break;
            }
            case Positioning.Fade:
            {
                FaderForBackToOrigin.alpha = Mathf.Lerp(FaderForBackToOrigin.alpha, 1.0f, Time.deltaTime * 2);

                if (FaderForBackToOrigin.alpha >= 0.6f)
                {
                    FaderForBackToOrigin.alpha = 1.0f;
                    mCurrentPosition = Positioning.BackToOrigin;
                    SceneManager.UnloadSceneAsync("TitleScreen");
                    }
                break;
            }
            case Positioning.BackToOrigin:
            {
                Camera.main.transform.position = new Vector3(0, 0, -10);
                StartCoroutine(FadeOutScreenFader(1f, 0f));
                mCurrentPosition = Positioning.Done;
                break;
            }
        }
    }

	public void ExitMenuOverlay()
    {
        mCurrentPosition = Positioning.IdleToEnd;
        Debug.Log("Pressed Start");
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

    public IEnumerator FadeOutScreenFader(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;
        float totalDuration = 1.0f;

        InGame = true;

        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;
            float currentAlpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / totalDuration);

            FaderForBackToOrigin.alpha = currentAlpha;

            yield return null;
        }

    }

}
