using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class SetSpriteLayer : MonoBehaviour {

    private ParticleSystem _particle;
    public string Layer = "ParallaxBackground";


    void Start()
    {
        _particle = GetComponent<ParticleSystem>();
        _particle.renderer.sortingLayerName = Layer;
    }

    void Update()
    {

    }
}
