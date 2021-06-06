using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FMOD;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class AudioManager : MonoBehaviour
{
    public static Dictionary<string, EventDescription> events = new Dictionary<string, EventDescription>();
    private List<string> tempPath = new List<string>();

    private void Awake()
    {
        Bank[] _banks = new Bank[0];
        AudioManager.events.Clear();
        if (RuntimeManager.StudioSystem.getBankList(out _banks) == RESULT.OK) // FUCK YOU FMOD :)
        {
            for (int i = 0; i < _banks.Length; i++)
            {
                if (_banks[i].getEventList(out EventDescription[] output) == RESULT.OK)
                {
                    for (int j = 0; j < output.Length; j++)
                    {
                        string path = "NULL:PATH";
                        if (output[j].getPath(out path) != RESULT.OK) Debug.LogWarning("[AudioM] : Couldn't fetch event path");

                        bool halt = false;
                        for (int q = 0; q < tempPath.Count; q++)
                        {
                            if (tempPath[q].Equals(path))
                            {
                                halt = true;
                            }
                        }
                        if (halt) continue;

                        events.Add(path, output[j]);
                        tempPath.Add(path);
                    }
                }
            }

            tempPath.Clear();
            foreach(KeyValuePair<string,EventDescription> eventDescription in events)
            {
                string value = "";
                eventDescription.Value.getPath(out value);
                Debug.Log(eventDescription.Key + ", Holds: " + value);
            }
        }
    }
}
