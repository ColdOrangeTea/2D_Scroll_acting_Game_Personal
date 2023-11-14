using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryText : MonoBehaviour
{
   [SerializeField]
    private List<string> StoryDialogue = new List<string>();
    [SerializeField]
    private int num = 0;
    [SerializeField]
    private Text curStoryDialogue;
    [SerializeField]
    private bool IsDialogueOver = false;

    [SerializeField] private SceneChangeManager manager; // 自行拖入
    [SerializeField] private string scene_to_load = "[SceneName]";
    [SerializeField] private SceneOrder.Scene scene_to_load_name; // 自行設定

    void Start()
    {
        manager = GameObject.Find("SceneChangeManager").GetComponent<SceneChangeManager>();
        scene_to_load = scene_to_load_name.ToString();

        curStoryDialogue = GetComponentInChildren<Text>();
        StoryDialogue.Add("Sasha : …… ");
        StoryDialogue.Add("Sasha : ……頭好痛");
        StoryDialogue.Add("Sasha : 我……腦海裡一片空白");
        StoryDialogue.Add("迷之音 : 快……(模糊)");
        StoryDialogue.Add("Sasha : 腦袋裡有聲音…");
        StoryDialogue.Add("迷之音 : …逃…(模糊) ");
        StoryDialogue.Add("Sasha : 什麼?");
        StoryDialogue.Add("迷之音 : 快逃!!!往地面上逃!!! ");
        StoryDialogue.Add("Sasha : !!!");
    }

    // Update is called once per frame
    void Update()
    { 
        curStoryDialogue.text = StoryDialogue[num].ToString();

        if (StoryDialogue.Count - 1 > num)
        {
            if (Input.GetMouseButtonDown(0))
                num++;
        }
        else if(StoryDialogue.Count - 1 == num && !IsDialogueOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                IsDialogueOver =true;
                manager.GetSceneToLoad(scene_to_load);
            }

        }

    }
}
