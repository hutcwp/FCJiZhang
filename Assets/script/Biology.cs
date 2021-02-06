using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biology : Robot
{
	[SerializeField] private UnityEngine.UI.Image _HPImage, HPImage;
	[SerializeField] private SpriteRenderer SpriteRenderer;
	[SerializeField] private Animator Animator;
	[SerializeField] private float shakeDuration, shakeAmount;

	private float _shakeDuration;
	private UnityEngine.Coroutine ShakeCoroutine;
	private Material _Material;
	private bool isShaking;

	private float curHealth = 1f;

	private Robot robot;

	[SerializeField] private  GameObject menu;

	// Start is called before the first frame update
	protected override void Start()
	{
		robot = this;
		SpriteRenderer.material = Instantiate(SpriteRenderer.material);
		menu.SetActive(false);
	}

	// Update is called once per frame
	void Update()
	{
        if (robot.healthRate() != curHealth)
        {
			curHealth = robot.healthRate();
			Hit();
        }
	}


    public void Hit()
	{
		HitFlash();
		//Shake();
	}

	private void HitFlash()
	{
		//fixme: 數字 1 應該改為自動讀取 HitFlash 這個動畫的總時間長
		//Animator.SetFloat("FlashDuration", 1 / shakeDuration);
		//Animator.SetTrigger("Hit");
		HPImage.fillAmount = curHealth;
		HpAnimation();
		if (HPImage.fillAmount <= 0) ReHpAnimation();

	}

	public void Shake()
	{
		if (isShaking == true) _shakeDuration += shakeDuration;
		if (isShaking == false) ShakeCoroutine = StartCoroutine(IEnumeratorShake());
	}

	private IEnumerator coroutine;
	public void HpAnimation()
	{
		if (coroutine != null) StopCoroutine(coroutine);
		coroutine = IEnumeratorHpAnimation(HPImage.fillAmount, _HPImage);
		StartCoroutine(coroutine);
	}

	public void ReHpAnimation()
	{
		_HPImage.fillAmount = 1;
		if (coroutine != null) StopCoroutine(coroutine);
		coroutine = IEnumeratorHpAnimation(1, HPImage);
		StartCoroutine(coroutine);
	}


	IEnumerator IEnumeratorHpAnimation(float targetValue, UnityEngine.UI.Image Image)
	{

		float _BarHPDuration = 0;
		float BarHPDuration = 1;
		while (_BarHPDuration <= BarHPDuration)
		{
			_BarHPDuration += Time.deltaTime;
			float _BarHP = Image.fillAmount;
			Image.fillAmount = Mathf.Lerp(_BarHP, targetValue, _BarHPDuration / BarHPDuration);
			yield return null;
		}

	}


	IEnumerator IEnumeratorShake()
	{
		_shakeDuration = shakeDuration;
		isShaking = true;
		while (_shakeDuration > 0.01f)
		{
			Vector3 rotationAmount = UnityEngine.Random.insideUnitCircle * shakeAmount;//A Vector3 to add to the Local Rotation
			rotationAmount.z = 0;
			_shakeDuration -= Time.deltaTime;
			SpriteRenderer.transform.localPosition = rotationAmount;
			yield return null;
		}
		SpriteRenderer.transform.localPosition = Vector3.zero;
		isShaking = false;
	}
}
