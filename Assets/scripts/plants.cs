using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using System.Runtime.CompilerServices;


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
    bool seeground = false;
    bool isupright = true;
    float checkTime;
    float old = 0f;
    public float sick = 0f;
    Rigidbody rb;
    
    

    protected override void Start()
    {
        base.Start();
        checkTime = Time.time + Random.Range(0f, 5f);
        plantRenderer = GetComponentInChildren<Renderer>();
        trunkRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        roundCollision = GetComponent<CapsuleCollider>();
        boxCollision = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        applyDna();
        Resize();
        age = 0;
        float plantSize = dna.maxHeight + dna.maxThickness;
        maxAge = plantSize * 100f;
        // start age timer
        // calculate max age based on size
        if(crowded())
        {
            global.Instance.returnPlantId(id);
            Destroy(gameObject); // destroy plant if too crowded
        }
        
        RaycastHit hit;
        if (Physics.Raycast(transform.position + UnityEngine.Vector3.up, UnityEngine.Vector3.down, out hit, 10f, groundLayer))
        {// snap plant to ground
            transform.position = hit.point;
        }
    }

    public void Update()
    {        
        if (Time.time >= checkTime)
        {
            age +=1;
            long thidss = id;
            checkTime = Time.time + 5f;
            if (age % 1 ==0)
            {
                RaycastHit hit;// if on the ground
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
                    // change color not shrink.
                    var sickColor = Color.Lerp(dna.stemColor, Color.black, sick);
                    plantRenderer.material.color = sickColor;
                    sick += 0.1f;
                    if (sick > .8f)
                    {
                        global.Instance.returnPlantId(id);
                        Destroy(gameObject); // kill sick plant
                    } 
                }
                else 
                {
                    if(rb != null && !rb.isKinematic)
                    { // root itself
                        rb.isKinematic = true;
                        rb.useGravity = false;
                    }
                    if (growth < dna.maxHeight || growth < dna.maxThickness)
                    {
                        if (!crowded())
                        {
                            growth += 0.01f;
                            Resize(); // Grow plant
                        }
                    }
                    if (sick > 0f)
                    {
                        sick -= 0.1f; // recover if not sick anymore
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
                    old += 0.1f;
                    if (old > .8f)
                    {
                        global.Instance.returnPlantId(id);
                        Destroy(gameObject); // kill old plant
                    }
                }
            }
            //breeding
            if (age % dna.breedingFrequency == 0 && !crowded() && growth > dna.maxHeight * 0.8f)
            {
                UnityEngine.Vector3 babyLocation = transform.position + new UnityEngine.Vector3( Random.Range(-3f, 3f),0f,Random.Range(-3f, 3f));
                spawner.InstanceCreator.SpawnPlant(babyLocation, dna);
            }
        }
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
        trunk.localPosition = new UnityEngine.Vector3(0f, y / 1f, 0f);
        
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



        /*
        // change trunk shape
        Mesh workingTrunkMesh;
        UnityEngine.Vector3[] origionalVerts;
        workingTrunkMesh = Instantiate(trunkMeshFilter.mesh);
        trunkMeshFilter.mesh = workingTrunkMesh;// create and assign clone
        origionalVerts = workingTrunkMesh.vertices;
        UnityEngine.Vector3[] newVerts = new UnityEngine.Vector3[origionalVerts.Length];
        origionalVerts.CopyTo(newVerts, 0);
        float minY = float.MaxValue;
        float maxY = float.MinValue;
        for (int i = 0; i < newVerts.Length; i++)
        {
            if (origionalVerts[i].y < minY) minY = origionalVerts[i].y;
            if (origionalVerts[i].y > maxY) maxY = origionalVerts[i].y;
        }
        for (int i = 0; i < newVerts.Length; i++)
        {
            float heightPercent = (origionalVerts[i].y - minY) / (maxY - minY);
            float width = 1f;
            if (heightPercent < 0.25f)
            {
                width = dna.trunkSegment1Width;
            }
            else if (heightPercent < 0.5f)
            {
                width = dna.trunkSegment2Width;
            }
            else if (heightPercent < 0.75f)
            {
                width = dna.trunkSegment3Width;
            }
            else
            {
                width = dna.trunkSegment4Width;
            }
            newVerts[i].x *= width;
            newVerts[i].z *= width;
        }
        workingTrunkMesh.vertices = newVerts;
        workingTrunkMesh.RecalculateBounds();
        workingTrunkMesh.RecalculateNormals();

        */

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
        // branch color here
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


