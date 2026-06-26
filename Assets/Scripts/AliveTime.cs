using UnityEngine;

public class AliveTime : MonoBehaviour
{
    [SerializeField] private float timeToDestroy;

    private void Awake()
    {
        Destroy(gameObject, timeToDestroy);
    }
}
