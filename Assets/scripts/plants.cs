using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Numerics;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;


public class plants : livingThing
{
    public LayerMask groundLayer; 
    public LayerMask plantLayer;
    public Transform model;
    public Renderer plantRenderer;
    public plantDNA dna;
    public float growth = 0.1f;
    
    

    protected override void Start()
    {
        base.Start();
        plantRenderer = GetComponentInChildren<Renderer>();
        applyDna();
        
        Resize();
        age = 0;
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

    protected override void Update()
    {
        base.Update();
        
        
        
        if (age % 10 == 0)
        {
            bool seeground = false;
            bool isupright = true;
            RaycastHit hit;// if on the ground
            if (Physics.Raycast(transform.position + UnityEngine.Vector3.up, UnityEngine.Vector3.down, out hit, 10f, groundLayer))
            {
                seeground = true;
            }

            float upright = UnityEngine.Vector3.Dot(transform.up, UnityEngine.Vector3.up);
            if (upright < 0.5f)
            {
                isupright = false;
            }


            if (!seeground || !isupright)
            {
                growth -= 0.01f;
                Resize(); // Shrink plant by 1%
                if (growth < 0.1f)
                {
                    global.Instance.returnPlantId(id);
                    Destroy(gameObject); // destroy small plant
                } 
            }
            
            else if (growth < dna.maxHeight || growth < dna.maxThickness)
            {
                if (!crowded())
                {
                    growth += 0.01f;
                    Resize(); // Grow plant
                }
            }
        }
        // breading
        if (dna.freaquency > 0 && age % dna.freaquency == 0 && !crowded())
        { 
            if (growth > dna.maxHeight * 0.8f)
            {
                UnityEngine.Vector3 babyLocation = transform.position + new UnityEngine.Vector3( Random.Range(-3f, 3f),0f,Random.Range(-3f, 3f));
                spawner.InstanceCreator.SpawnPlant(babyLocation, dna);
            }
        }

        if (age > dna.maxAge)
        {
            // old age filter
            growth -= 0.01f;
            Resize(); // Shrink plant by 1%
            if (growth < 0.1f)
            {
                global.Instance.returnPlantId(id);
                Destroy(gameObject); // Destroy old plant
            }
        }
    }

    public void Resize()
    {
        float xz = growth;
        float y = growth;

        if (growth > dna.maxHeight) y = dna.maxHeight; 
        if (growth > dna.maxThickness) xz = dna.maxThickness;
      
        model.localScale = new UnityEngine.Vector3(xz, y, xz);
        model.localPosition = new UnityEngine.Vector3(0f, y / 2f, 0f);
    }


    public bool crowded()
    {// return true if too crowded
        float radius = model.localScale.x;
        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            radius,
            ~0
        );
        int count = hits.Length;
        return count > 4;
    }


    public void applyDna()
    {
        // apply dna to plant
        plantRenderer.material.color = dna.stemColor;
        //model.localScale = new UnityEngine.Vector3(dna.sizeThickness, dna.sizeHeight, dna.sizeThickness);
    }
}


