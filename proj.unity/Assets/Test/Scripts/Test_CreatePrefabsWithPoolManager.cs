using PoolSystem;
using System.Collections;
using UnityEngine;

public class Test_CreatePrefabsWithPoolManager : MonoBehaviour
{

  private GameObject m_Prefab;
  private Pool m_Pool;

  public void Start()
  {
    StartCoroutine(TestRoutine());
  }

  private IEnumerator TestRoutine()
  {

    for (int i = 0; i < 10; i++)
    {
      GameObject go = PoolManager.Instantiate(PrefabPaths.CUBE_PATH);

      PooledObject pooled = go.GetComponent<PooledObject>();

      go.transform.SetParent(transform);

      yield return new WaitForSeconds(0.5f);
    }

    if (transform.childCount < 5)
    {
  
      //We should just reuse the same object
      IntegrationTest.Pass();
    }
    else
    {
      IntegrationTest.Fail(transform.gameObject, "Too many children were spawned. There should only be one.");
    }
  }
}
