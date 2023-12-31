using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoryText : MonoBehaviour
{
    [SerializeField]
    private List<string> OpeningStoryDialogue = new List<string>();
    [SerializeField]
    private List<string> EndingStoryDialogue = new List<string>();


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
        if (SceneChangeManager.CurrentScene.name == SceneOrder.Scene.StoryScene01.ToString())
        {
            OpeningStoryDialogue.Add("單人床上躺著一個小小的人影，他看起來表情痛苦，似乎是做了惡夢");
            OpeningStoryDialogue.Add("少年從噩夢中驚醒");
            OpeningStoryDialogue.Add("Sasha : …… ");
            OpeningStoryDialogue.Add("Sasha : ……頭好痛 ");
            OpeningStoryDialogue.Add("Sasha : 我……腦海裡一片空白");
            OpeningStoryDialogue.Add("迷之音 : 快……");
            OpeningStoryDialogue.Add("Sasha : 腦袋裡有聲音…?");
            OpeningStoryDialogue.Add("迷之音 : …逃…");
            OpeningStoryDialogue.Add("Sasha : 什麼? ");
            OpeningStoryDialogue.Add("迷之音 : 快逃!!!往地面上逃!!!");
        }

        if (SceneChangeManager.CurrentScene.name == SceneOrder.Scene.StoryScene02.ToString())
        {
            EndingStoryDialogue.Add("經過重重阻礙，薩沙最終成功逃離設施，但在前方等著他的是未知的世界...");
            EndingStoryDialogue.Add("~END~");

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (SceneChangeManager.CurrentScene.name == SceneOrder.Scene.StoryScene01.ToString())
        {
            curStoryDialogue.text = OpeningStoryDialogue[num].ToString();
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
            if (OpeningStoryDialogue.Count - 1 > num)
            {
                if (Input.GetMouseButtonDown(0))
                    num++;
            }
            else if (OpeningStoryDialogue.Count - 1 == num && !IsDialogueOver)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    IsDialogueOver = true;
                    manager.GetSceneToLoad(scene_to_load);
                }
            }
        }

        if (SceneChangeManager.CurrentScene.name == SceneOrder.Scene.StoryScene02.ToString())
        {
            curStoryDialogue.text = EndingStoryDialogue[num].ToString();
            if (num == 0)
            {
                CG1.sprite = CG[5];
            }
            if (EndingStoryDialogue.Count - 1 > num)
            {
                if (Input.GetMouseButtonDown(0))
                    num++;
            }
            else if (EndingStoryDialogue.Count - 1 == num && !IsDialogueOver)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    IsDialogueOver = true;
                    manager.GetSceneToLoad(scene_to_load);
                }
            }
        }




    }
}
