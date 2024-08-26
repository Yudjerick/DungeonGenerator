namespace DungeonGeneration
{
    public class PathfindingNode
    {
        public bool IsSolid { get; set; }
        public bool Explored { get; set; }
        public int X { get; set; }
        public int Z { get; set; }
        public PathfindingNode Previous
        {
            get => previous;
            set
            {
                previous = value;
                if (previous != null)
                {
                    G = value.G + 1;
                }

            }
        }
        private PathfindingNode previous;
        public float H { get; set; }
        public float G { get; private set; }
        public float F => H + G;

        public PathfindingNode(bool isSolid, bool explored, int x, int z, PathfindingNode previous, int h)
        {
            IsSolid = isSolid;
            Explored = explored;
            X = x;
            Z = z;
            Previous = previous;
            H = h;
        }

        public static PathfindingNode StartNode(int x, int z, int h)
        {
            var node = new PathfindingNode(false, true, x, z, null, h);
            node.G = 0;
            return node;
        }

        public static PathfindingNode Solid(int x, int z)
        {
            var node = new PathfindingNode(true, false, x, z, null, int.MaxValue);
            return node;
        }
        public PathfindingNode()
        {
        }
    }
}