namespace UnityEngine
{
  public class Component : Object
  {
    private GameObject m_GameObject;

    public GameObject gameObject
    {
      get { return m_GameObject; }
      internal set { m_GameObject = value; }
    }
  }
}