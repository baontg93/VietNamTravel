using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalGameEventManager : SingletonBehaviour<LocalGameEventManager>
{
    public delegate void VisitMyLandDetailResource();
    public static event VisitMyLandDetailResource eventVisitMyLandDetailResource;
}
