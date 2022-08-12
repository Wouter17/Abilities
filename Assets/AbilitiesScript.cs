using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Random = UnityEngine.Random;

public class AbilitiesScript : MonoBehaviour {

    public new KMAudio audio;
    public KMBombInfo bomb;
    
    public KMSelectable leftButton;
    public KMSelectable rightButton;
    public KMSelectable centreButton;

    public GameObject screen;
    public TextMesh textMesh;

    public Texture[] textures;
    
    //logic
    private int _currentSelectedIndex;
    private int CurrentSelectedIndex
    {
	    get
	    {
		    return _currentSelectedIndex;
	    }
	    set
	    {
		    _currentSelectedIndex = value;
		    setButtonText(_abilityNames[value]);
	    }
    }

    private List<string> _abilityNames;

    //settings
    private const int AmountOfPossibleOptions = 10;
    private static readonly int MainTex = Shader.PropertyToID("_MainTex");
    
    //logging
    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;

    void Awake () {
        moduleId = moduleIdCounter++;
		
        leftButton.OnInteract += delegate() { leftButtonPressed(); return false; };
        rightButton.OnInteract += delegate() { rightButtonPressed(); return false; };
        centreButton.OnInteract += delegate() { submitButtonPressed(); return false; };

        foreach (var texture in textures)
        {
	        texture.name = texture.name.Replace(';', ':');
        }
        
        generateAnswers();
    }

    private void rightButtonPressed()
	{
		if (moduleSolved) return;
		CurrentSelectedIndex = mod(CurrentSelectedIndex + 1, AmountOfPossibleOptions);
		rightButton.AddInteractionPunch(.5f);
	}
	
	private void leftButtonPressed()
	{
		if (moduleSolved) return;
		CurrentSelectedIndex = mod(CurrentSelectedIndex - 1, AmountOfPossibleOptions);
		leftButton.AddInteractionPunch(.5f);
	}

	private void submitButtonPressed()
	{
		if (moduleSolved) return;
		if (CurrentSelectedIndex == 0)
		{
			moduleSolved = true;
			
			log("Module Solved");
			GetComponent<KMBombModule>().HandlePass();
			audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.CorrectChime, transform);
		}
		else
		{
			log(string.Format("Strike! Expected '{0}' but '{1}' was selected instead", _abilityNames[0], _abilityNames[CurrentSelectedIndex]));
			GetComponent<KMBombModule>().HandleStrike();
			audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.Strike, transform);
			
			generateAnswers();
		}
		centreButton.AddInteractionPunch(.5f);
	}

	private void generateAnswers()
	{
		string[] files = textures.Select(texture => texture.name).ToArray();
		log(string.Format("Generating lucky numbers based on {0} possible files", files.Length));
		
		List<int> fileIndexesOfOptions = generateRandomFromRange(0, files.Length, AmountOfPossibleOptions);
		log("Your lucky numbers are " + fileIndexesOfOptions.Join(", "));
		
		_abilityNames = fileIndexesOfOptions.Select(index => files[index]).ToList();
		setScreenTexture(textures.First(texture => texture.name == _abilityNames[0]));
		log("These represent the following abilities, with the first being the correct answer:\n" + _abilityNames.Join("\n"));

		CurrentSelectedIndex = Random.Range(0, _abilityNames.Count);
	}

	private List<int> generateRandomFromRange(int lowerBound, int upperbound, int amount = 1)
	{
		if (amount < 1) throw new ArgumentException("Amount must be greater than 1");
		if (upperbound == lowerBound) throw new ArgumentException("Upper- and lower-bound cannot be the same");
		if (upperbound - lowerBound < 0)
		{
			var tmp = lowerBound;
			lowerBound = upperbound;
			upperbound = tmp;
		}
		if (upperbound - lowerBound < amount)
			throw new ArgumentException(
				"The difference between the upper- and lower-bound should be greater than the amount of digits generated");
		
		var numbers = new List<int>();

		while (amount > 0)
		{
			int randomNumber = Random.Range(lowerBound, upperbound);
			
			if (numbers.Contains(randomNumber)) continue;
			numbers.Add(randomNumber);
			amount--;
		}

		return numbers;
	}

	private void setButtonText(string text)
	{
		textMesh.text = text;
	}

	private void setScreenTexture(Texture texture)
	{
		screen.GetComponent<Renderer>().material.SetTexture(MainTex, texture);
	}

	int mod(int x, int m) {
		return (x%m + m)%m;
	}

	private void log(string log)
	{
		Debug.Log(string.Format("[Abilities #{0}] " + log, moduleId));
	}
}
