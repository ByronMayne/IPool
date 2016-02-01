using PoolSystem;
using System.Collections;
using UnityEngine;

public class Test_CreatePrefabsWithPoolManager : MonoBehaviour
{
  private const string PREFAB_PATH = "Prefabs/Cube [Auto Deallocate]";
  private GameObject m_Prefab;
  private Pool m_Pool;
  private Transform m_Parent;

  public void Start()
  {
    m_Parent = new GameObject("Deallocate Parent").transform;
    StartCoroutine(TestRoutine());
  }

  private IEnumerator TestRoutine()
  {

    for (int i = 0; i < 10; i++)
    {
      GameObject go = PoolManager.Instantiate(PREFAB_PATH);

      PooledObject pooled = go.GetComponent<PooledObject>();

      //go.transform.SetParent(m_Parent);

      yield return new WaitForSeconds(0.5f);
    }
    Debug.Log(m_Parent.childCount);
    if (m_Parent.childCount < 5)
    {
  
      //We should just reuse the same object
      IntegrationTest.Pass();
    }
    else
    {
      IntegrationTest.Fail(m_Parent.gameObject, "Too many children were spawned. There should only be one.");
    }
  }
}
