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
	private const string BULLET_PREFAB_PATH = "Ammo\Bullet_Prefab";
    
    /// <summary>
    /// Grabs an instance of a Bullet Object from the PoolManager and
    /// fires it.
    /// </summary>
    public void FireGun()
    {
		GameObject bullet = PoolManager.Instantiate( resourcePath : BULLET_PREFAB_PATH, 
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

In the example above we are using IPool in it's simplest form. Behind the scenes IPool creates a `Pool` for you when you call `PoolManager.Instantiate`. This works in a lot of cases but has the downside of this is the pool only has a set size of one. That means that everytime you ask for a new object it has to create a new one on the fly if no objects are currently inactive. Sometimes you will want to have you pool be a specific size. To do that you will create a new pool first. 

```csharp
using PoolSystem

public class Gun : MonoBehaviour
{
	private Pool m_BulletPool; 
	private const string BULLET_PREFAB_PATH = "Ammo\Bullet_Prefab";
    
    public void Awake()
    {
    	m_BulletPool = PoolManager.CreatePool(BULLET_PREFAB_PATH, poolSize: 10);
    }
    
    /// <summary>
    /// Grabs an instance of a Bullet Object from the PoolManager and
    /// fires it.
    /// </summary>
    public void FireGun()
    {
		GameObject bullet = m_BulletPool.Instantiate( position : this.transform.position,
                                                      rotation : this.transform.rotation );
                                 
    }
}
```
Here is our Gun class again but this time we used `PoolManager.CreatePool(string resourcePath)`. This gives us two advantages here. First we can set our `poolSize`. This tells the pool when it is created to grow to that size at a minimum. Instead of creating all of them at once it will create the prefabs over the course of several frames. 

The other advantage is that the `PoolManager` does not have to search for the `Pool` everytime you call the Instantiate function on it. This means one less lookup which means an increase in preformace for you. It is suggested you use this pattern instead of the first one we showed. 

