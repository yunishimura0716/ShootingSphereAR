using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyClass;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    const int N = 12;
    int max_balloon = 6;
    public int appear_balloon = 0;
    System.Random random;

    [SerializeField]
    GameObject[] spheres = new GameObject[N];
    [SerializeField]
    Text scoreText;

    static int score = 0;

    public List<GameObject> spheres_app = new List<GameObject>();
    public List<GameObject> spheres_dis = new List<GameObject>();

    public int Score
    {
        set
        {
            score = value;

            scoreText.text = score.ToString("D5");
        }
        get
        {
            return score;
        }
    }
    public static int getScore()
    {
        return score;
    }

    // countdown
    float countdown = 3f;
    int count;

    // Start is called before the first frame update
    void Start()
    {
        random = new System.Random();
        for (int i = 0; i < N; i++)
        {
            spheres_dis.Add(spheres[i]);
        }
        Score = 0;
    }

    void FixedUpdate()
    {
        if (countdown >= 0)
        {
            countdown -= Time.deltaTime;
            count = (int)countdown;
        }
        if (countdown <= 0)
        {
            addSphere();
        }
    }

    public void addSphere()
    {
        if (appear_balloon < max_balloon)
        {
            int total_count = max_balloon - appear_balloon;
            for (int i = 0; i < total_count; i++)
            {
                int index = random.Next(spheres_dis.Count);
                GameObject sphere = spheres_dis[index];
                spheres_dis.RemoveAt(index);

                spheres_app.Add(sphere);
                appear_balloon++;
                SphereController controller = sphere.gameObject.GetComponent<SphereController>();
                controller.InitSphere();
            }
        }
    }

    public void removeSphere(GameObject sphere)
    {
        spheres_app.Remove(sphere);
        appear_balloon--;
        spheres_dis.Add(sphere);

        int color = sphere.GetComponent<SphereController>().color;
        if (color == 0) {
            Score += 50;
        } else if (color == 1) {
            Score += 100;
        } else {
            Score += 150;
        }
    }
}
