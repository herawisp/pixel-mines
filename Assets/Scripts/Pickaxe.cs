using System.Collections.Generic;
using UnityEngine;

public enum PickaxeMaterial {Stone, Copper, Iron, Gold, Diamond}

public class Pickaxe : MonoBehaviour {

    //================================================================================================//
    //================================================================================================//

    public PickaxeMaterial Material {get; private set;}
    public List<RuntimeAnimatorController> AnimatorControllers;

    SpriteRenderer _spriteRenderer;
    Animator _animator;

    //================================================================================================//
    //================================================================================================//

    void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    public void ChangeMaterial(PickaxeMaterial newMaterial) {
        _animator.runtimeAnimatorController = AnimatorControllers[(int) newMaterial];
        Material = newMaterial;
    }
    
    public void FlipX(bool flipX) {
        transform.localPosition = new Vector3(flipX? 0.56f: -0.56f, 0, 0); 
        _spriteRenderer.flipX = flipX;
    }

    public void StartSwinging() {
        _spriteRenderer.enabled = true; 
        _animator.SetBool("IsSwinging", true);
    }
    
    public void StopSwinging() {
        _spriteRenderer.enabled = false; 
        _animator.SetBool("IsSwinging", false);
    }

    //================================================================================================//
    //================================================================================================//
}
