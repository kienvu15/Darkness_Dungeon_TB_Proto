using UnityEngine;

public class WeaponsVisualController : MonoBehaviour
{
    [SerializeField] private Transform[] gunTransform;

    [SerializeField] private Transform pistol;
    [SerializeField] private Transform revoler;
    [SerializeField] private Transform Rifle;
    [SerializeField] private Transform Shotgun;
    [SerializeField] private Transform Sniper;

    private void Start()
    {
        
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
    }

    private void SwitchOffGun()
    {
        for(int i = 0; i < gunTransform.Length; i++)
        {
            gunTransform[i].gameObject.SetActive(false);
        }
    }
}
