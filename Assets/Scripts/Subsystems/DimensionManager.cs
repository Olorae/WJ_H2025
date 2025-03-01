using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionManager : ISubSystem
{
    private static DimensionManager DimensionManagerInstance;

    public Action ToDeadLand;

    public Action ToLivingLand;

    public bool inLivingLand;

    public DimensionManager()
    {
        inLivingLand = false;
    }
    public static ISubSystem GetSubSystem()
    {

        if (DimensionManagerInstance == null) {
            DimensionManagerInstance = new DimensionManager();
        }

        return DimensionManagerInstance;
    }
}
