using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuSelectionManager : MonoBehaviour
{
    private MenuManager menuManager;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TextMeshProUGUI inputFieldText;
    // Start is called before the first frame update
    void Start()
    {
        menuManager = MenuManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (menuManager.amountOrTimer == 1)
        {
            text.text = "How many targets do you want to hit?";
            inputFieldText.text = "Enter number of targets";
            
        } else if (menuManager.amountOrTimer == 2)
        {
            text.text = "How many seconds do you want to go for?";
            inputFieldText.text = "Enter how many seconds";            
        }
    }

    public void BackToMenu()
    {
        menuManager.amountOrTimer = 0;
        menuManager.timer.isOn = false;
        menuManager.amountOfTargets.isOn = false;
        menuManager.sumbit.onClick.AddListener(menuManager.Submit);
        SceneManager.LoadScene(0);
    }

    //public void Submit()
    //{
    //    try 
    //    {
    //        Debug.Log("Started");
    //        int.Parse(inputField.text);
    //        Debug.Log("Ended");
    //    } catch (System.FormatException)
    //    {
    //        text.text = "Please enter a number";
    //    }
    //    //SceneManager.LoadScene(2);
    //}

    public void Submit()
    {
        try
        {
            Debug.Log("Started");
            int number = int.Parse(inputField.text);
            Debug.Log("Parsed number: " + number);
            // Uncomment the following line to load the scene after a successful parse
            // SceneManager.LoadScene(2);
        }
        catch (System.FormatException)
        {
            text.text = "Please enter a valid number.";
            Debug.LogWarning("Invalid input: " + inputField.text);
        }
        catch (System.Exception ex)
        {
            text.text = "An unexpected error occurred: " + ex.Message;
            Debug.LogError("Error: " + ex);
        }
        finally
        {
            Debug.Log("Ended");
        }
    }

}
