using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using CodeMonkey.Utils;

public class PlayerAnimation : MonoBehaviour
{
    /*
        Order in this Script:
            Rotation to Mouse
            Animation
            ...
    */

    //
    //Player Rotation to mouse preparation
        private Transform aimTransform;

    //END
    //

    //
    //Animation Stuff Preparation
        public float BackSpeed = -0.7f;
        private string currentState;
        private Animator animator;
        private PlayerControler playerControler;
        private PlayerMover playerMover;
        public bool LowerSpeed = false; //Speed Lowerer Boolean
        public bool isMeleeAttacking = false;
        AnimatorClipInfo[] animatorInfo;

        //Look at player when not current player
        public bool lookAtPlayer;

        //State Handeling
        private PlayerStatsAndState pSAT;

        //Chechink Melee Anim Spee
        float MeleeAnimSpeed;
        float AttackSpeedMulti;
        float AttackSpeed;

        void ChangeAnimationState(string newState)
    {
        //Stop the same animation from interrupting itself
        if (currentState == newState) return;

        //Play the animation Imputed by the script
        animator.Play(newState);

        //Reassigning the current State
        currentState = newState;

    }

    void PlayOnce(string newState)
    {
        string oldState;
        animatorInfo = animator.GetCurrentAnimatorClipInfo(0);
        oldState = animatorInfo[0].clip.name;
        animator.Play(newState);

        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
        {
            animator.Play(oldState);
        }
            


    }

    //Animation States
    //WARNING: This might get long, turn into "ENUM" in the future

    //Idles
    const string PLAYER_IdleFront = ("Player_IdleFront");
        const string PLAYER_IdleBack = ("Player_IdleBack");
        const string PLAYER_IdleLeft = ("Player_IdleLeft");
        const string PLAYER_IdleRight = ("Player_IdleRight");
        const string PLAYER_IdleFrontLeft = ("Player_IdleFrontLeft");
        const string PLAYER_IdleFrontRight = ("Player_IdleFrontRight");
        const string PLAYER_IdleBackLeft = ("Player_IdleBackLeft");
        const string PLAYER_IdleBackRight = ("Player_IdleBackRight");

        //Run
        const string PLAYER_RunFront = ("Player_RunFront");
        const string PLAYER_RunBack = ("Player_RunBack");
        const string PLAYER_RunRight = ("Player_RunRight");
        const string PLAYER_RunLeft = ("Player_RunLeft");
        const string PLAYER_RunBackRight = ("Player_RunBackRight");
        const string PLAYER_RunBackLeft = ("Player_RunBackLeft");
        const string PLAYER_RunFrontRight = ("Player_RunFrontRight");
        const string PLAYER_RunFrontLeft = ("Player_RunFrontLeft");

        //Direction checking function for the animation

        private void CheckDirection(bool condition)
        {
            if (condition == true)
            {
                animator.SetFloat("Direction", BackSpeed);
                LowerSpeed = true;
            }
            else
            {
                animator.SetFloat("Direction", 1);
                LowerSpeed = false;
            }
        }

    //Animation stuff
    //END

    private void Awake()
    {

        //State Handeling
        pSAT = GetComponent<PlayerStatsAndState>();

        //Player Rotation to mouse
        animator = GetComponent<Animator>();
        aimTransform = transform.Find("Aim");

        //Player Animation
        playerMover = GetComponent<PlayerMover>();
        playerControler = GetComponent<PlayerControler>();

        UpdateAnimClipTimes();

    }

    void Update()
    {

        if(pSAT.weaponType == "Melee")
        {
            AttackSpeed = GetComponentInChildren<MeleeWeaponControler>().AttackAnimSpeed;
            AttackSpeedMulti = AttackSpeed * MeleeAnimSpeed;
            animator.SetFloat("MeleeAttackSpeed", AttackSpeedMulti);
        }

        if (lookAtPlayer == true)
        {
            //LookAtPlayer();
            Debug.Log("Should be looking at plaer");
        }
        else
        {
            if (isMeleeAttacking == true)
            {
                MeleeAttack();
            }
            else
            {
                LookAtMouse();
            }
            
                
            
        }


    }

