using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class portal : MonoBehaviour
{
    public string Level1 = "level_1";
    public string TestScene1 = "TestScene1";
    public static int roomNum = 0;
    const int TestRoom = 0;
    const int Level1Room = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (roomNum == TestRoom)
            {
                Debug.Log("傳送到Level1Room");
                SceneManager.LoadScene(Level1);
                roomNum = Level1Room;
                return;
            }
            if (roomNum == Level1Room)
            {
                Debug.Log("傳送到TestScene1");
                SceneManager.LoadScene(TestScene1);
                roomNum = TestRoom;
                return;
            }
        }

    }
}
