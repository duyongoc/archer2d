using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Fruit : MonoBehaviour
{

    // inspector
    [SerializeField] private Transform effectFruit;

    // private
    private Rigidbody2D _rb2d;
    private Vector3 _originPosition;
    private Quaternion _originRotation;


    #region UNITY
    private void Start()
    {
        // Init();
    }

    // private void Update()
    // {
    // }
    #endregion


    public void Init()
    {
        _rb2d = GetComponent<Rigidbody2D>();
        _originPosition = transform.localPosition;
        _originRotation = transform.localRotation;

        Hide();
    }


    public void Show()
    {
        ResetData();
        gameObject.SetActive(true);
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }


    public void PlayEffectPerfectShoot()
    {
        effectFruit.SpawnToGarbage(transform.position, Quaternion.identity);
    }


    public void ImpactWithArrow()
    {
        _rb2d.gravityScale = 1;
        _rb2d.isKinematic = false;

        var angle = Random.Range(-360, -500);
        transform.DORotate(new Vector3(0, 0, angle), 1);
        _rb2d.AddForce(Vector2.right * 10, ForceMode2D.Impulse);

        DG.Tweening.DOVirtual.DelayedCall(0.5f, () =>
        {
            Hide();
            ResetData();
        });
    }


    public void ResetData()
    {
        _rb2d.gravityScale = 0;
        _rb2d.isKinematic = true;
        _rb2d.velocity = Vector2.zero;

        transform.localPosition = _originPosition;
        transform.localRotation = _originRotation;

        // remove all arrows in this fruit
        foreach (Transform child in transform)
        {
            if (child.name.Contains("Arrow"))
                Destroy(child.gameObject);
        }
    }


}
