using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;


public static class EventManager
{

    static bool initialized = false;


    #region Empty Events

    static Dictionary<EMPTYEVENTS, List<UnityEvent>>
        emptyInvokers = new Dictionary<EMPTYEVENTS, List<UnityEvent>>();
    static Dictionary<EMPTYEVENTS, List<UnityAction>>
        emptyListeners = new Dictionary<EMPTYEVENTS, List<UnityAction>>();



    public static void AddInvoker(EMPTYEVENTS eventName, UnityEvent invoker)
    {
        emptyInvokers[eventName].Add(invoker);
        foreach (UnityAction listener in emptyListeners[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void AddListener(EMPTYEVENTS eventName, UnityAction listener)
    {
        emptyListeners[eventName].Add(listener);
        foreach (UnityEvent invoker in emptyInvokers[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void RemoveListener(EMPTYEVENTS eventName, UnityAction listener)
    {
        foreach (UnityEvent invoker in emptyInvokers[eventName])
        {
            invoker.RemoveListener(listener);
        }
        emptyListeners[eventName].Remove(listener);
    }

    public static void RemoveInvoker(EMPTYEVENTS eventName, UnityEvent invoker)
    {
        foreach (UnityAction listener in emptyListeners[eventName])
        {
            invoker.RemoveListener(listener);
        }
        emptyInvokers[eventName].Remove(invoker);
    }


    #endregion


    #region Bool Events


    static Dictionary<BOOLEVENTS, List<UnityEvent<bool>>>
        boolInvokers = new Dictionary<BOOLEVENTS, List<UnityEvent<bool>>>();
    static Dictionary<BOOLEVENTS, List<UnityAction<bool>>>
        boolListeners = new Dictionary<BOOLEVENTS, List<UnityAction<bool>>>();



    public static void AddInvoker(BOOLEVENTS eventName, UnityEvent<bool> invoker)
    {
        boolInvokers[eventName].Add(invoker);
        foreach (UnityAction<bool> listener in boolListeners[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void AddListener(BOOLEVENTS eventName, UnityAction<bool> listener)
    {
        boolListeners[eventName].Add(listener);
        foreach (UnityEvent<bool> invoker in boolInvokers[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void RemoveListener(BOOLEVENTS eventName, UnityAction<bool> listener)
    {
        foreach (UnityEvent<bool> invoker in boolInvokers[eventName])
        {
            invoker.RemoveListener(listener);
        }
        boolListeners[eventName].Remove(listener);
    }

    public static void RemoveInvoker(BOOLEVENTS eventName, UnityEvent<bool> invoker)
    {
        foreach (UnityAction<bool> listener in boolListeners[eventName])
        {
            invoker.RemoveListener(listener);
        }
        boolInvokers[eventName].Remove(invoker);
    }

    #endregion

    #region Float Int Events

    static Dictionary<FLOATINTEVENTS, List<UnityEvent<float, int>>>
        floatIntInvokers = new Dictionary<FLOATINTEVENTS, List<UnityEvent<float, int>>>();
    static Dictionary<FLOATINTEVENTS, List<UnityAction<float, int>>>
        floatIntListeners = new Dictionary<FLOATINTEVENTS, List<UnityAction<float, int>>>();



    public static void AddInvoker(FLOATINTEVENTS eventName, UnityEvent<float, int> invoker)
    {
        floatIntInvokers[eventName].Add(invoker);
        foreach (UnityAction<float, int> listener in floatIntListeners[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void AddListener(FLOATINTEVENTS eventName, UnityAction<float, int> listener)
    {
        floatIntListeners[eventName].Add(listener);
        foreach (UnityEvent<float, int> invoker in floatIntInvokers[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void RemoveListener(FLOATINTEVENTS eventName, UnityAction<float, int> listener)
    {
        foreach (UnityEvent<float, int> invoker in floatIntInvokers[eventName])
        {
            invoker.RemoveListener(listener);
        }
        floatIntListeners[eventName].Remove(listener);
    }

    public static void RemoveInvoker(FLOATINTEVENTS eventName, UnityEvent<float, int> invoker)
    {
        foreach (UnityAction<float, int> listener in floatIntListeners[eventName])
        {
            invoker.RemoveListener(listener);
        }
        floatIntInvokers[eventName].Remove(invoker);
    }


    #endregion

    #region Int Events

    static Dictionary<INTEVENTS, List<UnityEvent<int>>>
        intInvokers = new Dictionary<INTEVENTS, List<UnityEvent<int>>>();
    static Dictionary<INTEVENTS, List<UnityAction<int>>>
        intListeners = new Dictionary<INTEVENTS, List<UnityAction<int>>>();



    public static void AddInvoker(INTEVENTS eventName, UnityEvent<int> invoker)
    {
        intInvokers[eventName].Add(invoker);
        foreach (UnityAction<int> listener in intListeners[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void AddListener(INTEVENTS eventName, UnityAction<int> listener)
    {
        intListeners[eventName].Add(listener);
        foreach (UnityEvent<int> invoker in intInvokers[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void RemoveListener(INTEVENTS eventName, UnityAction<int> listener)
    {
        foreach (UnityEvent<int> invoker in intInvokers[eventName])
        {
            invoker.RemoveListener(listener);
        }
        intListeners[eventName].Remove(listener);
    }

    public static void RemoveInvoker(INTEVENTS eventName, UnityEvent<int> invoker)
    {
        foreach (UnityAction<int> listener in intListeners[eventName])
        {
            invoker.RemoveListener(listener);
        }
        intInvokers[eventName].Remove(invoker);
    }


    #endregion

    #region Float Vector3 Events

    static Dictionary<FLOATVECTOR3EVENTS, List<UnityEvent<float, Vector3>>>
        floatVectorInvokers = new Dictionary<FLOATVECTOR3EVENTS, List<UnityEvent<float, Vector3>>>();

    static Dictionary<FLOATVECTOR3EVENTS, List<UnityAction<float, Vector3>>>
        floatVectorListeners = new Dictionary<FLOATVECTOR3EVENTS, List<UnityAction<float, Vector3>>>();


    public static void AddInvoker(FLOATVECTOR3EVENTS eventName, UnityEvent<float, Vector3> invoker)
    {

        floatVectorInvokers[eventName].Add(invoker);
        foreach (UnityAction<float, Vector3> listener in floatVectorListeners[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void AddListener(FLOATVECTOR3EVENTS eventName, UnityAction<float, Vector3> listener)
    {
        floatVectorListeners[eventName].Add(listener);
        foreach (UnityEvent<float, Vector3> invoker in floatVectorInvokers[eventName])
        {
            invoker.AddListener(listener);
        }
    }


    public static void RemoveListener(FLOATVECTOR3EVENTS eventName, UnityAction<float, Vector3> listener)
    {
        foreach (UnityEvent<float, Vector3> invoker in floatVectorInvokers[eventName])
        {
            invoker.RemoveListener(listener);
        }
        floatVectorListeners[eventName].Remove(listener);
    }

    public static void RemoveInvoker(FLOATVECTOR3EVENTS eventName, UnityEvent<float, Vector3> invoker)
    {
        foreach (UnityAction<float, Vector3> listener in floatVectorListeners[eventName])
        {
            invoker.RemoveListener(listener);
        }
        floatVectorInvokers[eventName].Remove(invoker);
    }

    #endregion

    #region ScriptableObjects

    static Dictionary<SCRIPTABLEOBJECTSEVENTS, List<UnityEvent<ScriptableObject>>>
        ScriptableObjectInvokers = new Dictionary<SCRIPTABLEOBJECTSEVENTS, List<UnityEvent<ScriptableObject>>>();

    static Dictionary<SCRIPTABLEOBJECTSEVENTS, List<UnityAction<ScriptableObject>>>
        ScriptableObjectListeners = new Dictionary<SCRIPTABLEOBJECTSEVENTS, List<UnityAction<ScriptableObject>>>();


    public static void AddInvoker(SCRIPTABLEOBJECTSEVENTS eventName, UnityEvent<ScriptableObject> invoker)
    {

        ScriptableObjectInvokers[eventName].Add(invoker);
        foreach (UnityAction<ScriptableObject> listener in ScriptableObjectListeners[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void AddListener(SCRIPTABLEOBJECTSEVENTS eventName, UnityAction<ScriptableObject> listener)
    {
        ScriptableObjectListeners[eventName].Add(listener);
        foreach (UnityEvent<ScriptableObject> invoker in ScriptableObjectInvokers[eventName])
        {
            invoker.AddListener(listener);
        }
    }


    public static void RemoveListener(SCRIPTABLEOBJECTSEVENTS eventName, UnityAction<ScriptableObject> listener)
    {
        foreach (UnityEvent<ScriptableObject> invoker in ScriptableObjectInvokers[eventName])
        {
            invoker.RemoveListener(listener);
        }
        ScriptableObjectListeners[eventName].Remove(listener);
    }

    public static void RemoveInvoker(SCRIPTABLEOBJECTSEVENTS eventName, UnityEvent<ScriptableObject> invoker)
    {
        foreach (UnityAction<ScriptableObject> listener in ScriptableObjectListeners[eventName])
        {
            invoker.RemoveListener(listener);
        }
        ScriptableObjectInvokers[eventName].Remove(invoker);
    }


    #endregion

    #region GameObjects Events

    static Dictionary<GAMEOBJECTINTEVENTS, List<UnityEvent<GameObject, int>>>
        gameObjectIntInvokers = new Dictionary<GAMEOBJECTINTEVENTS, List<UnityEvent<GameObject, int>>>();
    static Dictionary<GAMEOBJECTINTEVENTS, List<UnityAction<GameObject, int>>>
        gameObjectIntListeners = new Dictionary<GAMEOBJECTINTEVENTS, List<UnityAction<GameObject, int>>>();



    public static void AddInvoker(GAMEOBJECTINTEVENTS eventName, UnityEvent<GameObject, int> invoker)
    {
        gameObjectIntInvokers[eventName].Add(invoker);
        foreach (UnityAction<GameObject, int> listener in gameObjectIntListeners[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void AddListener(GAMEOBJECTINTEVENTS eventName, UnityAction<GameObject, int> listener)
    {
        gameObjectIntListeners[eventName].Add(listener);
        foreach (UnityEvent<GameObject, int> invoker in gameObjectIntInvokers[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void RemoveListener(GAMEOBJECTINTEVENTS eventName, UnityAction<GameObject, int> listener)
    {
        foreach (UnityEvent<GameObject, int> invoker in gameObjectIntInvokers[eventName])
        {
            invoker.RemoveListener(listener);
        }
        gameObjectIntListeners[eventName].Remove(listener);
    }

    public static void RemoveInvoker(GAMEOBJECTINTEVENTS eventName, UnityEvent<GameObject, int> invoker)
    {
        foreach (UnityAction<GameObject, int> listener in gameObjectIntListeners[eventName])
        {
            invoker.RemoveListener(listener);
        }
        gameObjectIntInvokers[eventName].Remove(invoker);
    }


    #endregion

    #region FloatEvents

    static Dictionary<FLOATEVENTS, List<UnityEvent<float>>>
           floatInvokers = new Dictionary<FLOATEVENTS, List<UnityEvent<float>>>();
    static Dictionary<FLOATEVENTS, List<UnityAction<float>>>
        floatListeners = new Dictionary<FLOATEVENTS, List<UnityAction<float>>>();



    public static void AddInvoker(FLOATEVENTS eventName, UnityEvent<float> invoker)
    {
        floatInvokers[eventName].Add(invoker);
        foreach (UnityAction<float> listener in floatListeners[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void AddListener(FLOATEVENTS eventName, UnityAction<float> listener)
    {
        floatListeners[eventName].Add(listener);
        foreach (UnityEvent<float> invoker in floatInvokers[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void RemoveListener(FLOATEVENTS eventName, UnityAction<float> listener)
    {
        foreach (UnityEvent<float> invoker in floatInvokers[eventName])
        {
            invoker.RemoveListener(listener);
        }
        floatListeners[eventName].Remove(listener);
    }

    public static void RemoveInvoker(FLOATEVENTS eventName, UnityEvent<float> invoker)
    {
        foreach (UnityAction<float> listener in floatListeners[eventName])
        {
            invoker.RemoveListener(listener);
        }
        floatInvokers[eventName].Remove(invoker);
    }

    #endregion


    #region GameObjectEvents

    static Dictionary<GAMEOBJECTEVENTS, List<UnityEvent<GameObject>>>
            gameObjectInvokers = new Dictionary<GAMEOBJECTEVENTS, List<UnityEvent<GameObject>>>();
    static Dictionary<GAMEOBJECTEVENTS, List<UnityAction<GameObject>>>
        gameObjectListeners = new Dictionary<GAMEOBJECTEVENTS, List<UnityAction<GameObject>>>();



    public static void AddInvoker(GAMEOBJECTEVENTS eventName, UnityEvent<GameObject> invoker)
    {
        gameObjectInvokers[eventName].Add(invoker);
        foreach (UnityAction<GameObject> listener in gameObjectListeners[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void AddListener(GAMEOBJECTEVENTS eventName, UnityAction<GameObject> listener)
    {
        gameObjectListeners[eventName].Add(listener);
        foreach (UnityEvent<GameObject> invoker in gameObjectInvokers[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void RemoveListener(GAMEOBJECTEVENTS eventName, UnityAction<GameObject> listener)
    {
        foreach (UnityEvent<GameObject> invoker in gameObjectInvokers[eventName])
        {
            invoker.RemoveListener(listener);
        }
        gameObjectListeners[eventName].Remove(listener);
    }

    public static void RemoveInvoker(GAMEOBJECTEVENTS eventName, UnityEvent<GameObject> invoker)
    {
        foreach (UnityAction<GameObject> listener in gameObjectListeners[eventName])
        {
            invoker.RemoveListener(listener);
        }
        gameObjectInvokers[eventName].Remove(invoker);
    }


    #endregion

    #region AsyncOperationEvents

    static Dictionary<ASYNCOPERATIONEVENTS, List<UnityEvent<AsyncOperation>>>
        asyncOperationInvokers = new Dictionary<ASYNCOPERATIONEVENTS, List<UnityEvent<AsyncOperation>>>();
    static Dictionary<ASYNCOPERATIONEVENTS, List<UnityAction<AsyncOperation>>>
        asyncOperationListeners = new Dictionary<ASYNCOPERATIONEVENTS, List<UnityAction<AsyncOperation>>>();



    public static void AddInvoker(ASYNCOPERATIONEVENTS eventName, UnityEvent<AsyncOperation> invoker)
    {
        asyncOperationInvokers[eventName].Add(invoker);
        foreach (UnityAction<AsyncOperation> listener in asyncOperationListeners[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void AddListener(ASYNCOPERATIONEVENTS eventName, UnityAction<AsyncOperation> listener)
    {
        asyncOperationListeners[eventName].Add(listener);
        foreach (UnityEvent<AsyncOperation> invoker in asyncOperationInvokers[eventName])
        {
            invoker.AddListener(listener);
        }
    }

    public static void RemoveListener(ASYNCOPERATIONEVENTS eventName, UnityAction<AsyncOperation> listener)
    {
        foreach (UnityEvent<AsyncOperation> invoker in asyncOperationInvokers[eventName])
        {
            invoker.RemoveListener(listener);
        }
        asyncOperationListeners[eventName].Remove(listener);
    }

    public static void RemoveInvoker(ASYNCOPERATIONEVENTS eventName, UnityEvent<AsyncOperation> invoker)
    {
        foreach (UnityAction<AsyncOperation> listener in asyncOperationListeners[eventName])
        {
            invoker.RemoveListener(listener);
        }
        asyncOperationInvokers[eventName].Remove(invoker);
    }

    #endregion

    public static void Initialize()
    {
        if (!initialized)
        {

            foreach (EMPTYEVENTS name in Enum.GetValues(typeof(EMPTYEVENTS)))
            {
                emptyInvokers[name] = new List<UnityEvent>();
                emptyListeners[name] = new List<UnityAction>();
            }
            foreach (BOOLEVENTS name in Enum.GetValues(typeof(BOOLEVENTS)))
            {
                boolInvokers[name] = new List<UnityEvent<bool>>();
                boolListeners[name] = new List<UnityAction<bool>>();
            }

            foreach (FLOATINTEVENTS name in Enum.GetValues(typeof(FLOATINTEVENTS)))
            {
                floatIntInvokers[name] = new List<UnityEvent<float, int>>();
                floatIntListeners[name] = new List<UnityAction<float, int>>();
            }

            foreach (INTEVENTS name in Enum.GetValues(typeof(INTEVENTS)))
            {
                intInvokers[name] = new List<UnityEvent<int>>();
                intListeners[name] = new List<UnityAction<int>>();
            }

            foreach (FLOATVECTOR3EVENTS name in Enum.GetValues(typeof(FLOATVECTOR3EVENTS)))
            {
                floatVectorInvokers[name] = new List<UnityEvent<float, Vector3>>();
                floatVectorListeners[name] = new List<UnityAction<float, Vector3>>();
            }

            foreach (SCRIPTABLEOBJECTSEVENTS name in Enum.GetValues(typeof(SCRIPTABLEOBJECTSEVENTS)))
            {
                ScriptableObjectInvokers[name] = new List<UnityEvent<ScriptableObject>>();
                ScriptableObjectListeners[name] = new List<UnityAction<ScriptableObject>>();
            }


            foreach (GAMEOBJECTEVENTS name in Enum.GetValues(typeof(GAMEOBJECTEVENTS)))
            {
                gameObjectInvokers[name] = new List<UnityEvent<GameObject>>();
                gameObjectListeners[name] = new List<UnityAction<GameObject>>();
            }

            foreach (GAMEOBJECTINTEVENTS name in Enum.GetValues(typeof(GAMEOBJECTINTEVENTS)))
            {
                gameObjectIntInvokers[name] = new List<UnityEvent<GameObject, int>>();
                gameObjectIntListeners[name] = new List<UnityAction<GameObject, int>>();
            }


            foreach (FLOATEVENTS name in Enum.GetValues(typeof(FLOATEVENTS)))
            {
                floatInvokers[name] = new List<UnityEvent<float>>();
                floatListeners[name] = new List<UnityAction<float>>();
            }

            foreach (ASYNCOPERATIONEVENTS name in Enum.GetValues(typeof(ASYNCOPERATIONEVENTS)))
            {
                asyncOperationInvokers[name] = new List<UnityEvent<AsyncOperation>>();
                asyncOperationListeners[name] = new List<UnityAction<AsyncOperation>>();
            }

            initialized = true;
            return;
        }
        else
            return;

    }


}
