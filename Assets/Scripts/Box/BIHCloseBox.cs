using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BIHCloseBox : BoxInteractionHandler
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioClip secondClip;
    [SerializeField] private int BIHindex;
    [SerializeField] private Animator boxAnim;
    [SerializeField] private Collider boxCollider;
    [SerializeField] private UnityEvent events;
    private AudioSource boxSource;
    private void Start()
    {
        boxSource = GetComponent<AudioSource>();
    }
    public override void HandlerRequest(int index, UnityAction action)
    {
        if (index != BIHindex)
        {
            base.HandlerRequest(index, action);
        }
        else
        {
            boxAnim.SetTrigger("Close");
            PlaySound(clip);
            StartCoroutine(DisableBoxCollider(action));
        }
    }
    private IEnumerator DisableBoxCollider(UnityAction action)
    {
        yield return null;
        boxCollider.enabled = false;
        float animLenght = boxAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animLenght);
        boxAnim.SetTrigger("Exit");
        PlaySound(secondClip);
        yield return null;
        boxCollider.enabled = false;
        animLenght = boxAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animLenght);
        boxCollider.enabled = true;
        action += () => events?.Invoke();
        action.Invoke();
    }
    public void PlaySound(AudioClip clip)
    {
        boxSource.clip = clip;
        boxSource.Play();
    }
}
