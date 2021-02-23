using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedKit : MonoBehaviour
{
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _speedDown;
    private Player _player;

    private void Update()
    {
        MoveDown();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            _player = other.gameObject.GetComponent<Player>();


            if (_player._isPlayerOne == true)
            {
                _player.AddHealthPLayerOne();
            }
            else if (_player._isPlayerTwo == true)
            {
                _player.AddHealthPlayerTwo();
            }
            AudioSource.PlayClipAtPoint(_audioClip, new Vector3(0,0,0), 1.0f);
            Destroy(this.gameObject);
        }
    }

    void MoveDown()
    {
        transform.Translate(Vector3.down * _speedDown * Time.deltaTime);

        if (transform.position.y < -10f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

}
