using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy ")]
public class EnemyStats : ScriptableObject
{
    [Range(0, 200)] public float maxHealth;
    [Range(0, 200)] public float health;
    [Range(0, 10)] public float speed;
    [Range(0, 100)] public float loudness;
    [Range(0, 100)] public float damage;
    [Range(0, 10)] public float sightRange;
    [Range(0, 5)] public float attackRange;
    [Range(0, 1)] public float attackRate;
    [Range(0, 10)] public float detectionRange;
    [Range(0, 1)] public float minStepInterval;
    [Range(0, 5)] public int numberHealth;
    [Range(0, 20)] public int numberMovement;

    //new
    [Range(0, 100)] public int moneyloot;
}
