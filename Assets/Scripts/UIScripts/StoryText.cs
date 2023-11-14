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

    [SerializeField] private SceneChangeManager manager; // �ۦ��J
    [SerializeField] private string scene_to_load = "[SceneName]";
    [SerializeField] private SceneOrder.Scene scene_to_load_name; // �ۦ�]�w

    void Start()
    {
        manager = GameObject.Find("SceneChangeManager").GetComponent<SceneChangeManager>();
        scene_to_load = scene_to_load_name.ToString();

        curStoryDialogue = GetComponentInChildren<Text>();
        StoryDialogue.Add("Sasha : �K�K ");
        StoryDialogue.Add("Sasha : �K�K�Y�n�h");
        StoryDialogue.Add("Sasha : �ڡK�K�����̤@���ť�");
        StoryDialogue.Add("�g���� : �֡K�K(�ҽk)");
        StoryDialogue.Add("Sasha : ���U�̦��n���K");
        StoryDialogue.Add("�g���� : �K�k�K(�ҽk) ");
        StoryDialogue.Add("Sasha : ����?");
        StoryDialogue.Add("�g���� : �ְk!!!���a���W�k!!! ");
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
