using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using UnityEngine;

[System.Serializable]
public class plantDNA
{
    
    
   
    // trunk
    public UnityEngine.Color stemColor = UnityEngine.Color.green;
    public float trunkSegment5Width = 0f;
    public float trunkSegment4Width = 0f;
    public float trunkSegment3Width = 0f;
    public float trunkSegment2Width = 0f;
    public float trunkSegment1Width = 0f;
    public float maxHeight = 1;
    public float maxThickness = 1;
    public float trunkFlat = 1;

    public int nutritionProtein = 0;
    public int nutritionToxin = 0;
    public int nutritionSugar = 0;
    //trunk


    // stick
    public int stickCount = 0;
    public float stickLength = .3f;
    //length distribution? longer, shorter near top, bottom?
    public float stickThickness = .1f;
    public float stickLowestY = 0.5f;// 0.1 - 1
    public float stickDownwardAngle = 45f;
    public UnityEngine.Color stickColor = UnityEngine.Color.green;
    //stick


    // leaves

    //leaves


    // flowers

    //flowers

        
    // breeding
    public int breedingFrequency = 5;
    //breeding
    //behavior
    //public int maxAge = 6000;
    //behavior

    public plantDNA Clone()
    {
        return (plantDNA)this.MemberwiseClone();
    }
}
