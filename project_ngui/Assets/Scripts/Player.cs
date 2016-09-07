﻿using UnityEngine;
using System.Collections;

public class Player {
    private static Player instance;
    public static Player Instance {
        get {
            if (instance == null) {
                instance = new Player();
            }
            return instance;
        }
    }

}
