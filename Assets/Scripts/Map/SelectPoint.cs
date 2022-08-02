using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectPoint
{
    private List<Node> alreadyNode = new List<Node>();

    public Node GetStartNodePoint(List<Node> nodelist)
    {
        int NodeNum = Random.Range(0, nodelist.Count);
        Node newNode = nodelist[NodeNum];

        newNode.SetStartNode();
        newNode.Getwalkable = true;
        nodelist.Remove(nodelist[NodeNum]);

        alreadyNode.Add(newNode);

        return newNode;
    }

    public Node GetEndNodePoint(List<Node> nodelist,Node existiedNode)
    {
        int nodeNumber = 0;
        Node newnode;
        while (true)
        {
            int NodeNum = Random.Range(0, nodelist.Count);

            bool success = true;

            for(int i = 0; i < existiedNode.neighbournode.Count; i++)
            {
                if(nodelist[NodeNum] == existiedNode.neighbournode[i])
                {
                    success = false;
                    break;
                }
            }

            if (!success) continue;

            nodeNumber = NodeNum;

            break;
        }

        newnode = nodelist[nodeNumber];
        newnode.SetEndNode();
        newnode.Getwalkable = true;
        nodelist.Remove(nodelist[nodeNumber]);

        alreadyNode.Add(newnode);

        return newnode;
    }

    public Node GetCheckNodePoint(List<Node> nodelist)
    {
        Node newnode;
        while (true)
        {
            int nodeNum = Random.Range(0, nodelist.Count);

            bool success = true;

            for(int i = 0; i < alreadyNode.Count; i++)
            {
                for(int j = 0; j < alreadyNode[i].neighbournode.Count; j++)
                {
                    if(nodelist[nodeNum] == alreadyNode[i].neighbournode[j])
                    {
                        success = false;
                        break;
                    }
                }
            }

            if (!success) continue;

            newnode = nodelist[nodeNum];
            break;
        }

        alreadyNode.Add(newnode);
        return newnode;

    }

}
