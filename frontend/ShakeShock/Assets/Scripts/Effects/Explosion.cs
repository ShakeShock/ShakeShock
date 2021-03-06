// --------------------------------------------------------------
// Shake Shock - Explosion                              3/05/2022
// Author(s): Cameron Carstens
// Contact: bitzoic.eth@gmail.com
// --------------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{

    #region Inspector Fields

    [Header("Dependencies")]
    [SerializeField]
    private Transform explosionTransform;
    [SerializeField]
    private CircleCollider2D circleCollider2;
    [SerializeField]
    private SpriteRenderer[] shockWaves;
    [SerializeField]
    private ParticleSystem particleSystem;

    [Header("Settings")]
    [SerializeField]
    private float expansionRate;
    [SerializeField]
    private float explosionTime;
    [SerializeField]
    private float cameraShakeIntensity;
    [SerializeField]
    private float cameraShakeLength;

    #endregion

    #region Run-Time Fields

    private float explosionSize;
    private float damage;
    private GameObject playerGameObject;
    private List<GameObject> hitPlayers;

    #endregion

    #region Monobehaviors

    private void Awake()
    {
        explosionTransform.localScale = new Vector3(0, 0, 0);
        hitPlayers = new List<GameObject>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DisableExplosion());
        CameraShake.main.ShakeCamera(cameraShakeIntensity, cameraShakeLength);
    }

    // Update is called once per frame
    void Update()
    {
        Expand();
        WaitForParticles();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && playerGameObject != collision.gameObject)
        {
            if (!hitPlayers.Contains(collision.gameObject))
            {
                collision.gameObject.GetComponent<Player>().GetPlayerHealth().TakeDamage(damage);
                hitPlayers.Add(collision.gameObject);
            }
        }
    }

    #endregion

    #region Public Methods

    public void SetExplosionSize(float size)
    {
        explosionSize = size;
    }

    #endregion

    #region Public Methods

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetPlayerGameobject(GameObject player)
    {
        playerGameObject = player;
    }

    #endregion

    #region Private Methods

    private void Expand()
    {
        if (circleCollider2.enabled == true)
        {
            explosionTransform.localScale += new Vector3(expansionRate, expansionRate, 0);
        }
    }

    private void WaitForParticles()
    {
        if (particleSystem.particleCount == 0 && circleCollider2.enabled == false)
        {
            Destroy(explosionTransform.gameObject);
        }
    }

    #endregion

    #region Coroutines

    private IEnumerator DisableExplosion()
    {
        yield return new WaitForSecondsRealtime(explosionTime);
        circleCollider2.enabled = false;
        foreach (SpriteRenderer sprite in shockWaves)
        {
            sprite.enabled = false;
        }

    }

    #endregion
}
