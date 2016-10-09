using UnityEngine;
using System.Collections;

public class InputDirector : MonoBehaviour
{
    private bool m_IsReplaying = false;

    private ReplaySystem m_ReplaySystem;

    [SerializeField]
    private KeyCode[] m_KeyCode;
    [SerializeField]
    public bool[] m_InputDir;

    void Start()
    {
        m_ReplaySystem = GameObject.Find("ReplayManager").GetComponent<ReplaySystem>();
        m_InputDir = new bool[m_KeyCode.Length];
    }

    void Update()
    {
        if (!m_IsReplaying)
        {
            PlayerInput();
        }
        else
        {
            InputReader();
        }

    }

    public void ResetControls()
    {
        for (int i = 0; i < m_KeyCode.Length; i++)
        {
            m_InputDir[i] = false;
        }
    }

    public float GetAxis(bool minus, bool plus)
    {
        if (minus)
        {
            return -1f;
        }

        if (plus)
        {
            return 1f;
        }

        return 0;
    }

    public void PlayerInput()
    {
        for (int i = 0; i < m_KeyCode.Length; i++)
        {
            if (Input.GetKeyDown(m_KeyCode[i]))
            {
                m_InputDir[i] = true;
                if (m_ReplaySystem.m_Recording)
                {
                    m_ReplaySystem.RecordInput(Time.time, i, true);
                }
            }
            else if (Input.GetKeyUp(m_KeyCode[i]))
            {
                m_InputDir[i] = false;
                if (m_ReplaySystem.m_Recording)
                {
                    m_ReplaySystem.RecordInput(Time.time, i, false);
                }
            }
        }
    }
    public void InputReader()
    {
        for (int i = 0; i < m_ReplaySystem.m_ReplayInput.Count; i++)
        {
            if (m_ReplaySystem.m_ReplayInput[i].timeStamp <= Time.time)
            {
                int index = m_ReplaySystem.m_ReplayInput[i].index;
                bool pressed = m_ReplaySystem.m_ReplayInput[i].pressed;

                if(pressed)
                {
                    m_InputDir[index] = true;
                }
                else
                {
                    m_InputDir[index] = false;
                }

                m_ReplaySystem.m_ReplayInput.RemoveAt(i);
            }
        }
    }

    public void ToggleReplay()
    {
        if (m_IsReplaying == false)
        {
            m_ReplaySystem.GetComponent<XMLReader>().XMLLoad();
            m_IsReplaying = true;
        }
        else
        {
            m_IsReplaying = false;
        }
    }
}