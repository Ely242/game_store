class Heap
{
    private class Node
    {
        public int Id { get; set; }
        public int Priority{ get; set; }
    }
    private List<Node> heap = new List<Node>();
    private Dictionary<int, int> indexMap = new Dictionary<int, int>();

    private bool compare(Node a, Node b) => a.Priority == b.Priority ? a.Id < b.Id : a.Priority > b.Priority;

    // Used to add a node to the priority queue or update the priority of an existing node
    public void AddOrUpdate(int id, int priority)
    {
        if (indexMap.ContainsKey(id))
        {
            int i = indexMap[id];
            var old = heap[i].Priority;

            heap[i].Priority = priority; // update to new priority

            if (priority > old) HeapifyUp(i); // the node's parent is above
            else HeapifyDown(i);
        }
        else
        {
            // this id isn't in here so we just need to add it to the pq
            var node = new Node { Id = id, Priority = priority };
            heap.Add(node);

            int i = heap.Count - 1;
            indexMap[id] = i;
            HeapifyUp(i);
        }
    }

    public int Pop()
    {
        if (heap.Count == 0)
            return -1;
            
        var root = heap[0]; // root node is at the front of the list

        // When we remove the root node (i.e the highest value) we swap it with the last node (for efficiency) then heapify down to fix the tree
        Swap(0, heap.Count - 1);
        indexMap.Remove(root.Id);

        heap.RemoveAt(heap.Count - 1); // remove the root which is at the end

        if (heap.Count > 0) HeapifyDown(0);

        return root.Id;
    }

    public int Peek() => heap[0].Id;

    private void HeapifyUp(int i)
    {
        while (i > 0)
        {
            int parent = (i - 1) >> 1;

            if (compare(heap[parent], heap[i])) break;

            Swap(i, parent);
            i = parent;
        }
    }

    private void HeapifyDown(int i)
    {
        int n = heap.Count;

        while (true)
        {
            int left = (i << 1) + 1, right = (i << 1) + 2;
            int best = i;

            // we get the child node with the highest priority and move down in that direction
            if (left < n && compare(heap[left], heap[best]))
                best = left;

            if (right < n && compare(heap[right], heap[best]))
                best = right;
            
            if (best == i) break; // the node belongs here

            Swap(i, best);
            i = best;
        }
    }

    private void Swap(int i, int j)
    {
        (heap[i], heap[j]) = (heap[j], heap[i]);
        indexMap[heap[i].Id] = i;
        indexMap[heap[j].Id] = j;
    }

    public bool Contains(int id) => indexMap.ContainsKey(id);

    public int Count => heap.Count;
}