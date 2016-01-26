using UnityEngine;
using PoolSystem;
using System.Collections;

[IntegrationTest.DynamicTest("Scenes/IPoolUnitTests.unity")]
public class Test_PoolCreation : MonoBehaviour
{
  private const string m_PrefabResoucePath = "Prefabs/Sphere";
  private Pool m_Pool;
  private GameObject m_Prefab;
  private Transform m_Parent; 

  public void Awake()
  {
    m_Prefab = Resources.Load<GameObject>(m_PrefabResoucePath);

    m_Parent = new GameObject("Test_Spawning").transform;

    m_Pool = new Pool(m_Prefab);

    StartCoroutine(TestRoutine());
  }


  private IEnumerator TestRoutine()
  {
    for (int i = 0; i < 10; i++)
    {
      GameObject go = m_Pool.GetNextAvaiable();

      go.transform.SetParent(m_Parent);

      print(go.activeSelf);

      PooledObject pooled = go.GetComponent<PooledObject>();

      yield return new WaitForSeconds(0.05f);

      pooled.Deallocate();

      yield return new WaitForSeconds(0.05f);
    }

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
