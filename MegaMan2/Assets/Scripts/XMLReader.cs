using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEditor;

public class XMLReader : MonoBehaviour
{
    private XmlDocument m_XMLDoc;
    private ReplaySystem m_ReplaySystem;
    
    void Start()
    {
        m_ReplaySystem = GameObject.Find("ReplayManager").GetComponent<ReplaySystem>();
    }

    public void XMLSave()
    {
        string path = "XML/Example.xml";

        StreamWriter textFile = File.CreateText(Application.dataPath + "/" + path);
        textFile.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
        textFile.Write("\n<Inputs>");

        for (int i = 0; i < m_ReplaySystem.m_ReplayInput.Count; i++)
        {
            WriteNodes(textFile, m_ReplaySystem.m_ReplayInput[i].timeStamp.ToString(), m_ReplaySystem.m_ReplayInput[i].index.ToString(), m_ReplaySystem.m_ReplayInput[i].pressed.ToString());
        }

        textFile.Write("\n</Inputs>");
        textFile.Close();
    }

    public void XMLLoad()
    {
        string path = "XML/Example.xml";

        m_XMLDoc = new XmlDocument();
        m_XMLDoc.Load(Application.dataPath + "/" + path);

        XmlElement baseNode = m_XMLDoc.DocumentElement;
        m_ReplaySystem.m_ReplayInput = new List<ReplaySystem.TimeStamp>();

        for (int i = 0; i < baseNode.ChildNodes.Count; i++)
        {
            XmlNode parent = baseNode.ChildNodes[i];

            float timeStamp = float.Parse(parent.ChildNodes[0].InnerText);
            int index = int.Parse(parent.ChildNodes[1].InnerText);
            bool pressed = bool.Parse(parent.ChildNodes[2].InnerText);

            ReplaySystem.TimeStamp ts = new ReplaySystem.TimeStamp();
            ts.timeStamp = timeStamp;
            ts.index = index;
            ts.pressed = pressed;

            Debug.Log("TimeStamp: " + ts.timeStamp + "/" + ts.index + "/" + ts.pressed);

            m_ReplaySystem.m_ReplayInput.Add(ts);
        }
    }

    private void WriteNodes(StreamWriter streamWriter, string timestamp, string keyCode, string pressed)
    {
        streamWriter.Write("\n\t<Input>");
        streamWriter.Write("\n\t\t<Timestamp>" + timestamp + "</Timestamp>");
        streamWriter.Write("\n\t\t<KeyCode>" + keyCode + "</KeyCode>");
        streamWriter.Write("\n\t\t<pressed>" + pressed + "</pressed>");
        streamWriter.Write("\n\t</Input>");
    }
}