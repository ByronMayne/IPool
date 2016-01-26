using TinyJSON;

namespace UnityEngine
{
  public struct Vector3
  { }

  public struct Quaternion
  {

  }

  public class Object
  {
    public static Object Instantiate(Object original, Vector3 position, Quaternion rotation)
    {
      return Instantiate<Object>(original);
    }

    public static Object Instantiate(Object original)
    {
      return Instantiate<Object>(original);
    }

    public static T Instantiate<T>(Object original) where T : Object
    {
      //To do a deep copy the object quickly I just use TinyJson.
      string json = JSON.Dump(original);

      return JSON.Load(json).Make<T>();
    }
  }
}