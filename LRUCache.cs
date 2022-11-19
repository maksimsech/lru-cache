namespace LruCacheLeetCode;

public class LRUCache
{
    private readonly int _capacity;
    private readonly Dictionary<int, Node> _map = new ();

    private Node? _firstNode = null;
    private Node? _lastNode = null;


    public int Count => _map.Count;


    public LRUCache(int capacity)
    {
        _capacity = capacity;
    }


    public int Get(int key)
    {
        if (!_map.TryGetValue(key, out var node))
        {
            // As per leetcode requirements
            return -1;
            // throw new KeyNotFoundException($"No element found for key: {key}.");
        }

        PushNodeToStart(node);
        return node.Value;
    }

    public void Put(int key, int value)
    {
        if (_map.TryGetValue(key, out var existingNode))
        {
            existingNode.Value = value;
            PushNodeToStart(existingNode);

            return;
        }

        var node = new Node(key, value);
        _map[key] = node;

        PushNodeToStart(node);

        if (Count <= _capacity)
        {
            return;
        }

        RemoveLastElement();
    }


    private void PushNodeToStart(Node node)
    {
        if (_lastNode == node)
        {
            _lastNode = node.Previous ?? _firstNode;
        }

        if (_firstNode == node)
        {
            return;
        }

        TryLinkNearbyNodes(node);

        var previousFirstNode = _firstNode;
        if (previousFirstNode is not null)
        {
            previousFirstNode.Previous = node;
        }

        node.Previous = null;
        node.Next = _firstNode;
        _firstNode = node;

        if (_lastNode is null)
        {
            _lastNode = node;
        }
        _lastNode.Next = null;
    }

    private void TryLinkNearbyNodes(Node node)
    {
        if (node.Previous is null && node.Next is null)
        {
            return;
        }

        if (node.Previous is not null)
        {
            node.Previous.Next = node.Next;
        }

        if (node.Next is not null)
        {
            node.Next.Previous = node.Previous;
        }

        node.Previous = null;
        node.Next = null;
    }

    private void RemoveLastElement()
    {
        _lastNode = _lastNode?.Previous;

        if (_lastNode is null)
        {
            return;
        }

        _map.Remove(_lastNode.Next!.Key);

        _lastNode.Next = null;
    }


    private class Node
    {
        public int Key { get; }

        public int Value { get; set; }

        public Node? Previous { get; set; }

        public Node? Next { get; set; }

        public Node(int key, int value)
        {
            Key = key;
            Value = value;
        }
    }
}
