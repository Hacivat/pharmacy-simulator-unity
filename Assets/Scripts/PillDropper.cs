using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class PillDropper : MonoBehaviour
{
    public List<Material> materials;
    public List<GameObject> pills;
    public int timesForEachPill;
    public float xScale;
    public float zScale;
    public List<GameObject> allPills = new List<GameObject>();
    void Start () {
        materials = materials.OrderBy(i => Random.value).ToList();
    }

    void ShufflePills() {
        int materialIndex = materials.Count;
        foreach (var pill in pills)
        {
            materialIndex--;
            Color color  =  materials[materialIndex].color;
            for (int i = 0; i < timesForEachPill; i++)
            {
                GameObject newPill = Instantiate(pill);
                newPill = pill;
                newPill.GetComponent<MeshRenderer>().materials[0].color = color;
                newPill.SetActive(false);
                allPills.Add(newPill);
            }
        }
        allPills = allPills.OrderBy(i => Random.value).ToList();
    }

    public void DropPills() {
        float xStart = transform.position.x - (xScale / 2); 
        float xEnd = transform.position.x + (xScale / 2); 
        
        float zStart = transform.position.z - (zScale / 2); 
        float zEnd = transform.position.z + (zScale / 2); 
    
        ShufflePills();
        float tempForPillDistanceX = 0f;
        float tempForPillDistanceZ = 0f;

        foreach (var pill in allPills)
        {
            tempForPillDistanceX = Random.Range(xStart, xEnd);
            tempForPillDistanceZ = Random.Range(zStart, zEnd);
            Vector3 dropPosition = new Vector3(tempForPillDistanceX, transform.position.y, tempForPillDistanceZ);
        
            GameObject newPill = Instantiate(pill, dropPosition, Quaternion.identity);
            newPill.SetActive(true);
        }
    }
}
