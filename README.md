# IPool


IPool is an open source [Unity Engine](http://unity3d.com/) pooling system. It's designed to be easy to use and only require a very small code change from normal Unity behaviour.

## Features
* Loads from Resources folder.
* Unlimited amount of pools.
* Implemented with a custom [LinkedList](https://en.wikipedia.org/wiki/Linked_list) to boost lookup times.
* Easy to use instantiation and reclaiming.
* Unit tested using the [Unity Test Tools](https://www.assetstore.unity3d.com/en/#!/content/13802)


## Usage [The Basics]

The API is namespaced under the `PoolSystem` and the primary class is `PoolingManager` and `PooledObject` component. IPool uses the resources folder to load all of its assets. This was done to allow for very easy conversion between existing systems and IPool. To start using the pooling system all you need to know is the path to the prefab in the resources folder. The prefab should also have a `PooledObject` component on it.


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
In the example above we are using IPool in it's simplest form. Behind the scenes IPool creates a `Pool` for you when you call `PoolManager.Instantiate`. This works in a lot of cases but has the downside of this is the pool only has a set size of one. That means that every time you ask for a new object it has to create a new one on the fly if no objects are currently inactive. Sometimes you will want to have you pool be a specific size. To do that you will create a new pool first.

```csharp
using PoolSystem

public class Gun : MonoBehaviour
{
    private Pool m_BulletPool;

    private const string BULLET_PREFAB_PATH = "Ammo\Bullet_Prefab";

    public void Awake()
    {
        m_BulletPool = PoolManager.CreatePool(BULLET_PREFAB_PATH);
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
Here is our Gun class again but this time we used `PoolManager.CreatePool(string resourcePath)`.
The advantage of using this is that the `PoolManager` does not have to search for the `Pool` every time you call the Instantiate function on it. This means one less lookup which means an increase in performance for you. It is suggested you use this pattern instead of the first one we showed.

## Usage [Intermediate]
If you are looking for more fine tuned control this is the section that you will want to read.

#### Setting the Pool size
Up until this point we have been creating pool and having their size expand when a prefab is requested and none are ready. This might work in a lot of cases but there are times when you want to have explicit control over the size. You can do this in two ways.

```csharp
using PoolSystem
public class Gun : MonoBehaviour

{

 private Pool m_BulletPool;
    public void Awake()
    {
        //We ask for a new pool and say we want the size to be 10
        m_BulletPool = PoolManager.CreatePool(BULLET_PREFAB_PATH, poolSize: 10);
        //Or
        m_BulletPool = PoolManager.CreatePool(BULLET_PREFAB_PATH);

        m_BulletPoool.SetPoolSize(10);
    }
}

```
Behind the scenes this is setting the `Pool.m_TargetPoolSize` value. This is used to check against the `Pool.m_CurrentPoolSize` each frame. If the current size is less than the target a new prefab will be added to our pool manager on that frame. This has the advantage of spacing out the Object Instantiation over multiple frames instead of all at once.