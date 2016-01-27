# IPool

IPool is an open source Unity pooling system. It's designed to be easy to use and only require a very small code change from normal Unity behaviour. 


## Features
* Loads from Resources folder.
* Unlimited amount of pools. 
* Implemented with a custom LinkedList to boost lookup times. 
* Easy to use instantiation and reclaming.
* Unit tested using the [Unity Test Tools](https://www.assetstore.unity3d.com/en/#!/content/13802)

## Usage
The API is namespaced under the `PoolSystem` and the primary class is `PoolingManager` and `PooledObject` component. IPool uses the resources folder to load all of it's assets. This was done to allow for very easy conversion between exsiting systems and IPool. To start using the pooling system all you need to know is the path to the prefab in the resources folder. The prefab should also have a `PooledObject` component on it. 

``` csharp
using PoolSystem

public class Gun : MonoBehaviour
{
	private const string m_BulletPrefabPath = "Ammo\Bullet_Prefab";
    
    /// <summary>
    /// Grabs an instance of a Bullet Object from the PoolManager and
    /// fires it.
    /// </summary>
    public void FireGun()
    {
		GameObject bullet = PoolManager.Instantiate( resourcePath : m_BulletPrefabPath, 
        				                             position : this.transform.position,
                                                     rotation : this.transform.rotation );
                                 
    }
}
```

The only thing that is missing in this example is the point at which the bullet will be returned to the queue. To do this you need to have a `PooledObject` component at the root of the prefab.

``` csharp
using PoolSystem
[RequiredComponent(typeof(PooledObject))]
public class Bullet : MonoBehaviour
{
	private PooledObject m_PooledObject;
    
    public PooledObject pooledObject
    {
    	get 
        {
        	if(m_PooledObject == null)
            {
				m_PooledObject = this.GetComponent<PooledObject>();
            }
            return m_PooledObject;
        }
	}
    
    /// <summary>
    /// When the bullet hits it's target this function will be called. Thi
    /// </summary>
    public void OnTargetHit()
    {
    	// The object will be disabled and be available next time the pool
        // manager is asked for a prefab. 
		pooledObject.Deallocate();
    }
}
```

// Note: This project is still in active development and will have more documention as progress is made.