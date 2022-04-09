using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerBow : MonoBehaviour
{

    public const float SPACE_SHOT = 0.01f;

    [Header("Setting Shoot")]
    [SerializeField] private Transform _bow;
    [SerializeField] private Transform _arrow;
    [SerializeField] private Transform _shotPoint;
    [SerializeField] private float _launchForce;

    [Header("Shoot indicator")]
    [SerializeField] private Transform point;
    [SerializeField] private int numberOfPoint;
    [SerializeField] private float spaceBetweenPoint;

    [Space(10)]
    [SerializeField] private Transform bowParent;
    [SerializeField] private Transform arrowParent;


    // private
    private List<Transform> _points;
    private List<Transform> _arrowList;
    private Vector2 _direction;

    private float _spacePoint;

    private bool _isCharging = false;
    private bool _isHolding = false;


    #region UNITY
    private void Start()
    {
        Init();
    }

    private void Update()
    {
        if (!GameMgr.Instance.IsInGameState)
            return;

        if (!GameController.Instance.CanShot)
            return;

        if (!GameController.Instance.IsTransition)
            return;

        UpdateBow();
    }
    #endregion


    private void Init()
    {
        _isCharging = false;
        _points = new List<Transform>();
        _arrowList = new List<Transform>();
        _spacePoint = spaceBetweenPoint * SPACE_SHOT;

        for (int i = 0; i < numberOfPoint; i++)
        {
            var offset = i * 0.01f;
            var circle = Instantiate(point, _shotPoint.position, Quaternion.identity, bowParent);

            Vector3 scale = circle.transform.localScale;
            scale += new Vector3(scale.x - offset, scale.y - offset, scale.z);
            circle.transform.localScale = scale / 2;

            circle.name = "point_" + i.ToString();
            _points.Add(circle);
        }
    }


    private void UpdateBow()
    {
        // Vector2 bowPos = _bow.position;
        // Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // _direction = mousePos - bowPos;
        // _bow.right = _direction;

        if (Input.GetMouseButtonDown(0))
        {
            _isHolding = true;
            _direction = new Vector2(1 * Time.deltaTime, 0);
            _bow.right = _direction;

            for (int i = 0; i < numberOfPoint; i++)
            {
                _points[i].transform.position = PointPosition(i * _spacePoint);
            }

            if (!_isCharging)
            {
                ShowPointIndicator();
                _bow.DORotate(new Vector3(0, 0, 60), 2f);

                SoundMgr.PlaySFXOneShot(SoundMgr.SFX_TENSION_ARROW);
                _isCharging = true;
            }
        }


        if (Input.GetMouseButtonUp(0) && _isCharging)
        {
            Shoot();
            HidePointIndicator();

            _bow.DOKill();
            _bow.DORotate(new Vector3(0, 0, -20), .5f);
            _isCharging = false;
            // _isHolding = false;
        }
    }


    public void DecreaseSpaceShoot(int turn)
    {
        float turnSpace = spaceBetweenPoint;
        switch (turn)
        {
            case 0:
            case 1:
                turnSpace = spaceBetweenPoint; break; 
            case 2:
            case 3:
                turnSpace = spaceBetweenPoint - 1; break;
            case 4:
            case 5:
                turnSpace = spaceBetweenPoint - 2; break;
            case 6:
            case 7:
            case 8:
                turnSpace = spaceBetweenPoint - 3; break;
            case 9:
            case 10:
            case 11:
                turnSpace = spaceBetweenPoint - 4; break;

            default:
                turnSpace = 0.2f; break;
        }

        _spacePoint = turnSpace * SPACE_SHOT;
    }


    private void Shoot()
    {
        Transform arrow = Instantiate(_arrow, _shotPoint.position, _shotPoint.rotation, arrowParent);
        arrow.GetComponent<Rigidbody2D>().velocity = _bow.right * _launchForce;

        _arrowList.Add(arrow);
        SoundMgr.StopSFX(SoundMgr.SFX_TENSION_ARROW);
        SoundMgr.Instance.PlaySFX(SoundMgr.SFX_RELEASE_ARROW);
    }


    public void ShowPointIndicator()
    {
        _points.ForEach(x => x.gameObject.SetActive(true));
    }


    public void HidePointIndicator()
    {
        _points.ForEach(x => x.gameObject.SetActive(false));
    }


    private Vector2 PointPosition(float t)
    {
        return (Vector2)_shotPoint.position + (_direction.normalized * _launchForce * t) + 0.5f * Physics2D.gravity * (t * t);
    }


    public void ResetTurn()
    {
        _arrowList.ForEach(x => { if (x != null) Destroy(x.gameObject); });
        HidePointIndicator();

        _arrowList.Clear();
        _bow.DOKill();
        _isCharging = false;
        _bow.DORotate(new Vector3(0, 0, -20), .0f);
    }


    public void ResetData()
    {
        _arrowList.ForEach(x => { if (x != null) Destroy(x.gameObject); });

        _arrowList.Clear();
        _spacePoint = spaceBetweenPoint * SPACE_SHOT;
    }

}
