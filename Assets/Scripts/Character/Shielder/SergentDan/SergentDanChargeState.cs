using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SergentDanChargeState : State<SergentDan>
{

    private static readonly SergentDanChargeState instance = new SergentDanChargeState();

    public static SergentDanChargeState Instance
    {
        get
        {
            return instance;
        }
    }

    public enum ChargeState
    {
        Nothing,
        Idle,
        NormalAttack,
        SpecialAttack,
        Chase
    }

    private ChargeState chargeState;
    private float distance;
    private float idleElapsedTime = 0;
    private float normalAttackElapsedTime = 0;
    private float rate = 1.4f;
    private int commandIndex;

    public override void Enter(SergentDan entity)
    {
        idleElapsedTime = 1;
        commandIndex = 0;
        chargeState = ChargeState.Idle;
        Debug.Log("Enter Charge");
    }

    public override void Execute(SergentDan entity)
    {
        distance = Mathf.Abs(entity.transform.position.x - entity.Player.position.x);
        // 攻击计数器
        normalAttackElapsedTime -= Time.deltaTime;

        if (chargeState == ChargeState.Idle)
        {
            idleElapsedTime -= Time.deltaTime;
            if (idleElapsedTime <= 0)
            {
                if (commandIndex > 4)
                {
                    entity.Animator.SetBool("Run", false);
                    entity.GetFSM().ChangeState(SergentDanLRState.Instance);
                    return;
                }
                chargeState = entity.AttackCommand[commandIndex];
                commandIndex++;
                idleElapsedTime = Random.Range(1f, 1.5f);
            }

        }
        else if (chargeState == ChargeState.NormalAttack)
        {
            entity.Controller.SetHorizontalForce(0.0f);
            if (normalAttackElapsedTime <= 0)
            {
                entity.IsInAttackAction = true;
                normalAttackElapsedTime = rate;
                entity.FaceToTarget();
                entity.Animator.Play("Base Layer.CRAttack", 0, 0);
            }

            if (entity.IsInAttackAction == false)
            {
                chargeState = ChargeState.Idle;
                entity.Animator.SetBool("Run", false);
            }
        }
        else if (chargeState == ChargeState.SpecialAttack)
        {
            entity.Controller.SetHorizontalForce(0.0f);
            if (normalAttackElapsedTime <= 0)
            {
                entity.IsInAttackAction = true;
                normalAttackElapsedTime = rate;
                entity.FaceToTarget();
                entity.Animator.SetBool("Jump", true);
            }

            //
            if (entity.IsInAttackAction == false)
            {
                chargeState = ChargeState.Idle;
                entity.Animator.SetBool("Run", false);
            }
        }
        else if (chargeState == ChargeState.Chase)
        {
            if (distance >= entity.AttackRange)
            {
                entity.Animator.SetBool("Run", true);
                entity.FaceToTarget();
            }
            else
            {
                chargeState = ChargeState.NormalAttack;
            }

        }
    }

    public override void Exit(SergentDan entity)
    {

        Debug.Log("Exit Charge");
    }

}
