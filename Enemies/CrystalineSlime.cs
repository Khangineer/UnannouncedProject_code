
using Assets.Code.Interfaces;
using Assets.Scripts;
using UnityEditor;
using UnityEngine;

public class CrystalineSlime : MonoBehaviour, IMob
{
    [SerializeField] private GameObject _hitPrefab;
    [SerializeField] private EnemyHealthbarController _enemyHealthbarController;
    [SerializeField] private CrystalineSlimeSO _crystalineSlimeSO;
    [SerializeField] private CrystalinePathSO _crystalinePathSO;


    private static Transform _playerTransform;

    private Animator _animator;

    public float HP { get; set; }
    public Transform Transform { get { return gameObject.transform; } } 


    void Start()
    {
        this.HP = _crystalineSlimeSO.GetHP();
        _enemyHealthbarController.Sethealth(HP, HP);
        _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        Movement();

        if (transform.position.x >= _playerTransform.position.x)
        {
            _animator.SetInteger("directionToLook", 1);
        }
        else
        {
            _animator.SetInteger("directionToLook", 0);
        }

        if (Vector3.Distance(transform.position, _playerTransform.position) < 1f)
        {
            _crystalinePathSO.RemoveEnemyFromList(this);
            OnDeath();
        }

        if(this.HP <= 0)
        {
            _crystalinePathSO.RemoveEnemyFromList(this);
            OnDeath();
        }
    }

    public void LooseHP(float hp)
    {
        this.HP -= hp;
        _enemyHealthbarController.Sethealth(HP, _crystalineSlimeSO.GetHP());
        ObjectPoolManager.SpawnObject(_hitPrefab, gameObject.transform.position, Quaternion.identity, ObjectPoolManager.PoolType.VFXs);
    }

    private void OnDeath()
    {
        this.HP = _crystalineSlimeSO.GetHP();
        _enemyHealthbarController.Sethealth(HP, HP);

        if (transform.position.x >= _playerTransform.position.x)
        {
            _animator.SetInteger("state", 1);
        }
        else
        {
            _animator.SetInteger("state", 0);
        }
    }

    private void Movement()
    {
        if (this.HP > 0)
        {
            Vector3 moveDirNormalized = (_playerTransform.position - transform.position).normalized;
            transform.position += moveDirNormalized * _crystalineSlimeSO.GetMovSpeed() * Time.deltaTime;
        }
        else
        {
            transform.position += new Vector3(0, 0, 0);
        }
    }

    public void RestoreHP(float hp)
    {
        this.HP += hp;
    }

    public void SpecialAction()
    {
        throw new System.NotImplementedException();
    }

    public void MoveTo(Vector3 position, float moveSpeed)
    {
        throw new System.NotImplementedException();
    }


}
