using System.Collections.Generic;
using UnityEngine;

public class CubicSphere : MonoBehaviour
{
    [System.Serializable]
    public class PotentialCube
    {
        public GameObject cubeObject;
        public float potential;
    }
    public float threshold = 0.5f;
    public float initialPotential = 1.0f; // Potentiel initial pour la sphère
    public List<PotentialCube> cubes;

    [System.Serializable]
    public class SphereData
    {
        public Vector3 position;
        public float radius;
    }

    public enum OperationType { Union, Intersection }

    public List<SphereData> spheres;
    public GameObject boundingBoxPrefab;
    public GameObject cubePrefab;
    public float cubeSize = 1f; // Taille fixée par l'utilisateur
    public OperationType operation = OperationType.Union;


    void Start()
    {
        GenerateVolume();
    }

    

    void GenerateVolume()
    {
        Vector3 minPoint = Vector3.positiveInfinity;
        Vector3 maxPoint = Vector3.negativeInfinity;

        // Calcul de la boîte englobante
        foreach (SphereData sphere in spheres)
        {
            minPoint = Vector3.Min(minPoint, sphere.position - Vector3.one * sphere.radius);
            maxPoint = Vector3.Max(maxPoint, sphere.position + Vector3.one * sphere.radius);
        }

        Vector3 BoundingBoxCenter = (minPoint + maxPoint) / 2;
        Vector3 BoundingBoxSize = maxPoint - minPoint;

        int gridSizeX = Mathf.CeilToInt(BoundingBoxSize.x / cubeSize);
        int gridSizeY = Mathf.CeilToInt(BoundingBoxSize.y / cubeSize);
        int gridSizeZ = Mathf.CeilToInt(BoundingBoxSize.z / cubeSize);


        // Instantiation du cube qui represente la boîte englobante
        GameObject boundingBox = Instantiate(boundingBoxPrefab, BoundingBoxCenter, Quaternion.identity);
        boundingBox.transform.localScale = BoundingBoxSize;

        // Création des spheres
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int z = 0; z < gridSizeZ; z++)
                {
                    Vector3 pos = minPoint + new Vector3(x, y, z) * cubeSize;
                    if (IsInsideSpheres(pos))
                    {
                        GameObject voxelGO = Instantiate(cubePrefab, pos, Quaternion.identity);
                        voxelGO.transform.parent = boundingBox.transform;
                    }
                }
            }
        }
    }

    bool IsInsideSpheres(Vector3 point)
    {
        bool insideAnySphere = false;
        bool insideAllSpheres = true;

        foreach (SphereData sphere in spheres)
        {
            float distanceToCenter = (sphere.position - point).magnitude;
            bool insideThisSphere = distanceToCenter < sphere.radius;

            if (insideThisSphere)
                insideAnySphere = true;
            else
                insideAllSpheres = false;
        }

        return operation == OperationType.Union ? insideAnySphere : insideAllSpheres;
    }

}
