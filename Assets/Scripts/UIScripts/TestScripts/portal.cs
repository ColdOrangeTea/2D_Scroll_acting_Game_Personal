using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class portal : MonoBehaviour
{
    // public string Level1 = "level_1";
    // public string TestScene1 = "TestScene1";
    // public string TestScene2 = "TestScene2";

    // const int Level1Room = 0;
    // const int Test1Room = 1;
    // const int Test2Room = 2;
    public Scene CurrentScene;
    [SerializeField]
    public enum SceneOrder
    {
        Level1 = 0,
        TestRoom1 = 1,
        TestRoom2 = 2
    }
    public SceneOrder CurrentRoomNum;

    void Start()
    {
        CurrentScene = SceneManager.GetActiveScene();
        Debug.Log("Active Scene is '" + CurrentScene.name + "'.");
    }

    void LoadScene()
    {
        // SceneManager.LoadScene();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // // SceneOrder current_name =SceneOrder.Level1;
        // if (other.gameObject.CompareTag("Player"))
        // {
        //     if (CurrentScene == current_name)
        //     {
        //         Debug.Log("傳送到Level1Room");
        //         SceneManager.LoadScene(Level1);
        //         roomNum = Level1Room;
        //         return;
        //     }
        //     if (CurrentRoomNum == Level1Room)
        //     {
        //         Debug.Log("傳送到TestScene1");
        //         SceneManager.LoadScene(TestScene1);
        //         roomNum = TestRoom;
        //         return;
        //     }
        // }

    }
}
