using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSc : MonoBehaviour
{
    [SerializeField] private GameObject[] SpringTree = null;
    [SerializeField] private GameObject[] FallTree = null;
    [SerializeField] private GameObject[] WinterTree = null;

    private GameObject springTree = null;
    private GameObject fallTree = null;
    private GameObject winterTree = null;
    private Node node = null;
    public Node SetNode { set => node = value; }

    private List<Node> usablenode = new List<Node>();
    private Queue<Fruit> fruitfromtree = new Queue<Fruit>();
    private Queue<GameObject> obsfromtree = new Queue<GameObject>();

    private List<Fruit> activefruit = new List<Fruit>();

    [SerializeField] private GameObject[] fruit = null;
    [SerializeField] private GameObject[] branch = null;

    WeatherSetting weathersettings = null;
    public WeatherSetting SetWeather { set => weathersettings = value; }


    private bool fall;
    private bool winter;

    private int TreeNum = 0;

    private void Awake()
    {
        activefruit.Clear();

        TreeNum = Random.Range(0, 3);
        springTree = Instantiate(SpringTree[TreeNum], this.transform.position, Quaternion.identity);
        springTree.SetActive(false);
        springTree.transform.parent = this.transform;
        fallTree = Instantiate(FallTree[TreeNum], this.transform.position, Quaternion.identity);
        fallTree.SetActive(false);
        fallTree.transform.parent = this.transform;
        winterTree = Instantiate(WinterTree[TreeNum], this.transform.position, Quaternion.identity);
        winterTree.SetActive(false);
        winterTree.transform.parent = this.transform;

        for (int i = 0; i < 8; i++)
        {
            int RandomNum = Random.Range(0, 2);

            var onj = Instantiate(fruit[RandomNum]).GetComponent<Fruit>();
            onj.gameObject.SetActive(false);
            onj.transform.parent = this.transform;
            fruitfromtree.Enqueue(onj);

            var onj2 = Instantiate(branch[RandomNum]);
            onj2.SetActive(false);
            //onj2.transform.parent = this.transform;
            obsfromtree.Enqueue(onj2);
        }

    }

    public void TreeChangeToFall()
    {
        springTree.SetActive(false);
        fallTree.SetActive(true);
    }
    public void TreeChangeToWinter()
    {
        fallTree.SetActive(false);
        winterTree.SetActive(true);
    }
    public void TreeChangeToSpring()
    {
        winterTree.SetActive(false);
        springTree.SetActive(true);
    }

    public void FallAct()
    {
        int X = GetNeighbour().Count;

        for (int i = 0; i< X; i++)
        {
            int rnadom = Random.Range(0, 2);
            if (rnadom == 0)
            {
                var obj = SetFruit();
                obj.transform.position = new Vector3(usablenode[i].gridX, usablenode[i].GetYDepth / 2, usablenode[i].gridY);
                obj.GetComponent<Fruit>().Setup(new Vector3(usablenode[i].gridX, usablenode[i].GetYDepth / 2, usablenode[i].gridY), this);
                activefruit.Add(obj);
                
            }
        }
    }

    public void WinterAct()
    {
        int X = GetNeighbour().Count;
        Debug.Log(X);

        for (int i = 0; i < X; i++)
        {

            int rnadom = Random.Range(0, 2);
            if (rnadom == 0)
            {

                var obj = SetBranch();
                if (obj != null)
                {
                    obj.transform.position = new Vector3(usablenode[i].gridX, usablenode[i].GetYDepth / 2, usablenode[i].gridY);
                    obj.GetComponent<Obstacle>().SetNode = usablenode[i];
                    usablenode[i].OnBranch();
                }
            }
        }
    }

    private List<Node> GetNeighbour()
    {
        usablenode.Clear();
        for(int i = 0; i < node.neighbournode.Count; i++)
        {
            if (!node.neighbournode[i].GetOnTower&&node.neighbournode[i].GetSetActive&& !node.neighbournode[i].start&& !node.neighbournode[i].end)
                usablenode.Add(node.neighbournode[i]);
        }

        return usablenode;
    }

    public void DisappearFruit()
    {
        for(int i = 0; i < activefruit.Count; i++)
        {
            activefruit[i].ReturnObj();
        }
        activefruit.Clear();

    }

    private Fruit SetFruit()
    {
        var obj = fruitfromtree.Dequeue();
        obj.gameObject.SetActive(true);
        return obj;
    }

    private GameObject SetBranch()
    {
        if (obsfromtree.Count > 0)
        {
            var obj = obsfromtree.Dequeue();
            obj.SetActive(true);
            return obj;
        }
        return null;
    }

    public void ReturnFruit(Fruit _fruit) 
    {
        _fruit.gameObject.SetActive(false);
        fruitfromtree.Enqueue(_fruit);

    }
    public void ReturnBranch(GameObject _branch) 
    {
        _branch.SetActive(false);
        obsfromtree.Enqueue(_branch);
    }


}
