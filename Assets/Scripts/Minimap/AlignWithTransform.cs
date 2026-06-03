using UnityEngine;

public class AlignWithTransform : MonoBehaviour
{

    private Transform _target;
    [SerializeField] private Vector3 _rotationOffset;

    private void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(_rotationOffset.x, _rotationOffset.y + _target.eulerAngles.y, _rotationOffset.z);
    }

}
