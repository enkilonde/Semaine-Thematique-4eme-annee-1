﻿using UnityEngine;

public struct VehicleProperties
{
    private float deltaTime
    {
        get { return AStar.FIXED_STEP; }
    }

    //Valeurs à initialiser
    public float rotationSpeed;
    public float maxSpeed;
    public float accelerationTime;
    public float grassSlowFactor;
    public float grassMaxSpeed;


    //Valeurs à updater
    public Vector3 position;
    public float orientation;
    public VehicleAction action;
    public int nextWaypointIndex;


    //Variables internes
    float speedAcceleration;
    public float grassDecelerate;



    public CustomTransform UpdateVehicle(VehicleAction action, bool isInGrass)
    {
        float orientationIncrement = 0.0f;
        float speedIncrement = 0.0f;

        //if (isInGrass) grassDecelerate -= deltaTime * grassSlowFactor;
        //else grassDecelerate += deltaTime * grassSlowFactor;

        //grassDecelerate = Mathf.Clamp(grassDecelerate, grassMaxSpeed, 1);

        if (isInGrass)
        {
            if(speedAcceleration > grassMaxSpeed)
            speedAcceleration = Mathf.Clamp(speedAcceleration - deltaTime * grassSlowFactor, grassMaxSpeed, 1);
        } 

        if ((action & VehicleAction.ACCELERATE) == VehicleAction.ACCELERATE) {
            speedAcceleration += deltaTime / accelerationTime;
        }
        else
        {
            speedAcceleration -= deltaTime / accelerationTime;
        }

        if ((action & VehicleAction.BRAKE) == VehicleAction.BRAKE)
        {
            speedAcceleration -= deltaTime / accelerationTime;
        }
        speedAcceleration = Mathf.Clamp01(speedAcceleration);
        speedIncrement = maxSpeed * deltaTime * speedAcceleration;

        if ((action & VehicleAction.LEFT) == VehicleAction.LEFT)
        {
            orientationIncrement = rotationSpeed * deltaTime;
        }

        if ((action & VehicleAction.RIGHT) == VehicleAction.RIGHT)
        {
            orientationIncrement = -rotationSpeed * deltaTime;
        }

       

        orientation = orientation + orientationIncrement;

        CustomTransform output;
        output.rotation = Quaternion.AngleAxis(orientation, Vector3.up);
        output.position = position + speedIncrement * (output.rotation * Vector3.forward); //experimental
        return output;
    }
}

public struct CustomTransform
{
    public Vector3 position;
    public Quaternion rotation;
}

