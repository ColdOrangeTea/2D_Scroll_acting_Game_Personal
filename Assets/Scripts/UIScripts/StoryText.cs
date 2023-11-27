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
    public List<Sprite> CG = new List<Sprite>();
    public Image CG1;
    [SerializeField] private SceneChangeManager manager;
    [SerializeField] private string scene_to_load = "[SceneName]";
    [SerializeField] private SceneOrder.Scene scene_to_load_name;

    void Start()
    {
        manager = GameObject.Find("SceneChangeManager").GetComponent<SceneChangeManager>();
        CG1 = GameObject.Find("CG1").GetComponent<Image>();
        scene_to_load = scene_to_load_name.ToString();

        curStoryDialogue = GetComponentInChildren<Text>();
        StoryDialogue.Add("單人床上躺著一個小小的人影，他看起來表情痛苦，似乎是做了惡夢");
        StoryDialogue.Add("少年從噩夢中驚醒");
        StoryDialogue.Add("Sasha : …… ");
        StoryDialogue.Add("Sasha : ……頭好痛 ");
        StoryDialogue.Add("Sasha : 我……腦海裡一片空白");
        StoryDialogue.Add("迷之音 : 快……");
        StoryDialogue.Add("Sasha : 腦袋裡有聲音…?");
        StoryDialogue.Add("迷之音 : …逃…");
        StoryDialogue.Add("Sasha : 什麼? ");
        StoryDialogue.Add("迷之音 : 快逃!!!往地面上逃!!!");
    }

    // Update is called once per frame
    void Update()
    {
        curStoryDialogue.text = StoryDialogue[num].ToString();
        if (num == 1)
        {
            CG1.sprite = CG[1];
        }
        if (num == 3)
        {
            CG1.sprite = CG[2];
        }
        if (num == 5)
        {
            CG1.sprite = CG[3];
        }
        if (num == 9)
        {
            CG1.sprite = CG[4];
        }
        if (StoryDialogue.Count - 1 > num)
        {
            if (Input.GetMouseButtonDown(0))
                num++;
        }
        else if (StoryDialogue.Count - 1 == num && !IsDialogueOver)
        {
            if (Input.GetMouseButtonDown(0))
            {
                IsDialogueOver = true;
                manager.GetSceneToLoad(scene_to_load);
            }

        }

    }
}
