using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReplaySystem : MonoBehaviour
{
    public bool m_Recording;
    public List<TimeStamp> m_ReplayInput = new List<TimeStamp>();

    public void RecordInput(float tStamp, int tIndex, bool pressed)
    {
        TimeStamp ts = new TimeStamp();
        ts.timeStamp = tStamp;
        ts.index = tIndex;
        ts.pressed = pressed;

        m_ReplayInput.Add(ts);
    }

    public void ToggleRecording()
    {
        if(m_Recording)
        {
            m_Recording = false;
        }
        else
        {
            m_Recording = true;
        }
    }

    public struct TimeStamp
    {
        public float timeStamp;
        public int index;
        public bool pressed;
    }

    public struct InputTracker
    {
        public bool left;
        public bool right;
        public bool jump;
        public bool shoot;
        public bool burstShoot;
    }
}