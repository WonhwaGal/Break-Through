using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MVC : MonoBehaviour
{
}
public interface IView
{
}

public interface IModel
{
    void Receive();
    void Update();
}

public interface IController
{
    void AddView<T>() where T : class, IView;
}

public interface IController<V, M> : IController
    where V : class, IView
    where M : class, IModel
{
    V View { get; }
    M Model { get; }
    void UpdateView();
}