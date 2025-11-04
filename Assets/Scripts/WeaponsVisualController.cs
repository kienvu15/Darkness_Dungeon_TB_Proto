using UnityEngine;

public class WeaponsVisualController : MonoBehaviour
{
    [SerializeField] private Transform[] gunTransform;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revoler;
    [SerializeField] private Transform Rifle;
    [SerializeField] private Transform Shotgun;
    [SerializeField] private Transform Sniper;

    private Transform currentGun;

    [Header("LeftHand")]
    [SerializeField] private Transform leftHand;

    private void Start()
    {
        SwitchOn(pistol);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchOn(Sniper);
        }
    }

    private void SwitchOn(Transform gunTransform)
    {
        SwitchOffGun();
        gunTransform.gameObject.SetActive(true);
        currentGun = gunTransform;

        AttackLeftHand();
    }

    private void SwitchOffGun()
    {
        for(int i = 0; i < gunTransform.Length; i++)
        {
            gunTransform[i].gameObject.SetActive(false);
        }
    }

    private void AttackLeftHand()
    {
        Transform targetTransform = currentGun.GetComponentInChildren<LeftHandTransform>().transform;
        leftHand.localPosition = targetTransform.localPosition;
        leftHand.localRotation = targetTransform.localRotation;
    }
}
