using UnityEngine;
using System.Collections;

public class Shield : MonoBehaviour {
    public float ShotPowerReduction;
    public float ChargeShieldAmount;
    public GameObject ShieldObject;
    public float ShieldExpandedScale;
    public float ShieldExpandTime;
    public float ShieldDeployLength;
    public float ShieldRechargeTime;

    MouseInput mouseInput;
    GameController gameController;
    GUIController guiControler;

    float shieldPower;
    public float Power
    {
        get { return shieldPower; }
    }

    float shieldCharge;

    public void ReduceShieldFromShot()
    {
        shieldPower = Mathf.Clamp(shieldPower - ShotPowerReduction, 0f, 100f);
    }

    public void DamageShield(float damage)
    {
        shieldPower -= damage;
        if (shieldPower < 0)
            die();
    }

    public void ChargeShield()
    {
        shieldPower = Mathf.Clamp(shieldPower + ChargeShieldAmount, 0f, 100f);
    }

	void Start () 
    {
        mouseInput = GetComponent<MouseInput>();

        shieldPower = 100;
        ShieldObject.transform.localScale = Vector3.zero;
        ShieldObject.SetActive(false);

        shieldCharge = 100;

        gameController = GameObject.FindGameObjectWithTag(Tags.GameController).GetComponent<GameController>();
        guiControler = GameObject.FindGameObjectWithTag(Tags.GUI).GetComponent<GUIController>();
	}

    void Update()
    {
        guiControler.PowerGuage.value = shieldPower / 100.0f;
        guiControler.AbsorbtionGuage.value = shieldCharge / 100.0f;
    }

    void FixedUpdate()
    {
        if (mouseInput.RightMouseButton && !ShieldObject.activeInHierarchy && Mathf.Abs(shieldCharge - 100) < 0.001)
            DeployShield();
        if (!mouseInput.RightMouseButton && ShieldObject.activeInHierarchy)
            RetractShield();
    }

    void die()
    {
        guiControler.PowerGuage.value = shieldPower / 100.0f;
        guiControler.AbsorbtionGuage.value = shieldCharge / 100.0f;
        gameObject.SetActive(false);
    }

    void DeployShield()
    {
        ShieldObject.SetActive(true);
        Go.to(ShieldObject.transform, ShieldExpandTime, new GoTweenConfig().scale(ShieldExpandedScale));
        gameController.ShieldDeployed = true;
        StartCoroutine("decreaseShieldCharge");
    }

    void RetractShield()
    {
        StopCoroutine("decreaseShieldCharge");
        gameController.ShieldDeployed = false;
        Go.to(ShieldObject.transform, ShieldExpandTime, new GoTweenConfig().scale(0).onComplete(f => { ShieldObject.SetActive(false); StartCoroutine("shieldRechargePause"); }));
    }

    IEnumerator decreaseShieldCharge()
    {
        float counter = 0;
        while (counter < ShieldDeployLength)
        {
            counter += Time.deltaTime;
            shieldCharge = (1 - (counter / ShieldDeployLength)) * 100f;
            yield return null;
        }
        if (mouseInput.RightMouseButton)
            RetractShield();
    }

    IEnumerator shieldRechargePause()
    {
        yield return new WaitForSeconds(1f);
        StartCoroutine("rechargeShield");
    }

    IEnumerator rechargeShield()
    {
        float counter = shieldCharge / 100f * ShieldRechargeTime;
        while (counter < ShieldRechargeTime)
        {
            counter += Time.deltaTime;
            shieldCharge = (counter / ShieldRechargeTime) * 100f;
            yield return null;
        }
        shieldCharge = 100;
    }
}
