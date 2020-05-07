using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterIK : MonoBehaviour
{
    private Character m_Character = null;
    private Animator animator = null;
    private Rigidbody m_Rigidody = null;
    [SerializeField]
    private Transform targetTransform = null;

    [SerializeField]
    private Hook hook;



    public void SetTarget(Transform newTransform)
    {
        targetTransform = newTransform;
    }

    public Transform GetTarget()
    {
        return targetTransform;
    }

    public void ReleaseTarget()
    {
        targetTransform = null;
    }

    private void Start()
    {
        m_Character = GetComponent<Character>();
        animator = GetComponent<Animator>();
        m_Rigidody = GetComponent<Rigidbody>();
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if (targetTransform != null)
        {
            animator.SetLookAtWeight(0.6f);
            animator.SetLookAtPosition(targetTransform.position);



            //var hips = animator.GetBoneTransform(HumanBodyBones.Hips);

            
            /*m_Rigidody.isKinematic = true;
            m_Rigidody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            Quaternion quat = Quaternion.Euler(hook.GetAngle(), 0.0f, 0.0f);
            animator.SetBoneLocalRotation(HumanBodyBones.Hips, quat);

            m_Rigidody.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            m_Rigidody.isKinematic = false;*/

        }
        
    }
}
