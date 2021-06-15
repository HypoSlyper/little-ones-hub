using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void SetLevel(Level level)
    {
        LevelSelection.instance.SetLevel(level);
    }
}
