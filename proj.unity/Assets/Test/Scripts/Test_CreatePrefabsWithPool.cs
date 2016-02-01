using UnityEngine;
using PoolSystem;
using System.Collections;

[IntegrationTest.DynamicTest("Scenes/IPoolUnitTests.unity")]
public class Test_CreatePrefabsWithPool : MonoBehaviour
{
  private const string m_PrefabResoucePath = "Prefabs/Sphere";
  private Pool m_Pool;
  private Transform m_Parent; 

  public void Awake()
  {
    m_Parent = new GameObject("Test_Spawning").transform;

    m_Pool = new Pool(m_PrefabResoucePath);

    StartCoroutine(TestRoutine());
  }


  private IEnumerator TestRoutine()
  {
    for (int i = 0; i < 10; i++)
    {
      GameObject go = m_Pool.Instantiate();

      go.transform.SetParent(m_Parent);

      PooledObject pooled = go.GetComponent<PooledObject>();

      yield return new WaitForSeconds(0.05f);

      pooled.Deallocate();

      yield return new WaitForSeconds(0.05f);
    }

    yield return new WaitForSeconds(0.5f);

    if(m_Parent.childCount < 2)
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
