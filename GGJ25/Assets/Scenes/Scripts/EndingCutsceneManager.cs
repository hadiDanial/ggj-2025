using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingCutsceneManager : MonoBehaviour
{
    private bool didEndCutscene = false;    

    public Animator cutsceneAnimator;

    private FixableObjectCurve[] fixCheckList;

    void Start()
    {
        fixCheckList = GameObject.FindObjectsOfType<FixableObjectCurve>();

    }

    void Update()
    {
        if(didEndCutscene)
        {
            return;
        }
        else
        {
            CheckEndCutscene();
        }
    }

    private void CheckEndCutscene()
    {
        bool allFixed = true;
        foreach(FixableObjectCurve fix in fixCheckList)
        {
            allFixed = allFixed && fix.IsClean;
        }
        Debug.Log("All fixed "+allFixed.ToString()+" time: "+Time.time);

        if(allFixed)
        {
            didEndCutscene = true;
            cutsceneAnimator.SetTrigger("EndingCutscene");
        }
    }
}
