using PoolSystem;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PooledObject))]
public class AutoDeallocate : MonoBehaviour
{
  [SerializeField]
  private float m_Time;
  [SerializeField]
  private PooledObject m_PooledObject;

  public float time
  {
    get { return m_Time; }
    set { m_Time = value; }
  }

  public PooledObject pooledObject
  {
    set { m_PooledObject = value; }
    get
    {
      if(m_PooledObject == null)
      {
        m_PooledObject = this.GetComponent<PooledObject>();
      }
      return m_PooledObject;
    }
  }


  public void OnEnable()
  {
    StartCoroutine(Deallocate());
  }

  private IEnumerator Deallocate()
  {
    yield return new WaitForSeconds(m_Time);
    pooledObject.Deallocate();

  }
}
