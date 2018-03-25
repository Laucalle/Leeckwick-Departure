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
    float gen_step;
    public float ray_factor;

    public LayerMask blockingLayer;

	// Use this for initialization
	void Awake () {
        open = new List<Node>();
        closed = new List<Node>();
    }

    public void InitVars(float g_step)
    {
        gen_step = g_step;
        Node.node_step = gen_step / 2;
    }

    private List<Vector2> BuildPlan(Node current) {
        List<Vector2> path = new List<Vector2>();
        Node parent = current.parent;
        while (parent != null) {
            path.Add(parent.pos);
            parent = parent.parent;
        }
        path.Reverse();
        open.Clear();
        closed.Clear();
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
    private List<KeyValuePair<bool, Vector2>> TryRay(Node current, float step) {
        //hit.collider == null || hit.collider == GetComponent<Collider2D>() || hit.collider.name == "Player"

        List<KeyValuePair<bool, Vector2>> result = new List<KeyValuePair<bool, Vector2>>();

        Vector2 vstep = new Vector2(-step,step);
        float distance_diag = Vector2.Distance( current.pos, vstep + current.pos);

        RaycastHit2D hit = Physics2D.Linecast(current.pos, current.pos + (vstep*ray_factor), blockingLayer);
        result.Add(new KeyValuePair<bool, Vector2>(hit.collider == null, current.pos + vstep));
        //if (!result[result.Count-1].Key) Debug.Log(hit.collider.name);

        vstep = new Vector2(0, step);
        float distance_str = Vector2.Distance( current.pos, vstep + current.pos);

        hit = Physics2D.Linecast(current.pos, current.pos + (vstep * ray_factor), blockingLayer);
        result.Add(new KeyValuePair<bool, Vector2>(hit.transform == null , current.pos + vstep));
        //if (!result[result.Count - 1].Key) Debug.Log(hit.collider.name);

        vstep = new Vector2(step, step);
        hit = Physics2D.Linecast(current.pos, current.pos + (vstep * ray_factor), blockingLayer);
        result.Add(new KeyValuePair<bool, Vector2>(hit.transform == null, current.pos + vstep));
        //if (!result[result.Count - 1].Key) Debug.Log(hit.collider.name);

        vstep = new Vector2(step, 0);
        distance_str = Vector2.Distance(current.pos, vstep + current.pos);

        hit = Physics2D.Linecast(current.pos, current.pos + (vstep * ray_factor), blockingLayer);
        result.Add(new KeyValuePair<bool, Vector2>(hit.transform == null , current.pos + vstep));
        //if (!result[result.Count - 1].Key) Debug.Log(hit.collider.name);

        vstep = new Vector2(step, -step);
        hit = Physics2D.Linecast(current.pos, current.pos + (vstep * ray_factor), blockingLayer);
        result.Add(new KeyValuePair<bool, Vector2>(hit.transform == null, current.pos + vstep));
        //if (!result[result.Count - 1].Key) Debug.Log(hit.collider.name);

        vstep = new Vector2(0, -step);

        distance_str = Vector2.Distance( current.pos, vstep + current.pos);
        hit = Physics2D.Linecast(current.pos, current.pos + (vstep * ray_factor), blockingLayer);
        result.Add(new KeyValuePair<bool, Vector2>(hit.transform == null, current.pos + vstep));
        //if (!result[result.Count - 1].Key) Debug.Log(hit.collider.name);

        vstep = new Vector2(-step,-step);

        hit = Physics2D.Linecast(current.pos, current.pos + (vstep * ray_factor), blockingLayer);
        result.Add(new KeyValuePair<bool, Vector2>(hit.transform == null, current.pos + vstep));
        //if (!result[result.Count - 1].Key) Debug.Log(hit.collider.name);

        vstep = new Vector2(-step, 0);

        distance_str = Vector2.Distance(current.pos, vstep + current.pos);
        hit = Physics2D.Linecast(current.pos, current.pos + (vstep * ray_factor), blockingLayer);
        result.Add(new KeyValuePair<bool, Vector2>(hit.transform == null, current.pos + vstep));
        //if (!result[result.Count - 1].Key) Debug.Log(hit.collider.name);
 
        foreach (KeyValuePair<bool, Vector2> pair in result) {
            if (pair.Key)
            {
                //Debug.DrawLine(current.pos, pair.Value, Color.blue, 3f);
            }
            else {
                //Debug.DrawLine(current.pos, pair.Value, Color.red, 3f);
            }
        }

        return result;
    }
    
    private void ExpandNode(Node current, Vector2 destination, float step) {

        List<KeyValuePair<bool, Vector2>> possible_nodes = TryRay(current, step);

        for (int i = 1; i <= possible_nodes.Count; i++) {
            if (possible_nodes[i % possible_nodes.Count].Key && possible_nodes[(i + 1) % possible_nodes.Count].Key && possible_nodes[(i - 1) % possible_nodes.Count].Key) {
                float distance = Vector2.Distance(possible_nodes[i % possible_nodes.Count].Value, current.pos);
                Node expand = new Node(possible_nodes[i % possible_nodes.Count].Value, Vector2.Distance(possible_nodes[i % possible_nodes.Count].Value, destination), current.g + distance, current);
                //Debug.DrawLine(current.pos, possible_nodes[i % possible_nodes.Count].Value, Color.blue, 3f);
                if (open.Contains(expand))
                {
                    Node idx = open.Find(x => (x.Equals(expand)));
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
            }
        }
    }

    void RemoveWorseThan(float threshold) {
        for (int i = 0; i < open.Count; i++) {
            if (open[i].g + open[i].h > threshold) {
                open.RemoveRange(i, open.Count - i);
                Debug.Log(open.Count);
                break;
            }
        }
    }

    public List<Vector2> planToPosition(Vector2 origin, Vector2 destination, float fat_dot) {
        Node start = new Node(origin, Vector2.Distance(origin,destination), 0, null);
        Node.node_step = gen_step / 4;
        if (fat_dot < gen_step) fat_dot = gen_step;
        open.Add(start);
        Node dest = new Node(destination, 0, 0, null);
        Node current;
        float best_fit;
        while (open.Count != 0) {
            best_fit = open[0].g + open[0].h;
            //Debug.Log("mejor f: "+ best_fit+", umbral: " + best_fit * 3f);
            RemoveWorseThan(best_fit * 3f);

            current = open[0];
            /*if (current.Equals(dest))
            {
                return BuildPlan(current);
            }*/
            if (Vector2.Distance(destination, current.pos) < fat_dot)
            {
                return BuildPlan(current);
            }
            
            open.Remove(current);
            closed.Add(current);

            ExpandNode(current, destination,  gen_step);
           
            open.Sort(new NodeComparer());
            
        }

        //List<Vector2> result = BuildPlan(current);
        open.Clear();
        closed.Clear();
        return new List<Vector2>();
    }

	// Update is called once per frame
	void Update () {
		
	}
}
