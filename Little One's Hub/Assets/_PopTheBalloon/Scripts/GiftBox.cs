using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftBox : MonoBehaviour
{
    public AudioSource source;
    public ParticleSystem particle;

    private void OnMouseDown()
    {
        StartCoroutine(PlayEffect());
    }

    IEnumerator PlayEffect()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        particle.Play();
        source.Play();
        GiftManager.instance.GiftPlayer();

        yield return new WaitForSeconds(source.clip.length);

        Destroy(this.gameObject);
    }
}
