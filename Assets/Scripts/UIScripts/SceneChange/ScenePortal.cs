using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScenePortal : MonoBehaviour
{
    [SerializeField] private SceneChangeManager manager; // 自行拖入
    [SerializeField] private string scene_to_load = "[SceneName]";
    [SerializeField] private SceneOrder.Scene scene_to_load_name; // 自行設定
    [SerializeField] private Text portal_name; // 自行拖入

    void Start()
    {
        manager = GameObject.Find("SceneChangeManager").GetComponent<SceneChangeManager>();
        scene_to_load = scene_to_load_name.ToString();
        portal_name.text = scene_to_load;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            manager.GetSceneToLoad(scene_to_load);
        }

    }
}
