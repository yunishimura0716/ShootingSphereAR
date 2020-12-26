using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public Text timeTexts;
    public Text CountText;
    float totalTime = 10;
    int retime;
    float countdown = 3f;
    int count;

    // Start is called before the first frame update
    void Start()
    {
        retime = (int)totalTime;
        timeTexts.text = retime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (countdown >= 0)
        {
            countdown -= Time.deltaTime;
            count = (int)countdown;
            CountText.text = count.ToString();
        }
        if (countdown <= 0)
        {
            CountText.text = "";
            totalTime -= Time.deltaTime;
            retime = (int)totalTime;
            timeTexts.text = retime.ToString();
            
            if (retime == 0)
            {
                // new scene
                SceneManager.LoadScene("Scenes/Result");
            }
        }
    }
}
