using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    //
    [SerializeField] private Transform bloodEffect;
    [SerializeField] private Transform bloodPoint;

    // private
    private Rigidbody2D _rb2d;
    private bool _hasHit;
    private GameController _gameController;


    #region UNITY
    private void Start()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _gameController = GameController.Instance;
    }

    private void Update()
    {
        UpdateArrow();
    }
    #endregion



    private void UpdateArrow()
    {
        if (_hasHit)
            return;

        float angle = Mathf.Atan2(_rb2d.velocity.y, _rb2d.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    public void HitAndStopArrow(Transform parent)
    {
        _hasHit = true;
        GetComponent<Collider2D>().enabled = false;
        transform.parent = parent.transform;

        _rb2d.velocity = Vector2.zero;
        _rb2d.isKinematic = true;
        _rb2d.freezeRotation = true;
    }


    private void CreateBloodEffect()
    {
        bloodEffect.SpawnToGarbage(bloodPoint.position, Quaternion.identity);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.tag)
        {
            case "PerfectShoot":
                print("perfect shoot");
                other.gameObject.GetComponentInParent<Fruit>()?.PlayEffectPerfectShoot();

                _gameController.AddScore(30);
                _gameController.PlayerPerfectShootEffect();
                SoundMgr.Instance.PlaySFX(SoundMgr.SFX_PERFECT_SHOOT);
                break;

            case "Tree":
                _gameController.MissingArrow();
                break;
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "ModelTarget":
                CreateBloodEffect();
                HitAndStopArrow(other.transform);
                _gameController.ShakeCameraHitTheTaget();
                other.transform.DORotate(new Vector3(0, 0, -10f), 0.25f)
                    .OnComplete(() =>
                    {
                        other.transform.DORotate(Vector3.zero, 0f);
                    });

                _gameController.MissingArrow();
                SoundMgr.Instance.PlaySFX(SoundMgr.SFX_TARGET_HIT);
                break;

            case "Fruit":
                HitAndStopArrow(other.transform);
                _gameController.HitTheTarget(true);

                other.gameObject.GetComponent<Fruit>()?.ImpactWithArrow();
                _gameController.AddScore(10);

                SoundMgr.Instance.PlaySFX(SoundMgr.SFX_FRUIT_HIT);
                _gameController.NotifyEndAction();
                break;
        }
    }





}
