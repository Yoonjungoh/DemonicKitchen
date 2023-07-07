using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG
{
    public class Functions
    {
        public static void ChangeScene(int idx)
        {
            SceneManager.LoadScene(idx);
        }

        public static void ChangeScene(string value)
        {
            SceneManager.LoadScene(value);
        }
    }
}
