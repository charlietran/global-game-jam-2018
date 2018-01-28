﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTwoBehavior : MonoBehaviour
{
    public float sensitivity;
    public float minX;
    public float maxX;

    public Light hunterLight;

    public float minZ;
    public float maxZ;

    private bool touchingPlayer = false;
    private bool touchingComputer = false;

    public float lightStartIntensity = 2f;
    public float lightStartRange = 24f;
    public float lightStartAngle = 15.7f;
    public Color32 lightStartColor = new Color32(255, 139, 139, 255);

    public float lightEndIntensity = 4.5f;
    public float lightEndRange = 24f;
    public float lightEndAngle = 10.8f;
    public Color32 lightEndColor = new Color32(255, 0, 0, 255);
    public float lightSpeed = 0.8f;

    public float lightProgress = 0f;

    public AudioClip failureSound;

    float timeOfLastHumanCheck = 0f;
    float intervalHumanCheck = 5f;

    bool humanChecking = false;

    public void Start()
    {
        maxX = Game.Instance.map.maxX;
        minX = Game.Instance.map.minX;
        maxZ = Game.Instance.map.maxZ;
        minZ = Game.Instance.map.minZ;
        SetupLight();
    }

    void SetupLight()
    {
        hunterLight.intensity = lightStartIntensity;
        hunterLight.range = lightStartRange;
        hunterLight.spotAngle = lightStartAngle;
        hunterLight.color = lightStartColor;
    }

    Vector3 GetMovement()
    {
        Vector3 pos = transform.position;

        float moveHorizontal = Input.GetAxis("Hunter Horizontal Joystick") * sensitivity;
        float moveVertical = Input.GetAxis("Hunter Vertical Joystick") * sensitivity;

        pos.x = Mathf.Clamp(transform.position.x + moveHorizontal, minX, maxX);
        pos.z = Mathf.Clamp(transform.position.z + moveVertical, minZ, maxZ);

        return pos;
    }

    void CheckForHuman()
    {
        if (touchingPlayer)
        {
            Game.Instance.HunterWin();
        }
        else if (touchingComputer)
        {
            Failure();
            Game.Instance.AddHunterFailure();
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            touchingPlayer = true;
        }

        if (other.tag == "Computer")
        {
            touchingComputer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            touchingPlayer = false;
        }

        if (other.tag == "Computer")
        {
            touchingComputer = false;
        }
    }

    public void Failure()
    {
        Camera.main.GetComponent<CameraBehavior>().Shake();
        // Sound effect
        Music.Instance.PlaySFX(failureSound);
    }

    void Update()
    {
<<<<<<< HEAD
      if (Input.GetKey("joystick 2 button " + i))
      {
        expanded = true;
        if (lightProgress >= 1f && (Time.time >= timeOfLastHumanCheck + intervalHumanCheck))
=======
        transform.position = GetMovement();
        hunterLight.transform.LookAt(transform.position);

        bool expanded = false;
        for (int i = 0; i < 20; i++)
>>>>>>> 792e941537500a273cef0ca79098d36e9a0f877a
        {
            if (Input.GetKey("joystick 2 button " + i))
            {
                expanded = true;
                if (lightProgress >= 1f && !humanChecking)
                {
                    humanChecking = true;
                    CheckForHuman();
                }
                else
                {
                    SetLightProgress(1);
                }
            }
        }
        if (!expanded)
        {
            if (lightProgress > 0)
                SetLightProgress(-1);
        }
    }

    void SetLightProgress(int multiplier)
    {
        lightProgress += multiplier * Time.deltaTime * lightSpeed;
        lightProgress = Mathf.Clamp(lightProgress, 0, 1);
        if(humanChecking && lightProgress ==0) {
            humanChecking = false;
        }
        hunterLight.intensity = Mathf.Lerp(lightStartIntensity, lightEndIntensity, lightProgress);
        hunterLight.range = Mathf.Lerp(lightStartRange, lightEndRange, lightProgress);
        hunterLight.spotAngle = Mathf.Lerp(lightStartAngle, lightEndAngle, lightProgress);
        hunterLight.color = Color.Lerp(lightStartColor, lightEndColor, lightProgress);
    }

    IEnumerator ResetLight()
    {
        while (lightProgress > 0)
        {
            lightProgress -= Time.deltaTime * lightSpeed;
            lightProgress = Mathf.Clamp(lightProgress, 0, 1);

            hunterLight.intensity = Mathf.Lerp(lightStartIntensity, lightEndIntensity, lightProgress);
            hunterLight.range = Mathf.Lerp(lightStartRange, lightEndRange, lightProgress);
            hunterLight.spotAngle = Mathf.Lerp(lightStartAngle, lightEndAngle, lightProgress);
            hunterLight.color = Color.Lerp(lightStartColor, lightEndColor, lightProgress);
            yield return null;
        }
    }
}