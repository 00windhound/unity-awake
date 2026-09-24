using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


public class plants : livingThing
{
    public LayerMask groundLayer; 
    public LayerMask plantLayer;
    public Transform trunk;
    public CapsuleCollider roundCollision;
    public BoxCollider boxCollision;
    public GameObject stickPrefab;
    //public Transform sticks;
    public Renderer plantRenderer;
    public plantDNA dna;
    public float growth = 0.1f;
    public float maxAge = 100f;
    public List<Stick> sticks = new List<Stick>();
    public SkinnedMeshRenderer trunkRenderer;
    public float growSpeed = 1f;
    bool seeground = false;
    bool isupright = true;
    float lastUpdateTime;
    float old = 0f;
    public float sick = 0f;
    Rigidbody rb;
    
    

    protected override void Start()
    {
        base.Start();
        plantRenderer = GetComponentInChildren<Renderer>();
        trunkRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        roundCollision = GetComponent<CapsuleCollider>();
        boxCollision = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        applyDna();
        Resize();
        float plantSize = dna.maxHeight + dna.maxThickness;
        maxAge = plantSize * 100f;
        // start age timer
        // calculate max age based on size
        if(age == 0)
        {
            if(crowded()){Die();}
            else{age = 1;}
        }
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position + UnityEngine.Vector3.up, UnityEngine.Vector3.down, out hit, 10f, groundLayer))
        {// snap plant to ground
            transform.position = hit.point;
        }
        global.Instance.plantsRunningList.Add(this);//add reference to global script
    }


    public void UpdatePlant()
    {
        if (Time.time > lastUpdateTime + growSpeed)
        {
            lastUpdateTime = Time.time;
            age +=1; 

            RaycastHit hit;// check if on the ground
            if (Physics.Raycast(transform.position + UnityEngine.Vector3.up, UnityEngine.Vector3.down, out hit, 10f, groundLayer))
            {
                seeground = true;
                float upright = UnityEngine.Vector3.Dot(transform.up, hit.normal);
                if (upright < 0.8f){isupright = false;}
                else{isupright = true;}
            }
            else{seeground = false;}
            if (!seeground || !isupright)
            {
                // sick plant, change color
                var sickColor = Color.Lerp(dna.stemColor, Color.black, sick);
                plantRenderer.material.color = sickColor;
                sick += 0.1f;
                if (sick > .8f){Die();} 
            }
            else 
            {
                if(rb != null && !rb.isKinematic)
                { // root itself
                    rb.isKinematic = true;
                    rb.useGravity = false;
                }
                if (growth < dna.maxHeight || growth < dna.maxThickness)
                {// grow the plant
                    if (!crowded()) {growth += 0.01f; Resize();}
                }
                if (sick > 0f)// recover if not sick anymore
                {
                    sick -= 0.1f; 
                    var recoverColor = Color.Lerp(dna.stemColor, Color.black, sick);
                    plantRenderer.material.color = recoverColor;
                };
                    
            }

            // old age
            if (age > maxAge)
            {
                // old age color
                var oldColor = Color.Lerp(dna.stemColor, Color.black, old);
                plantRenderer.material.color = oldColor;
                growth -= 0.02f;
                Resize();
                old += 0.1f;
                if (old > .8f){Die();}
            }
            
            //breeding
            if (age % dna.breedingFrequency == 0 && !crowded() && growth > dna.maxHeight * 0.8f)
            {
                UnityEngine.Vector3 babyLocation = transform.position + new UnityEngine.Vector3( Random.Range(-3f, 3f),0f,Random.Range(-3f, 3f));
                spawner.InstanceCreator.SpawnPlant(babyLocation, dna);
            }
        }
        
    }

    public void Die()
    {
        global.Instance.returnPlantId(id);
        global.Instance.plantsRunningList.Remove(this);
        Destroy(gameObject);
    }


    public void Resize()
    {
        // resize collider
        roundCollision.radius = growth;
        roundCollision.height = growth * 2f;
        roundCollision.center = new UnityEngine.Vector3(0f, roundCollision.radius, 0f);
        boxCollision.size = new UnityEngine.Vector3(growth, growth, growth);
        boxCollision.center = new UnityEngine.Vector3(0f, growth / 2f, 0f);

        // resizing trunk
        float x = growth;// x is width or flat
        float y = growth;// y is width
        float z = growth;// z is height
        if (growth > dna.maxThickness) y = dna.maxThickness;
        if (growth > dna.maxHeight) z = dna.maxHeight;
        x = dna.trunkFlat * y; 
        trunk.localScale = new UnityEngine.Vector3(x, y, z);
        trunk.localPosition = new UnityEngine.Vector3(0f, 0f, 0f);
        
        // resizing sticks
         foreach (Stick s in sticks)
        {
            s.gameObject.transform.localScale = new UnityEngine.Vector3(dna.stickThickness * growth, dna.stickLength * growth, dna.stickThickness * growth);
            s.gameObject.transform.localPosition = new UnityEngine.Vector3(0f, s.heightPercent * y, 0f);
            UnityEngine.Quaternion rotateAround = UnityEngine.Quaternion.Euler(0f, s.angleAround, 0f);
            UnityEngine.Quaternion tilt = UnityEngine.Quaternion.Euler(-dna.stickDownwardAngle, 0f, 0f);
            s.gameObject.transform.localRotation = rotateAround * tilt;
        }

    }


    public bool crowded()
    {// return true if too crowded
        float radius = trunk.localScale.x;
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            radius,
            ~0
        );
        int count = hits.Length;
        return count > 3;
    }


    public void applyDna()
    {
        // apply trunk color
        plantRenderer.material.color = dna.stemColor;
        

        //change trunk shape keys
        int height5 = trunkRenderer.sharedMesh.GetBlendShapeIndex("height5");
        trunkRenderer.SetBlendShapeWeight(height5, dna.trunkSegment5Width);
        int height4 = trunkRenderer.sharedMesh.GetBlendShapeIndex("height4");
        trunkRenderer.SetBlendShapeWeight(height4, dna.trunkSegment4Width);
        int height3 = trunkRenderer.sharedMesh.GetBlendShapeIndex("height3");
        trunkRenderer.SetBlendShapeWeight(height3, dna.trunkSegment3Width);
        int height2 = trunkRenderer.sharedMesh.GetBlendShapeIndex("height2");
        trunkRenderer.SetBlendShapeWeight(height2, dna.trunkSegment2Width);
        int height1 = trunkRenderer.sharedMesh.GetBlendShapeIndex("height1");
        trunkRenderer.SetBlendShapeWeight(height1, dna.trunkSegment1Width);


        // add branches
        for (int i = 0; i < dna.stickCount; i++)
        {
            GameObject stickObj = Instantiate(stickPrefab);
            stickObj.transform.SetParent(transform,false);
            Stick stickData = new Stick();
            stickData.gameObject = stickObj;

            stickData.heightPercent= Random.Range(dna.stickLowestY, 1f );
            stickData.angleAround = Random.Range(0f, 360f);
            //stickData.outwardAngle = Random.Range(20f, 70f);

            sticks.Add(stickData);
        }

        //change color
        plantRenderer.material.color = dna.stemColor;
    }


    public PlantData Data()// for saving the game
    {
        PlantData data = new PlantData();
        data.position = transform.position;
        data.age = age;
        data.growth = growth;
        data.dna = dna;
        return data;
    }

    public void LoadData(PlantData data)
    {
        transform.position = data.position;
        age = data.age;
        growth = data.growth;
        dna = data.dna;
    }
    
    [System.Serializable]
    public class PlantData
    {
        public UnityEngine.Vector3 position;
        public float age;
        public float growth;
        public plantDNA dna;
    }

    [System.Serializable]
    public class Stick
    {
        public GameObject gameObject;
        public float heightPercent;
        public float angleAround;
        //public float outwardAngle;

    }


    
}


