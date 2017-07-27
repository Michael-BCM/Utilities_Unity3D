using UnityEngine;

public class GlassTileParticles : MonoBehaviour
{
    [SerializeField]
    GameObject prefab;
    bool canSpawn = false;

    private float xRotation = 40F, yRotation = 40F, zRotation = 40F;
    private float rotateSpeedLimit = 200F;
    private float rotateIntv = 4F;

    [SerializeField]
    private Color cubeColor;

    private void Start()
    {
        InvokeRepeating("changeX", 0, 0.1F);
        InvokeRepeating("changeY", 0, 0.1F);
        InvokeRepeating("changeZ", 0, 0.1F);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            canSpawn = !canSpawn;

        SpawnCube();
        transform.Rotate(new Vector3(xRotation, yRotation, zRotation) * Time.deltaTime, Space.Self);
    }

    void changeRotation(ref float rotationDegree)
    {
        if (rotationDegree > -rotateSpeedLimit && rotationDegree < rotateSpeedLimit)
            rotationDegree += Random.Range(-rotateIntv, rotateIntv);

        else if (rotationDegree <= -rotateSpeedLimit)
            rotationDegree += rotateIntv;

        else if (rotationDegree >= rotateSpeedLimit)
            rotationDegree -= rotateIntv;

    }

    void changeX() { changeRotation(ref xRotation); }
    void changeY() { changeRotation(ref yRotation); }
    void changeZ() { changeRotation(ref zRotation); }

    void SpawnCube()
    {
        if (!canSpawn)
        {
            return;
        }
        GameObject cube = Instantiate(prefab, transform.position, Quaternion.Euler(0, Random.Range(0, 90F), 0));
        cube.transform.localScale = new Vector3(Random.Range(0, 1F), Random.Range(0, 1F), Random.Range(0, 1F));

        if (cube.transform.localScale.x > 0.2 && cube.transform.localScale.y > 0.2 && cube.transform.localScale.z > 0.2)
            Destroy(cube);

        Renderer r = cube.GetComponent<Renderer>();
        r.material.color = cubeColor;
        r.material.SetFloat("_Metallic", Random.Range(0.7F, 1F));
        r.material.SetFloat("_Glossiness", Random.Range(0.7F, 1F));
        Destroy(cube, 10F);
    }
}