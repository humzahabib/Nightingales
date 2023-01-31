using UnityEngine;
using UnityEngine.Events;

public class FloatEvent : UnityEvent<float>
{ }


public class EmptyEvent : UnityEvent
{ }

public class BoolEvent : UnityEvent<bool>
{ }

public class IntEvent : UnityEvent<int>
{ }


public class FloatIntEvent : UnityEvent<float, int>
{ }

public class FloatVectorEvent : UnityEvent<float, Vector3>
{ }

public class ScriptableObjectEvent : UnityEvent<ScriptableObject>
{ }

public class GameObjectEvent : UnityEvent<GameObject>
{ }

public class GameObjectIntEvent : UnityEvent<GameObject, int>
{ }

public class AsyncOperationEvent : UnityEvent<AsyncOperation>
{ }


