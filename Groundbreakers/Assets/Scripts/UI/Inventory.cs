using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public Sprite[] moduleIcons = new Sprite[numSlots];
    public GameObject[] modules = new GameObject[numSlots];
    public const int numSlots = 30;

    public void AddModule(GameObject module)
    {
        for (int i = 0; i < modules.Length; i++)
        {
            if (modules[i] == null)
            {
                ModuleTemplate script = module.GetComponent<ModuleTemplate>();
                modules[i] = module;
                moduleIcons[i] = script.icon;
                return;
            }
        }
    }

    public void RemoveModule(GameObject moduleToRemove)
    {
        for (int i = 0; i < modules.Length; i++)
        {
            if (modules[i] == moduleToRemove)
            {
                modules[i] = null;
                moduleIcons[i] = null;
                return;
            }
        }
    }
}
