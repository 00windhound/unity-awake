using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using UnityEngine;

[System.Serializable]
public class plantDNA
{
    
    
   
    // stem
    public UnityEngine.Color stemColor = UnityEngine.Color.green;
    public float trunkSegment4Width = 1f;
    public float trunkSegment3Width = 1f;
    public float trunkSegment2Width = 1f;
    public float trunkSegment1Width = 1f;
    public float maxHeight = 1;
    public float maxThickness = 1;

    public int nutritionProtein = 0;
    public int nutritionToxin = 0;
    public int nutritionSugar = 0;
    //stem


    // stick
    public int stickCount = 0;
    public float stickLength = 0;
    public float stickThickness = 0;
    //stick


    // leaves

    //leaves


    // flowers

    //flowers

        
    // breeding
    public int freaquency = 130;
    //breeding
    //behavior
    public int maxAge = 6000;
    //behavior

    public plantDNA Clone()
    {
        return (plantDNA)this.MemberwiseClone();
    }
}