    public void MeleeAttack()
    {
        Vector3 mPosition = UtilsClass.GetMouseWorldPosition();

        Vector3 aimDirection = (mPosition - transform.position).normalized;
        float Angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        //
        //Player Look n' Run Animations

        //Right
        if ((Angle < 22.5f && Angle > 0f) || (Angle < 0f && Angle > -22.5f))
        {

            ChangeAnimationState("Melee_Player_Right");
            float timeToStop = MeleeAnimSpeed * AttackSpeedMulti;
            Invoke("MakeMeleeAttackFalse", AttackSpeed);

        }

        //Back Right
        else if (Angle > 22.5f && Angle < 67.5f)
        {
            ChangeAnimationState("Melee_Player_BackRight");
            float timeToStop = MeleeAnimSpeed * AttackSpeedMulti;
            Invoke("MakeMeleeAttackFalse", timeToStop);
        }

        //Back
        else if (Angle > 67.5f && Angle < 112.5f)
        {
            ChangeAnimationState("Melee_Player_Back");
            float timeToStop = MeleeAnimSpeed * AttackSpeedMulti;
            Invoke("MakeMeleeAttackFalse", timeToStop);
        }

        //Back Left
        else if (Angle > 112.5f && Angle < 157.5f)
        {
            ChangeAnimationState("Melee_Player_BackLeft");
            float timeToStop = MeleeAnimSpeed * AttackSpeedMulti;
            Invoke("MakeMeleeAttackFalse", timeToStop);
        }

        //Left
        else if ((Angle > 157.5f && Angle < 180f) || (Angle > -180f && Angle < -157.5f))
        {
            ChangeAnimationState("Melee_Player_Left");
            float timeToStop = MeleeAnimSpeed * AttackSpeedMulti;
            Invoke("MakeMeleeAttackFalse", timeToStop);
        }

        //Front Left
        else if (Angle > -157.5f && Angle < -112.5f)
        {
            ChangeAnimationState("Melee_Player_FrontLeft");
            float timeToStop = MeleeAnimSpeed * AttackSpeedMulti;
            Invoke("MakeMeleeAttackFalse", timeToStop);
        }

        //Front
        else if (Angle > -112.5f && Angle < -67.5f)
        {
            ChangeAnimationState("Melee_Player_Front");
            float timeToStop = MeleeAnimSpeed * AttackSpeedMulti;
            Invoke("MakeMeleeAttackFalse", timeToStop);
        }

        //Front Right
        else if (Angle > -67.5f && Angle < 22.5f)
        {
            ChangeAnimationState("Melee_Player_FrontRight");
            float timeToStop = MeleeAnimSpeed * AttackSpeedMulti;
            Invoke("MakeMeleeAttackFalse", timeToStop);
        }
    }
    public void MakeMeleeAttackFalse()
    {
        isMeleeAttacking = false;
    }
    public void LookAtMouse()
    {
        Vector3 mPosition = UtilsClass.GetMouseWorldPosition();

        Vector3 aimDirection = (mPosition - transform.position).normalized;
        float Angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        //
        //Player Look n' Run Animations

        //Right
        if ((Angle < 22.5f && Angle > 0f) || (Angle < 0f && Angle > -22.5f))
        {

            //Check direction
            CheckDirection(playerControler.movementInput.x < 0);

            //Add Run
            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunRight);
                return;
            }
            ChangeAnimationState(pSAT.playerState + PLAYER_IdleRight);

            
        }

        //Back Right
        else if (Angle > 22.5f && Angle < 67.5f)
        {
            //Check direction
            CheckDirection(playerControler.movementInput.y < 0 || playerControler.movementInput.x < 0);

            //Add Run

            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunBackRight);
                return;
            }
            ChangeAnimationState(pSAT.playerState + PLAYER_IdleBackRight);
        }

        //Back
        else if (Angle > 67.5f && Angle < 112.5f)
        {
            //Check direction
            CheckDirection(playerControler.movementInput.y < 0);

            //Add Run
            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunBack);
                return;
            }
            ChangeAnimationState(pSAT.playerState + PLAYER_IdleBack);
        }

        //Back Left
        else if (Angle > 112.5f && Angle < 157.5f)
        {
            //Check direction
            CheckDirection(playerControler.movementInput.y < 0 || playerControler.movementInput.x > 0);

            //Add Run
            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunBackLeft);
                return;
            }
            ChangeAnimationState(pSAT.playerState + PLAYER_IdleBackLeft);
        }

        //Left
        else if ((Angle > 157.5f && Angle < 180f) || (Angle > -180f && Angle < -157.5f))
        {
            //Check direction
            CheckDirection(playerControler.movementInput.x > 0);

            //Add Run
            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunLeft);
                return;
            }
            ChangeAnimationState(pSAT.playerState + PLAYER_IdleLeft);
        }

        //Front Left
        else if (Angle > -157.5f && Angle < -112.5f)
        {
            //Check direction
            CheckDirection(playerControler.movementInput.x > 0 || playerControler.movementInput.y > 0);

            //Add Run
            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunFrontLeft);
                return;
            }
            ChangeAnimationState(pSAT.playerState + PLAYER_IdleFrontLeft);
        }

        //Front
        else if (Angle > -112.5f && Angle < -67.5f)
        {
            //Check direction
            CheckDirection(playerControler.movementInput.y > 0);

            //Add Run
            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunFront);
                return;
            }


            ChangeAnimationState(pSAT.playerState + PLAYER_IdleFront);
        }

        //Front Right
        else if (Angle > -67.5f && Angle < 22.5f)
        {
            //Check direction
            CheckDirection(playerControler.movementInput.x < 0 || playerControler.movementInput.y > 0);

            //Add Run
            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunFrontRight);
                return;
            }
            ChangeAnimationState(pSAT.playerState + PLAYER_IdleFrontRight);
        }
    }

    public void LookAtPlayer()
    {
        Vector3 mPosition = gameObject.transform.position;

        Vector3 aimDirection = (mPosition /*- transform.position*/).normalized;
        float Angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        //
        //Player Look n' Run Animations

        //Right
        if ((Angle < 22.5f && Angle > 0f) || (Angle < 0f && Angle > -22.5f))
        {
            //Check direction
            CheckDirection(playerControler.movementInput.x < 0);

            //Add Run
            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunRight);
                return;
            }
            ChangeAnimationState(pSAT.playerState + PLAYER_IdleRight);
        }

        //Back Right
        else if (Angle > 22.5f && Angle < 67.5f)
        {
            //Check direction
            CheckDirection(playerControler.movementInput.y < 0 || playerControler.movementInput.x < 0);

            //Add Run

            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunBackRight);
                return;
            }
            ChangeAnimationState(pSAT.playerState + PLAYER_IdleBackRight);
        }

        //Back
        else if (Angle > 67.5f && Angle < 112.5f)
        {
            //Check direction
            CheckDirection(playerControler.movementInput.y < 0);

            //Add Run
            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunBack);
                return;
            }
            ChangeAnimationState(pSAT.playerState + PLAYER_IdleBack);
        }

        //Back Left
        else if (Angle > 112.5f && Angle < 157.5f)
        {
            //Check direction
            CheckDirection(playerControler.movementInput.y < 0 || playerControler.movementInput.x > 0);

            //Add Run
            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunBackLeft);
                return;
            }
            ChangeAnimationState(pSAT.playerState + PLAYER_IdleBackLeft);
        }

        //Left
        else if ((Angle > 157.5f && Angle < 180f) || (Angle > -180f && Angle < -157.5f))
        {
            //Check direction
            CheckDirection(playerControler.movementInput.x > 0);

            //Add Run
            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunLeft);
                return;
            }
            ChangeAnimationState(pSAT.playerState + PLAYER_IdleLeft);
        }

        //Front Left
        else if (Angle > -157.5f && Angle < -112.5f)
        {
            //Check direction
            CheckDirection(playerControler.movementInput.x > 0 || playerControler.movementInput.y > 0);

            //Add Run
            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunFrontLeft);
                return;
            }
            ChangeAnimationState(pSAT.playerState + PLAYER_IdleFrontLeft);
        }

        //Front
        else if (Angle > -112.5f && Angle < -67.5f)
        {
            //Check direction
            CheckDirection(playerControler.movementInput.y > 0);

            //Add Run
            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunFront);
                return;
            }


            ChangeAnimationState(pSAT.playerState + PLAYER_IdleFront);
        }

        //Front Right
        else if (Angle > -67.5f && Angle < 22.5f)
        {
            //Check direction
            CheckDirection(playerControler.movementInput.x < 0 || playerControler.movementInput.y > 0);

            //Add Run
            if (playerMover.currentSpeed > 0)
            {
                ChangeAnimationState(pSAT.playerState + PLAYER_RunFrontRight);
                return;
            }
            ChangeAnimationState(pSAT.playerState + PLAYER_IdleFrontRight);
        }
    }

    public void UpdateAnimClipTimes()
    {
        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            switch (clip.name)
            {
                case "Melee_Player_Front":
                    MeleeAnimSpeed = clip.length;
                    break;

            }
        }


    }


}
