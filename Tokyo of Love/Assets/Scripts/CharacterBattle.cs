﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBattle : MonoBehaviour
{
    public float slideSpeed = 10.0f;
    public float reachedDistance = 1.0f;
    public int healthMax = 100;
    public HealthBar healthBar;
    private bool isPlayerAlly;
    public bool checkedDeath = false;
    public bool blocking = false;
    public int damagemin;
    public int damagemax;
    private BattleHandler battleHandler;

    [SerializeField] private Transform damagePopup;
    private Vector3 slideTargetPosition;
    private Action onSlideComplete;
    private GameObject selectionCircle;
    private GameObject arrow;
    private HealthSystem healthSystem;
    private State state;

    private enum State
    {
        Idle,
        Sliding,
        Busy
    }

    private void Awake()
    {
        selectionCircle = transform.Find("SelectionCircle").gameObject;
        arrow = transform.Find("SelectionArrow").gameObject;
        HideSelectionCircle();
        state = State.Idle;
    }

    private void Start()
    {
        battleHandler = GameObject.Find("BattleHandler").GetComponent<BattleHandler>();
    }

    public void Setup(bool isPlayerAlly)
    {
        this.isPlayerAlly = isPlayerAlly;

        healthSystem = new HealthSystem(healthMax);
        healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        //PlayIdleAnimation();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e)
    {
        healthBar.SetSize(healthSystem.GetHealthPercent());
    }

    private void PlayIdleAnimation()
    {
        /*if(isPlayerAlly)
            controller. // Play idle animation new Vector(1, 0)
        else
            controller. // Play idle animation new Vector(-1, 0)
        */
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;

            case State.Sliding:
                transform.position += (slideTargetPosition - GetPosition()) * slideSpeed * Time.deltaTime;
                if (Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance)
                {
                    transform.position = slideTargetPosition;
                    onSlideComplete();
                }
                break;

            case State.Busy:
                break;
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void Damage(int damageAmount)
    {
        healthSystem.Damage(damageAmount);
        DamagePopup.Create(damagePopup, GetPosition(), damageAmount, false);

        /*if(healthSystem.IsDead())
            controller. // play dead animation*/
    }

    public bool IsDead()
    {
        return healthSystem.IsDead();
    }

    public void Attack(CharacterBattle target, Action onAttackComplete)
    {
        Vector3 slideTargetPosition = target.GetPosition() + (GetPosition() - target.GetPosition()).normalized * 10.0f;
        Vector3 startingPosition = GetPosition();

        // Slide to target
        SlideToPosition(slideTargetPosition, () => {
            // Arrive to target, attack it
            state = State.Busy;
            Vector3 attackDir = (target.GetPosition() - GetPosition()).normalized;
            //controller. // Play attack animation
            
            int damageAmount = UnityEngine.Random.Range(damagemin, damagemax);
            
            if (target.blocking) damageAmount = damageAmount / 2;

            target.Damage(damageAmount);
            if (target.IsDead() && target.checkedDeath == false)
            {
                if (target.isPlayerAlly) battleHandler.allies--;
                else battleHandler.enemies--;
                target.checkedDeath = true;
            }
            // Attack comlpeted, slide back
            SlideToPosition(startingPosition, () => {
                state = State.Idle;
                //controller. // Play idle animation
                onAttackComplete();
            });
        });
    }

    private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete)
    {
        this.slideTargetPosition = slideTargetPosition;
        this.onSlideComplete = onSlideComplete;
        state = State.Sliding;
        //if (slideTargetPosition.x > 0)
        // controller. // Play slide animation right
        // else
        // controller. // Play slide animation left 
    }

    public void HideSelectionCircle()
    {
        selectionCircle.SetActive(false);
    }

    public void ShowSelectionCircle()
    {
        selectionCircle.SetActive(true);
    }

    public void HideArrow()
    {
        arrow.SetActive(false);
    }

    public void ShowArrow()
    {
        arrow.SetActive(true);
    }
}