using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CustomInjectedPrefabFactory : IFactory<GameObject, Transform, GameObject>
{
    private readonly DiContainer _dicontainer;

    public CustomInjectedPrefabFactory(DiContainer dicontainer)
    {
        _dicontainer = dicontainer;
    }

    public GameObject Create(GameObject gameObject, Transform parent) =>
        _dicontainer.InstantiatePrefab(gameObject, parent);

}
