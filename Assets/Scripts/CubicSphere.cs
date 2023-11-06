using UnityEngine;

public class CubicSphere : MonoBehaviour
{
    public GameObject cubePrefab;
    public float radius = 10.0f;
    public Vector3 center = Vector3.zero;
    public int subDivision = 10;

    private float lastRadius;
    private int lastSubDiv;
    private Vector3 lastCenter;
    void Start()
    {
        lastRadius = radius;
        lastSubDiv = subDivision;
        lastCenter = center;
        GenerateCubicSphere(center, radius, subDivision);
    }

    void Update()
    {
        if (lastRadius != radius || lastSubDiv != subDivision
            || lastCenter != center)
        {
            DeleteCubicSphere();
            GenerateCubicSphere(center, radius, subDivision);
            lastRadius = radius;
            lastSubDiv = subDivision;
            lastCenter = center;
        }
        
    }

    void DeleteCubicSphere()
    {
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }
    void GenerateCubicSphere(Vector3 center, float radius, int subDivision)
    {
        float voxelSize = 2.0f * radius / subDivision;

        for (float x = center.x - radius; x <= center.x + radius; x += voxelSize)
        {
            for (float y = center.y - radius; y <= center.y + radius; y += voxelSize)
            {
                for (float z = center.z - radius; z <= center.z + radius; z += voxelSize)
                {
                    float distance = Mathf.Sqrt(x * x + y * y + z * z);

                    if (distance <= radius)
                    {
                        Vector3 position = new Vector3(x, y, z);
                        GameObject cube = Instantiate(cubePrefab, position, Quaternion.identity);
                        cube.transform.localScale = new Vector3(voxelSize, voxelSize, voxelSize);
                        cube.transform.SetParent(transform);
                    }
                }
            }
        }
    }
}
