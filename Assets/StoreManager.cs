using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public class StoreManager : MonoBehaviour
{
    public GameObject Plywood;
    public GameObject Floor;
    public float StoreLength;
    public float StoreWidth;
    public List<(float, float, float, float)> subdivisions = new List<(float, float, float, float)>();
    
    private Dictionary<string, GameObject> shelves = new Dictionary<string, GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        List<string> Items = new List<string>() {"Grocery_CocoCrunch", "Grocery_Danisa", "Grocery_iHop", "Grocery_JollyTime", "Grocery_Mamon", "Grocery_Wafer"};
        //BuildPresetStore("Debug", 5);
        int seed = Random.Range(0, 100);
        Debug.Log("Seed: " + seed);
        BuildRandomStore(seed, 20, 20, 4.8f, 0.45f, 0.4f, 0.02f, 1, 4, 3, 0.5f);
        //FillShelves(270, 270, Items);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void BuildPresetStore(string Preset, int level)
    {
        float ShelfLength;
        float ShelfWidth;
        float ShelfHeight;
        float ShelfThickness;
        int ShelfLevel;
        switch (Preset)
        {
            case "Debug":
                // 1 shelf for testing
                ShelfLength = 3f;
                ShelfWidth = 0.45f;
                ShelfHeight = 0.4f;
                ShelfThickness = 0.02f;
                ShelfLevel = 4;
                SpawnRoom(5f, 5f);
                SpawnShelf("Shelf_Debug", 5f/2f, 5f/2f, ShelfLength-(2*(ShelfWidth+ShelfThickness)), ShelfWidth, ShelfHeight, ShelfThickness, ShelfLevel, 0);
                break;
            // case "Small":
            //     // 12m x 12m store
            //     // 4.8m x 0.45m x 0.4m shelf space * level
            //     length = 4.8f;
            //     width = 0.45f;
            //     height = 0.4f;
            //     thickness = 0.02f;
            //     lengthSide = (width+thickness)*2;

            //     SpawnRoom(0.8f, 1.2f);

            //     for(int i=-3; i<3; i++)
            //     {
            //         SpawnShelf("Shelf1a", 0, i*(2*(width+thickness)+1)+(0.5f*(width+thickness)+1)-0.5f, length, width, height, thickness, level, 0);
            //         SpawnShelf("Shelf1b", 0, i*(2*(width+thickness)+1)+(1.5f*(width+thickness)+thickness+1)-0.5f, length, width, height, thickness, level, 180);
            //         SpawnShelf("Shelf1c", -(thickness+length+width)/2, i*(2*(width+thickness)+1)+(1+width+thickness)-0.50f+thickness/2, lengthSide, width, height, thickness, level, 90);
            //         SpawnShelf("Shelf1d", (thickness+length+width)/2, i*(2*(width+thickness)+1)+(1+width+thickness)-0.50f+thickness/2, lengthSide, width, height, thickness, level, 270);
            //     }
        
            //     break;
            default:
                Debug.LogError("Preset not found.");
                break;
        }
    }

    public void BuildRandomStore(int seed, float StoreLength, float StoreWidth, float ShelfLength, float ShelfWidth, float ShelfHeight, float ShelfThickness, float ShelfDistance, int ShelfLevel, int division, float variation)
    {
        // Randomly generate a store layout
        // Lagay minimum and maximum shelf lwh
        // Variation determines how much the store layout can vary, float from 0 to 1

        Random.InitState(seed);
        SpawnRoom(StoreLength, StoreWidth);
        DivideRoom(StoreLength, StoreWidth, division, 2*(ShelfWidth+ShelfThickness), ShelfDistance);  

        foreach ((float Length, float Width, float CenterX, float CenterY) subdivision in subdivisions)
        {
            int Randomizer = Random.Range(0, 2);
            if (Randomizer == 0)
            { 
                int Rows = Mathf.FloorToInt(subdivision.Width / (2*(ShelfWidth + ShelfThickness) + ShelfDistance));

                Debug.Log("Rows: " + Rows);
                Debug.Log("Min Length: " + Mathf.FloorToInt(subdivision.Length / (4*(ShelfWidth + ShelfThickness) + ShelfDistance)));
                for (int i = 0; i < Rows; i++)
                {
                    int Gaps = Random.Range(0, Mathf.FloorToInt(subdivision.Length / (4*(ShelfWidth + ShelfThickness) + ShelfDistance)));
                    // Use the variable 'Gaps' or perform some operations here
                    ShelfLength = (subdivision.Length-((2+Gaps)*ShelfDistance))/(Gaps+1);
                    Debug.Log("Row "+ (i+1)+"\nGaps: " + Gaps);
                    Debug.Log("Shelf Length: " + ShelfLength);
                    for (int j = 0; j <= Gaps; j++)

                    {
                        float shelfX = subdivision.CenterX - (subdivision.Length / 2) + j * (ShelfDistance + 2 * (ShelfWidth + ShelfThickness) + ShelfLength - (2 * (ShelfWidth + ShelfThickness))) + ShelfDistance + ShelfWidth + ShelfThickness + (ShelfLength - (2 * (ShelfWidth + ShelfThickness))) / 2;
                        float shelfZ = i * (2 * (ShelfWidth + ShelfThickness) + 1) + (0.5f * (ShelfWidth + ShelfThickness) + 1) - 0.5f;
                        SpawnShelf("Shelf_" + subdivisions.IndexOf(subdivision) + i + j + "_f", shelfX, shelfZ, ShelfLength - (2 * (ShelfWidth + ShelfThickness)), ShelfWidth, ShelfHeight, ShelfThickness, ShelfLevel, 0);
                        SpawnShelf("Shelf_" + subdivisions.IndexOf(subdivision) + i + j + "_b", shelfX, shelfZ + (ShelfWidth + ShelfThickness), ShelfLength - (2 * (ShelfWidth + ShelfThickness)), ShelfWidth, ShelfHeight, ShelfThickness, ShelfLevel, 180);
                        SpawnShelf("Shelf_" + subdivisions.IndexOf(subdivision) + i + j + "_l", shelfX - (ShelfThickness + ShelfLength - (2 * (ShelfWidth + ShelfThickness)) + ShelfWidth) / 2, shelfZ + (ShelfWidth + ShelfThickness) / 2, (ShelfWidth + ShelfThickness) * 2, ShelfWidth, ShelfHeight, ShelfThickness, ShelfLevel, 90);
                        SpawnShelf("Shelf_" + subdivisions.IndexOf(subdivision) + i + j + "_r", shelfX + (ShelfThickness + ShelfLength - (2 * (ShelfWidth + ShelfThickness)) + ShelfWidth) / 2, shelfZ + (ShelfWidth + ShelfThickness) / 2, (ShelfWidth + ShelfThickness) * 2, ShelfWidth, ShelfHeight, ShelfThickness, ShelfLevel, 270);
                    }
                } 
            }
            else if (Randomizer == 1)
            {

                int Rows = Mathf.FloorToInt(subdivision.Length / (2*(ShelfWidth + ShelfThickness) + ShelfDistance));

                Debug.Log("Rows: " + Rows);
                Debug.Log("Min Length: " + Mathf.FloorToInt(subdivision.Width / (4*(ShelfWidth + ShelfThickness) + ShelfDistance)));
                for (int i = 0; i < Rows; i++)
                {
                    int Gaps = Random.Range(0, Mathf.FloorToInt(subdivision.Width / (4*(ShelfWidth + ShelfThickness) + ShelfDistance)));
                    // Use the variable 'Gaps' or perform some operations here
                    ShelfLength = (subdivision.Width-((2+Gaps)*ShelfDistance))/(Gaps+1);
                    Debug.Log("Row "+ (i+1)+"\nGaps: " + Gaps);
                    Debug.Log("Shelf Length: " + ShelfLength);
                    for (int j = 0; j <= Gaps; j++)

                    {
                        float shelfX = subdivision.CenterY - (subdivision.Width / 2) + j * (ShelfDistance + 2 * (ShelfWidth + ShelfThickness) + ShelfLength - (2 * (ShelfWidth + ShelfThickness))) + ShelfDistance + ShelfWidth + ShelfThickness + (ShelfLength - (2 * (ShelfWidth + ShelfThickness))) / 2;
                        float shelfZ = subdivision.CenterX - (subdivision.Length / 2) + i * (2 * (ShelfWidth + ShelfThickness) + 1) + (0.5f * (ShelfWidth + ShelfThickness) + 1) - 0.5f;
                        SpawnShelf("Shelf_" + subdivisions.IndexOf(subdivision) + i + j + "_f", shelfZ, shelfX, ShelfLength - (2 * (ShelfWidth + ShelfThickness)), ShelfWidth, ShelfHeight, ShelfThickness, ShelfLevel, 90);
                        SpawnShelf("Shelf_" + subdivisions.IndexOf(subdivision) + i + j + "_b", shelfZ + (ShelfWidth + ShelfThickness), shelfX, ShelfLength - (2 * (ShelfWidth + ShelfThickness)), ShelfWidth, ShelfHeight, ShelfThickness, ShelfLevel, 270);
                        SpawnShelf("Shelf_" + subdivisions.IndexOf(subdivision) + i + j + "_l", shelfZ + (ShelfWidth + ShelfThickness) / 2, shelfX - (ShelfThickness + ShelfLength - (2 * (ShelfWidth + ShelfThickness)) + ShelfWidth) / 2, (ShelfWidth + ShelfThickness) * 2, ShelfWidth, ShelfHeight, ShelfThickness, ShelfLevel, 0);
                        SpawnShelf("Shelf_" + subdivisions.IndexOf(subdivision) + i + j + "_r", shelfZ + (ShelfWidth + ShelfThickness) / 2, shelfX + (ShelfThickness + ShelfLength - (2 * (ShelfWidth + ShelfThickness)) + ShelfWidth) / 2, (ShelfWidth + ShelfThickness) * 2, ShelfWidth, ShelfHeight, ShelfThickness, ShelfLevel, 180);
                    }
                } 
            }            
        }


    }

    public void FillShelves(int seed, float rotation, List<string> Items)
    {
        int pseudoseed = 0;
        foreach (var shelfEntry in shelves)
        {
            string shelfId = shelfEntry.Key;
            SpawnProducts(seed + pseudoseed, shelfId, rotation, Items);
            pseudoseed++;
        }
    }

    public void SpawnRoom(float length, float width)
    {
        GameObject StoreFloor = null;
        StoreLength = length;
        StoreWidth = width;
        StoreFloor = Instantiate(Floor, new Vector3(length / 2, 0, width / 2), Quaternion.identity);
        StoreFloor.transform.localScale = new Vector3(length/10, 1, width/10);
    }

    public void SpawnShelf(string ShelfId, float x, float z, float length, float width, float height, float thickness, int level, float rotation)
    {
        List<GameObject> Panels = new List<GameObject>();

        for(int i=0; i<level; i++)
        {
            Vector3 BottomPanelPosition = new Vector3(0, thickness/2+(height+thickness)*i, 0);
            Vector3 BottomPanelScale = new Vector3(length, thickness, width);
            GameObject BottomPanel = Instantiate(Plywood, BottomPanelPosition, Quaternion.identity);
            BottomPanel.transform.localScale = BottomPanelScale;
            Panels.Add(BottomPanel);
        }
        /*
        Vector3 LeftPanelPosition = new Vector3(x-1*length/2-thickness/2, ((height+thickness)*level+thickness)/2, z+thickness/2);
        Vector3 LeftPanelScale = new Vector3(thickness, (height+thickness)*level+thickness, width+thickness);
        GameObject LeftPanel = Instantiate(Plywood, LeftPanelPosition, Quaternion.identity);
        LeftPanel.transform.localScale = LeftPanelScale;
        Panels.Add(LeftPanel);

        Vector3 RightPanelPosition = new Vector3(x+length/2+thickness/2, ((height+thickness)*level+thickness)/2, z+thickness/2);
        Vector3 RightPanelScale = new Vector3(thickness, (height+thickness)*level+thickness, width+thickness);
        GameObject RightPanel = Instantiate(Plywood, RightPanelPosition, Quaternion.identity);
        RightPanel.transform.localScale = RightPanelScale;
        Panels.Add(RightPanel);
        */
        Vector3 BackPanelPosition = new Vector3(0, ((height+thickness)*level+thickness)/2, width/2 + thickness/2);
        Vector3 BackPanelScale = new Vector3(length, (height+thickness)*level+thickness, thickness);
        GameObject BackPanel = Instantiate(Plywood, BackPanelPosition, Quaternion.identity);
        BackPanel.transform.localScale = BackPanelScale;
        Panels.Add(BackPanel);

        List<CombineInstance> combine = new List<CombineInstance>();

        foreach (GameObject obj in Panels)
        {
            MeshFilter meshFilter = obj.GetComponent<MeshFilter>();
            if (meshFilter != null)
            {
                CombineInstance ci = new CombineInstance();
                ci.mesh = meshFilter.sharedMesh;
                ci.transform = obj.transform.localToWorldMatrix;
                combine.Add(ci);
                obj.SetActive(false);
                Destroy(obj);
            }
        }
        GameObject NewShelf = new GameObject("NewShelf");
        MeshFilter ShelfMeshFilter = NewShelf.AddComponent<MeshFilter>();
        MeshRenderer ShelfMeshRenderer = NewShelf.AddComponent<MeshRenderer>();

        NewShelf.transform.position = new Vector3(x, 0, z);
        NewShelf.transform.rotation = Quaternion.Euler(0, rotation, 0);
        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine.ToArray(), true, true);
        ShelfMeshFilter.mesh = combinedMesh;

        MeshCollider ShelfMeshCollider = NewShelf.AddComponent<MeshCollider>();
        ShelfMeshCollider.sharedMesh = combinedMesh;

        ShelfMeshRenderer.materials = Panels[0].GetComponent<MeshRenderer>().materials;

        Shelf TaggedShelf = NewShelf.AddComponent<Shelf>();
        TaggedShelf.id = ShelfId;
        TaggedShelf.length = length;
        TaggedShelf.width = width;
        TaggedShelf.height = height;
        TaggedShelf.thickness = thickness;
        TaggedShelf.level = level;
        TaggedShelf.rotation = rotation;

        //Debug.Log("Shelf spawned. ID: " + ShelfId);
        shelves[ShelfId] = NewShelf;
    }
    
    public void SpawnProducts(int seed, string ShelfId, float ProductOrientation, List<string> Items)
    {
        List<GameObject> Products = new List<GameObject>();
        foreach (string item in Items)
        {
            GameObject obj = Resources.Load<GameObject>("Products/" + item);
            if (obj != null)
            {
                Products.Add(obj);
            }
            else
            {
                Debug.LogError("Product not found: " + item);
            }
        }

        Random.InitState(seed);
        GameObject shelf = shelves[ShelfId];
        float length = shelf.GetComponent<Shelf>().length;
        float width = shelf.GetComponent<Shelf>().width;
        float height = shelf.GetComponent<Shelf>().height;
        float thickness = shelf.GetComponent<Shelf>().thickness;
        int level = shelf.GetComponent<Shelf>().level;
        Debug.Log("Shelf length: " + length + ", Shelf width: " + width + ", Shelf level: " + level);

        // Process the cached items
        if (Products == null || Products.Count == 0)
        {
            Debug.LogError("No items available to spawn.");
            return;
        }
        Transform shelfTransform = shelf.transform;
        Vector3 shelfRight = shelfTransform.right;  // Use the local right direction
        Vector3 shelfForward = shelfTransform.forward;  // Use the local forward direction based on current rotation
        Vector3 shelfLeftPosition = shelfTransform.position - (shelfRight * (length / 2));  // Calculate the leftmost position of the shelf
        float shelfDownY = shelfTransform.position.y + (shelfTransform.localScale.y / (level+1));  // Calculate the bottom y position of the shelf
        Vector3 shelfBackPosition = shelfTransform.position - (shelfForward * (width / 2));
        Vector3 shelfLeftBackPosition = shelfBackPosition - (shelfRight * (length / 2));  // Calculate the left backmost position of the shelf
        float buffer = 0.01f;  // Buffer space between items

        Debug.Log($"Forward vector: {shelfForward}, Right vector: {shelfRight}, Left position: {shelfLeftPosition}, Down y: {shelfDownY}, Back position: {shelfBackPosition}");
        for(int i = 0; i<level; i++)
        {
            float spaceTaken = 0;
            float spaceTakenDepth = 0;
            List<(GameObject, float)> firstRowItems = new List<(GameObject, float)>();

            while (spaceTaken < length)
            {
                GameObject item = Products[Random.Range(0, Products.Count)];
                MeshRenderer itemRenderer = item.GetComponent<MeshRenderer>();
                float itemLength;
                float itemDepth;
                // Adjust item length and depth based on ProductOrientation
                if (ProductOrientation == 90 || ProductOrientation == 270)
                {
                    itemLength = itemRenderer.bounds.size.z;
                    itemDepth = itemRenderer.bounds.size.x;
                }
                else if (ProductOrientation == 0 || ProductOrientation == 180)
                {
                    itemLength = itemRenderer.bounds.size.x;
                    itemDepth = itemRenderer.bounds.size.z;
                }
                else
                {
                    Debug.LogError("Invalid ProductOrientation.");
                    return;
                }
                Debug.Log($"Item: {item.name}, Length: {itemLength}, Depth: {itemDepth}");

                if (spaceTaken + itemLength <= length)
                {
                    // Calculate position for the item
                    Vector3 position = shelfLeftBackPosition + (shelfRight * (spaceTaken + (itemLength / 2) + buffer));
                    position.y = i*(height+thickness)+shelfDownY;  // Use the calculated bottom y position of the shelf
                    position += shelfForward * (itemDepth / 2);  // Adjust position based on item depth

                    // Create a rotation of -90 degrees on the y-axis, limited to the shelf's y-axis rotation
                    Quaternion rotation = Quaternion.LookRotation(shelfForward, Vector3.up) * Quaternion.Euler(0, ProductOrientation, 0);

                    // Instantiate the item with the calculated position and rotation
                    GameObject instantiatedItem = Instantiate(item, position, rotation);
                    instantiatedItem.tag = "Grippable";
                    firstRowItems.Add((instantiatedItem, itemDepth));
                    Debug.Log($"Placed {item.name} at position: {position} with length {itemLength}");

                    // Move to the next position
                    spaceTaken += itemLength + buffer;
                }
                else
                {
                    spaceTaken += itemLength;
                    Debug.Log($"Item {item.name} does not fit on the shelf, stopping arrangement.");
                    break;
                }
            }

            // Copy the first row items to fill the depth of the shelf
            foreach (var (item, itemDepth) in firstRowItems)
            {
                spaceTakenDepth = itemDepth + buffer;

                while (spaceTakenDepth < width)
                {
                    if (spaceTakenDepth + itemDepth <= width)
                    {
                        Vector3 position = item.transform.position + (shelfForward * spaceTakenDepth);
                        Quaternion rotation = item.transform.rotation;

                        Instantiate(item, position, rotation);
                        Debug.Log($"Copied {item.name} to position: {position}");

                        spaceTakenDepth += itemDepth + buffer;
                    }
                    else
                    {
                        spaceTakenDepth += itemDepth;
                    }
                }
            }
        }
    }

    public void DestroyAllProducts()
    {
        GameObject[] products = GameObject.FindGameObjectsWithTag("Grippable");
        foreach (GameObject product in products)
        {
            Destroy(product);
        }
    }

    public void DestroyAllShelves()
    {
        // Find all GameObjects tagged as "Shelf"
        Shelf[] allShelves = FindObjectsOfType<Shelf>();
        foreach (Shelf shelf in allShelves)
        {
            Destroy(shelf.gameObject);
        }

        // Clear the shelves dictionary
        shelves.Clear();
    }

    public void DestroyRoom()
    {
        GameObject floor = GameObject.Find("Floor");
        if (floor != null)
        {
            Destroy(floor);
        }
        DestroyAllProducts();
        DestroyAllShelves();
        StoreLength = 0;
        StoreWidth = 0;
        subdivisions.Clear();
    }

    private void DivideRoom(float length, float width, int division, float MaxShelfWidth, float ShelfDistance)
    {
        subdivisions.Clear(); // Clear existing subdivisions
        subdivisions.Add((length, width, length/2, width/2));
        List<(float, float, float, float)> LuckyList = new List<(float, float, float, float)>(subdivisions);
        for(int i=0; i<division; i++)
        {
            if(LuckyList.Count == 0)
            {
                Debug.Log("\nNo more subdivisions to divide.");
                Debug.Log("\nSubdivisions: " + subdivisions.Count);
            }

            (float, float, float, float) Lucky = LuckyList[Random.Range(0, LuckyList.Count)];
            (float, float, float, float) Lucky1 = (0, 0, 0, 0);
            (float, float, float, float) Lucky2 = (0, 0, 0, 0);
            int randomizer = Random.Range(0, 1);

            if (Lucky.Item1 >= 2*(MaxShelfWidth + 2 * ShelfDistance) && Lucky.Item2 >= 2*(MaxShelfWidth + 2 * ShelfDistance))
            {
                if(randomizer == 0)
                {
                    Lucky1 = (Lucky.Item1/2, Lucky.Item2, Lucky.Item3-Lucky.Item1/4, Lucky.Item4);
                    Lucky2 = (Lucky.Item1/2, Lucky.Item2, Lucky.Item3+Lucky.Item1/4, Lucky.Item4);
                }
                else
                {
                    Lucky1 = (Lucky.Item1, Lucky.Item2/2, Lucky.Item3, Lucky.Item4-Lucky.Item2/4);
                    Lucky2 = (Lucky.Item1, Lucky.Item2/2, Lucky.Item3, Lucky.Item4+Lucky.Item2/4);
                }
            }
            else if (Lucky.Item1 >= 2*(MaxShelfWidth + 2 * ShelfDistance))
            {
                Lucky1 = (Lucky.Item1/2, Lucky.Item2, Lucky.Item3-Lucky.Item1/4, Lucky.Item4);
                Lucky2 = (Lucky.Item1/2, Lucky.Item2, Lucky.Item3+Lucky.Item1/4, Lucky.Item4); 
            }
            else if (Lucky.Item2 >= 2*(MaxShelfWidth + 2 * ShelfDistance))
            {
                Lucky1 = (Lucky.Item1, Lucky.Item2/2, Lucky.Item3, Lucky.Item4-Lucky.Item2/4);
                Lucky2 = (Lucky.Item1, Lucky.Item2/2, Lucky.Item3, Lucky.Item4+Lucky.Item2/4); 
            }

            subdivisions.Add(Lucky1);
            subdivisions.Add(Lucky2);
            subdivisions.Remove(Lucky);

            if(Lucky1.Item1 >= 2*(MaxShelfWidth + 2 * ShelfDistance) || Lucky1.Item2 >= 2*(MaxShelfWidth + 2 * ShelfDistance))
            {
                LuckyList.Add(Lucky1);
                LuckyList.Add(Lucky2);
            }

            LuckyList.Remove(Lucky);

        }
    int counter = 0;
    foreach (var subdivision in subdivisions)
        {
            Debug.Log(counter+$"Subdivision - Length: {subdivision.Item1}, Width: {subdivision.Item2}, CenterX: {subdivision.Item3}, CenterY: {subdivision.Item4}");
            counter++;
        }
    }

    private static Vector2 RotatePoint(Vector2 point, Vector2 pivot, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cosTheta = Mathf.Cos(rad);
        float sinTheta = Mathf.Sin(rad);

        float dx = point.x - pivot.x;
        float dy = point.y - pivot.y;

        float newX = pivot.x + cosTheta * dx - sinTheta * dy;
        float newY = pivot.y + sinTheta * dx + cosTheta * dy;

        return new Vector2(newX, newY);
    }
}
