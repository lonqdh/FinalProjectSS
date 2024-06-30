using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Đây là một generic class, nó defines structure và hành vi của class, nhưng mà để specific data type của class là T as a placeholder to be specified later
//Generic classes use placeholders like <T> to represent the data type that will be used with the class. This allows a single class definition to work with different data types, such as integers, strings, or custom objects.
//So for classes that inherit from the Singleton<T> generic class, which inherits from the unity base MonoBehaviour class, will indirectly gains access to the functionalities of MonoBehaviour and fulfills the requirement of the constraint "where T : MonoBehaviour".
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            if(instance == null)
            {
                instance = new GameObject().AddComponent<T>();
            }
            return instance;
        }
    }
}
