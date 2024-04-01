using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;
using Rnd = UnityEngine.Random;

public class YellowFaceScript : MonoBehaviour {

    static int _moduleIdCounter = 1;
    int _moduleID = 0;

    public KMNeedyModule Module;
    public KMBombInfo Bomb;
    public KMAudio Audio;
    public SpriteRenderer YellowFace;
    public Sprite[] YellowFaceSprites;

    private Coroutine YellowFaceTalkCoroutine, YellowFaceMoveCoroutine;
    private Vector3 InitPos;
    private KMAudio.KMAudioRef Sound;

    void Awake()
    {
        _moduleID = _moduleIdCounter++;
        Bomb.OnBombExploded += delegate { if (Sound != null) Sound.StopSound(); };
        Bomb.OnBombSolved += delegate { if (Sound != null) Sound.StopSound(); };
        Module.OnNeedyActivation += delegate { HandleActivation(); };
        Module.OnTimerExpired += delegate { HandleDeactivation(); };
        Module.OnNeedyDeactivation += delegate { HandleDeactivation(); };
        InitPos = YellowFace.transform.localPosition;
    }

    void HandleActivation()
    {
        if (Sound != null)
            Sound.StopSound();
        Sound = Audio.PlaySoundAtTransformWithRef("audio", transform);
        YellowFaceTalkCoroutine = StartCoroutine(YellowFaceTalk());
        YellowFaceMoveCoroutine = StartCoroutine(YellowFaceMove());
    }

    void HandleDeactivation()
    {
        if (Sound != null)
            Sound.StopSound();
        StopCoroutine(YellowFaceTalkCoroutine);
        StopCoroutine(YellowFaceMoveCoroutine);
        YellowFace.sprite = YellowFaceSprites[0];
        YellowFace.transform.localPosition = InitPos;
    }

    private IEnumerator YellowFaceTalk()
    {
        while (true)
        {
            YellowFace.sprite = YellowFaceSprites[0];
            yield return new WaitForSeconds(1 / 8f);
            YellowFace.sprite = YellowFaceSprites[1];
            yield return new WaitForSeconds(1 / 24f);
            YellowFace.sprite = YellowFaceSprites[2];
            yield return new WaitForSeconds(1 / 24f);
            YellowFace.sprite = YellowFaceSprites[1];
            yield return new WaitForSeconds(1 / 24f);
        }
    }

    private IEnumerator YellowFaceMove(float variance = 0.005f)
    {
        while (true)
        {
            YellowFace.transform.localPosition = InitPos + new Vector3(Rnd.Range(-variance, variance), 0, Rnd.Range(-variance, variance));
            yield return null;
        }
    }

#pragma warning disable 414
    private string TwitchHelpMessage = "This module has no commands, but thanks for trying!";
#pragma warning restore 414

    IEnumerator ProcessTwitchCommand(string command)
    {
        yield return null;
    }
}
