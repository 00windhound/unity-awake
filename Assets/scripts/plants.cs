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
    public Transform trunk;
    public GameObject stickPrefab;
    //public Transform sticks;
    public Renderer plantRenderer;
    public MeshFilter trunkMeshFilter;
    public plantDNA dna;
    public float growth = 0.01f;

    
    

    protected override void Start()
    {
        base.Start();
        plantRenderer = GetComponentInChildren<Renderer>();
        applyDna();
        Resize();
        age = 0;
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
                if (growth < 0.01f)
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
            if (growth < 0.01f)
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
      
        trunk.localScale = new UnityEngine.Vector3(xz, y, xz);
        trunk.localPosition = new UnityEngine.Vector3(0f, y / 1f, 0f);
        // change branches here
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
        return count > 4;
    }


    public void applyDna()
    {
        // apply base color
        plantRenderer.material.color = dna.stemColor;
        
        
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


        // add branches
        for (int i = 0; i < dna.stickCount; i++)
        {
            GameObject newStick = Instantiate(stickPrefab);
            newStick.transform.parent = transform;
        }
    }
}


