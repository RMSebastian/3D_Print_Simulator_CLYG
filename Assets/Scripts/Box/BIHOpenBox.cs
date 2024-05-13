using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BIHOpenBox : BoxInteractionHandler
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private int BIHindex;
    [SerializeField] private Animator boxAnim;
    [SerializeField] private Collider boxCollider;
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
            boxAnim.SetTrigger("Open");
            PlaySound();
            StartCoroutine(DisableBoxCollider());
        }
    }
    private IEnumerator DisableBoxCollider()
    {
        yield return null;
        boxCollider.enabled = false;
        float animLenght = boxAnim.GetCurrentAnimatorClipInfo(0)[0].clip.length;
        yield return new WaitForSeconds(animLenght);
        boxCollider.enabled = true;
    }
    public void PlaySound()
    {
        boxSource.clip = clip;
        boxSource.Play();
    }
}
