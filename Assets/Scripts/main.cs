using System.Collections.Generic;
using UnityEngine;
using Microsoft.Azure.Kinect.BodyTracking;

public class main : MonoBehaviour
{
    // Handler for SkeletalTracking thread.
    public GameObject m_tracker;
    private BackgroundDataProvider m_backgroundDataProvider;
    public BackgroundData m_lastFrameData = new BackgroundData();

    void Start()
    {
        
        SkeletalTrackingProvider m_skeletalTrackingProvider = new SkeletalTrackingProvider();

        //tracker ids needed for when there are two trackers
        const int TRACKER_ID = 0;
        m_skeletalTrackingProvider.StartClientThread(TRACKER_ID);
        m_backgroundDataProvider = m_skeletalTrackingProvider;
    }

    void Update()
    {
        Debug.Log("1");
        if (m_backgroundDataProvider.IsRunning)
        {
            Debug.Log("2");
            if (m_backgroundDataProvider.GetCurrentFrameData(ref m_lastFrameData))
            {
                Debug.Log("3");
                if (m_lastFrameData.NumOfBodies != 0)
                {
                    Debug.Log("4");
                    m_tracker.GetComponent<TrackerHandler>().updateTracker(m_lastFrameData);
                }
            }
        }
    }

    void OnApplicationQuit()
    {
        // Stop background threads.
        if (m_backgroundDataProvider != null)
        {
            m_backgroundDataProvider.StopClientThread();
        }
    }
}
