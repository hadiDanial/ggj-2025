using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TypeWriter : MonoBehaviour 
{
	public bool typeOnStart = true;

	public float typeDelay;

	public SpecialChar[] specialCharacters;

	private Text text;
	private string textInfo; 

	public AudioSource audioSource;

	void Start()
	{
		text = GetComponent<Text>();
		textInfo = text.text;
		text.text = "";

		audioSource = GetComponent<AudioSource>();

		if(typeOnStart)
		{
			PlayText();
		}
	}

	public void PlayText()
	{
		StopCoroutine("PlayTextCoroutine");
		StartCoroutine("PlayTextCoroutine");
	}

	IEnumerator PlayTextCoroutine()
	{
		foreach(char c in textInfo)
		{
			text.text += c;
			if(audioSource != null)
			{
				audioSource.Play();
			}
			bool found = false;
			foreach(SpecialChar sc in specialCharacters)
			{
				if(sc.specialCharacter == c && !found)
				{
					found = true;
					yield return new WaitForSeconds(typeDelay * sc.waitTimeModifier);
				}
			}
			if(!found)
			{
				yield return new WaitForSeconds(typeDelay);
			}
		}
	}

	[System.Serializable]
	public class SpecialChar
	{
		public char specialCharacter = ' ';
		public float waitTimeModifier = 1; //will wait delayTime * waitTimeModifier

	}

    public void ResetText()
    {
        text.text = "";
    }
}
