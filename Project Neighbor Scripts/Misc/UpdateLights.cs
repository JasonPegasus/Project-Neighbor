using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class UpdateLights : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Bake());
    }

    private IEnumerator Bake()
    {
        while(true)
        {
            GameObject plr = GameManager.utils.player;
            if (plr.GetComponent<PlayerController>().CurrSpeed == plr.GetComponent<PlayerController>().SprintSpeed)
            {yield return new WaitForSeconds(Time.deltaTime*4);}
            else
            {yield return new WaitForSeconds(Time.deltaTime);}
            if (Mathf.Abs(plr.GetComponent<CharacterController>().velocity.x + plr.GetComponent<CharacterController>().velocity.z) > 0 ||
            Mathf.Abs(Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) > 0)
            {
                GetComponent<HDAdditionalLightData>().RequestShadowMapRendering();
            }
        }
    }
}
