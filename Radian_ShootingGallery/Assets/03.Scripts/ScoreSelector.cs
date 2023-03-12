using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreSelector : MonoBehaviour
{
    public float letterChangeThreshold = 3f;
    public float currentInputValue=0;
    public int currentLetter;
    public int currentLetterIndex;
    public TextMeshProUGUI scoreTextHolder, nameTextHolder;
    public char[] currentName;

    bool inputMode=true;
    public ScoreManager scoreManager;
 
    // Start is called before the first frame update
    void Start()
    {
        currentLetter = 65;
    }

    // Update is called once per frame
    void Update()
    {
        

        if (!nameTextHolder || !scoreTextHolder || !scoreManager) return;
        if (inputMode)
        {
            print((Input.GetAxis("Horizontal") != 0));
            if (Input.GetAxis("Horizontal") != 0)
                SelectLetter(Input.GetAxis("Horizontal"));
            else
                currentInputValue = 0;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                currentLetterIndex++;
                if (currentLetterIndex>2)
                {
                    nameTextHolder.text += "END";
                    inputMode = false;

                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {

                scoreManager.AddScore(scoreTextHolder.text, nameTextHolder.text);
            }
        }
    }

    public void SelectLetter(float x)
    {
        if (currentInputValue<letterChangeThreshold && currentInputValue>-letterChangeThreshold)
        {
            currentInputValue += x;
        }
        else
        {
            if (currentInputValue >= letterChangeThreshold)
            {
                currentLetter = currentLetter+1<91?currentLetter+1:65;
                Debug.Log((char)currentLetter);
                currentInputValue = 0;
                
                currentName = nameTextHolder.text.ToCharArray(); 
                currentName[currentLetterIndex] = (char)currentLetter;
                nameTextHolder.text =new string(currentName);
            }
            else if (currentInputValue <= -letterChangeThreshold)
            {
                currentLetter = currentLetter - 1 < 65 ? 90: currentLetter -1;
                Debug.Log((char)currentLetter);
                currentInputValue = 0;
                currentName = nameTextHolder.text.ToCharArray();
                currentName[currentLetterIndex] = (char)currentLetter;
                nameTextHolder.text = new string(currentName);
            }
          
        }
    }
}
