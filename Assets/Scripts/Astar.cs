using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node {
    
    public static float node_step;

    public Vector2 pos;
    public float h;
    public float g;
    public Node parent;
    public Node(Vector2 position, float H, float G, Node Parent) {
        pos = position;
        h = H;
        g = G;
        parent = Parent;
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
    public override bool Equals(object x) {
        if (x == null) return false;
        Node other = (Node)x;
        return (Vector2.Distance(pos, other.pos) < node_step);  
    }
    public bool Equals(Node n) {
        if (n == null) return false;
        return (Vector2.Distance(pos, n.pos) < node_step);
    }
}

public class NodeComparer : IComparer<Node> {
    public int Compare(Node x, Node y){
        float result = ((x.h + x.g) - (y.h + y.g));
        if (result > 0) return 1;
        if (result < 0) return -1;
        return 0;
    }
}


public class Astar : MonoBehaviour {
    List<Node> open;
    List<Node> closed;
    [SerializeField]
    public float gen_step;
	// Use this for initialization
	void Start () {
        Node.node_step = gen_step / 2;
        open = new List<Node>();
        closed = new List<Node>();
    }

    private List<Vector2> BuildPlan(Node current) {
        List<Vector2> path = new List<Vector2>();
        Node parent = current.parent;
        while (parent != null) {
            path.Add(parent.pos);
            parent = parent.parent;
        }
        path.Reverse();
        return path;
    }
    private void UpdateParents(Node expand) {
        open.ForEach(node => {
            if (expand.Equals(node.parent)) {
                node.g = expand.g + Vector2.Distance(node.pos, expand.pos);
                UpdateParents(node);
            }
        });
    }
    
    private void ExpandNode(Node current, Vector2 destination, Vector2 step) {
        //float gen_step = Node.node_step * 2;
        Vector2 new_pos = current.pos + step;
        float distance = Vector2.Distance(new_pos, current.pos);

        //Ray ray = new Ray(current.pos, current.pos - new_pos);
        RaycastHit2D hit = Physics2D.Raycast(current.pos, current.pos - new_pos, distance);
        
        if (hit.collider == null || hit.collider == GetComponent<Collider2D>())
        {
            Node expand = new Node(new_pos, Vector2.Distance(new_pos, destination), current.g + distance, current);
            if (open.Contains(expand))
            {
                Node idx = open.Find( x => (x.Equals(expand)));
                if (idx.g + idx.h > expand.g + expand.h)
                {
                    idx.parent = current;
                    idx.g = expand.g;
                }
            }
            else if (closed.Contains(expand))
            {
                Node idx = closed.Find(x => (x.Equals(expand)));
                if (idx.g + idx.h > expand.g + expand.h)
                {
                    idx.parent = current;
                    idx.g = expand.g;
                    UpdateParents(expand);
                }
            }
            else
            {
                open.Add(expand);
            }
        } else Debug.Log(hit.collider.name);
    }
    public List<Vector2> planToPosition(Vector2 origin, Vector2 destination) {
        Node start = new Node(origin, Vector2.Distance(origin,destination), 0, null);

        open.Add(start);
        Node dest = new Node(destination, 0, 0, null);
        Node current;
        //float gen_step = Node.node_step * 2;
        while (open.Count != 0) {
            current = open[0];
            Debug.Log(current.pos);
            if (current.Equals(dest))
            {
                return BuildPlan(current);
            }

            open.Remove(current);
            closed.Add(current);

            //top left
            ExpandNode(current, destination, new Vector2(-gen_step, gen_step));
            // up
            ExpandNode(current, destination, new Vector2(0, gen_step));
            // top right
            ExpandNode(current, destination, new Vector2(gen_step, gen_step));
            // left
            ExpandNode(current, destination, new Vector2(-gen_step, 0));
            // rigt
            ExpandNode(current, destination, new Vector2(gen_step, 0));
            // bottom left
            ExpandNode(current, destination, new Vector2(-gen_step, -gen_step));
            // down
            ExpandNode(current, destination, new Vector2(0, -gen_step));
            // bottom rigth
            ExpandNode(current, destination, new Vector2(gen_step, -gen_step));
            open.Sort(new NodeComparer());
        }
        // there is no path so it returns an empty list
        return new List<Vector2>();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
