﻿using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class NotesData : ScriptableObject
{
    public List<NoteData> notes = new List<NoteData>();

    [Serializable]
    public class NoteData
    {
        public string Id;
        public LocalizationData[] Localizations;
    }

    [Serializable]
    public class LocalizationData
    {
        public string LocaleId;
        [TextArea]
        public string Text;
    }
}
