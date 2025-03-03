using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

[CreateAssetMenu(fileName = "NewCutsceneCollection", menuName = "ScriptableObjects/Cutscenes")]
public class CutsceneCollection : ScriptableObject
{
    [SerializeField] TimelineAsset[] timelines;

    public TimelineAsset GetTimeline(int timelineIndex)
    {
        if (timelines.Length >= (timelineIndex + 1))
        {
            //Debug.LogWarning(timelineIndex.ToString());
            TimelineAsset timelineAsset = timelines[timelineIndex];
            return timelineAsset;
        }

        return null;
    }

}
