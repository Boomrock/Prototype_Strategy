using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;

interface IPlayer
{
    void MovingTheView();

    void ClickEvent();

    void CreatingAGroupOfUnits();
}

interface IUnit
{
    void MovingToAnyLocation();

    void MovingFromTheCursor();

    void Working();

    void UnitCreation();

    void UnitDeath();

    void UnitUpgrade();
} 

interface IBuilding
{
    void CreatingABuilding();

    void BuildingUpgrade();

    void BuildingDestruction();

    void MovingABuilding();
}
