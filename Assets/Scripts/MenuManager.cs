using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] public Toggle amountOfTargets;
    [SerializeField] public Toggle timer;
    [SerializeField] public Button sumbit;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] public int amountOrTimer; //amount is 1 timer is 2

    public static MenuManager Instance;
    // Start is called before the first frame update

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        amountOfTargets.isOn = false;
        timer.isOn = false;
        amountOrTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Submit()
    {
        if (amountOfTargets.isOn && !timer.isOn)
        {
            amountOrTimer = 1;
            SceneManager.LoadScene(1);
        } else if (!amountOfTargets.isOn && timer.isOn)
        {
            amountOrTimer = 2;
            SceneManager.LoadScene(1);            
        } else if (!amountOfTargets.isOn && !timer.isOn)
        {
            text.text = "Please select an option";
        } else if (amountOfTargets.isOn && timer.isOn)
        {
            text.text = "Please select only one option";
        }
    }
}
