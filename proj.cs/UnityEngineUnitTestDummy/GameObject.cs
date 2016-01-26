using System;
using System.Collections.Generic;
using TinyJSON;

namespace UnityEngine
{
  public class GameObject : Object
  {
    private string m_Name; 
    private List<Component> m_Components;

    public GameObject()
    {
      m_Components = new List<Component>();
    }

    public GameObject(string name)
    {
      m_Name = name;
      m_Components = new List<Component>();
    }

    public string name
    {
      get
      {
        return m_Name;
      }
      set
      {
        m_Name = value;
      }
    }


    public T AddComponent<T>() where T : Component
    {
      T comp = System.Activator.CreateInstance<T>();
      comp.gameObject = this;
      m_Components.Add(comp);
      return comp;
    }

    public Component GetComponent(Type type)
    {
      foreach(Component component in m_Components)
      {
        if(component.GetType() == type)
        {
          return component;
        }
      }
      return null;
    }

    public Component GetComponent<T>()
    {
      foreach (Component component in m_Components)
      {
        if (component.GetType() == typeof(T))
        {
          return component;
        }
      }
      return null;
    }

    public static GameObject Instantiate(GameObject original)
    {
      //To do a deep copy the object quickly I just use TinyJson.
      string json = JSON.Dump(original);

      return JSON.Load(json).Make<GameObject>();
    }
  }
}
