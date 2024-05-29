using Zenject;
using UnityEngine;

public class InjectedPrefabFactory : IFactory<GameObject, Transform, GameObject>
{
    public GameObject Create(GameObject prefab, Transform parent)
    {
        return Object.Instantiate(prefab, parent);
    }
}