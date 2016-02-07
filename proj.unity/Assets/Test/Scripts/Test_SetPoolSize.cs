using UnityEngine;
using System.Collections;
using PoolSystem;
using UnityEngine.Assertions;

public class Test_SetPoolSize : MonoBehaviour
{
  private Pool m_Pool;
  [SerializeField]
  private int m_PoolCount = 10;

  public void OnEnable()
  {
    m_Pool = PoolManager.CreatePool(PrefabPaths.SPHERE_PATH, poolSize: m_PoolCount, parent: transform);
    StartCoroutine(TestQueueInit());
  }

  private IEnumerator TestQueueInit()
  {

    yield return new WaitForSeconds(2f);

    if(transform.childCount == m_PoolCount)
    {
      IntegrationTest.Pass();
    }
    else
    {
      IntegrationTest.Fail(gameObject, string.Format("The pool was initialized with a size of {0} but {1} children were created", m_PoolCount, transform.childCount));
    }
  }
}
