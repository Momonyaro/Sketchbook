﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueTrigger : MonoBehaviour
{
    [Tooltip("The text that appears when the player touches the trigger")]
    public string dialogue;
    [Tooltip("The time (in seconds) that passes after a character in the sentence has been displayed before the next character gets displayed")]
    public float timeBetweenCharacters = 0.05f;
    [Tooltip("The TextMeshPro object that displays the dialogue text")]
    public TMP_Text dialogueText;

    // Use this for initialization
    void Start()
    {
        dialogueText.text = "";
    }

    IEnumerator TypeScentence(string sentence)
    {
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(timeBetweenCharacters);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && dialogueText.text == "")
        {
            StartCoroutine(TypeScentence(dialogue));
        }
    }
}