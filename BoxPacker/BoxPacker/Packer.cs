using System.Collections.Generic;
using System.Linq;

namespace BoxPacker
{
    public class Packer
    {
        public Packer()
        {
            boxes = new List<Box>();
        }

        public class Node
        {
            public Node right;
            public Node down;
            public float x;
            public float y;
            public float w;
            public float h;
            public bool used;
        }
        
        public class Box
        {
            public float height;
            public float width;
            public float area;
            public Node position;
        }

        public List<Box> boxes;
        private Node rootNode;

        public void AddBox(Box box)
        {
            box.area = box.width * box.height;
            boxes.Add(box);
        }

        public void Pack(float containerWidth, float containerHeight)
        {
            boxes = boxes.OrderByDescending(x => x.area).ToList();
            rootNode = new Node { w = containerWidth, h = containerHeight };

            foreach (var box in boxes)
            {
                var node = FindNode(rootNode, box.width, box.height);
                if (node != null)
                {
                    box.position = SplitNode(node, box.width, box.height);
                } else
                {
                    box.position = GrowNode(box.width, box.height);
                }
            }
        }

        private Node FindNode(Node rootNode, float w, float h)
        {
            if (rootNode.used)
            {
                var nextNode = FindNode(rootNode.right, w, h);

                if (nextNode == null)
                {
                    nextNode = FindNode(rootNode.down, w, h);
                }

                return nextNode;
            }
            else if (w <= rootNode.w && h <= rootNode.h)
            {
                return rootNode;
            }
            else
            {
                return null;
            }
        }

        private Node SplitNode(Node node, float w, float h)
        {
            node.used = true;
            node.down =  new Node { x = node.x,     y = node.y + h, w = node.w,     h = node.h - h  };
            node.right = new Node { x = node.x + w, y = node.y,     w = node.w - w, h = h           };
            return node;
        }

        private Node GrowNode(float w, float h)
        {
            bool canGrowDown = (w <= rootNode.w);
            bool canGrowRight = (h <= rootNode.h);

            bool shouldGrowRight = canGrowRight && (rootNode.h >= (rootNode.w + w));
            bool shouldGrowDown = canGrowDown && (rootNode.w >= (rootNode.h + h));

            if (shouldGrowRight)
            {
                return growRight(w, h);
            } else if (shouldGrowDown)
            {
                return growDown(w, h);
            }
            else if (canGrowRight)
            {
                return growRight(w, h);
            }
            else if (canGrowDown)
            {
                return growDown(w, h);
            } else
            {
                return null;
            }
        }

        private Node growRight(float w, float h)
        {
            rootNode = new Node()
            {
                used = true,
                x = 0,
                y = 0,
                w = rootNode.w + w,
                h = rootNode.h,
                down = rootNode,
                right = new Node() { x = rootNode.w, y = 0, w = w, h = rootNode.h }
            };

            Node node = FindNode(rootNode, w, h);
            if (node!=null)
            {
                return SplitNode(node, w, h);
            }
            else
            {
                return null;
            }
        }

        private Node growDown(float w, float h)
        {
            rootNode = new Node()
            {
                used = true,
                x = 0,
                y = 0,
                w = rootNode.w,
                h = rootNode.h + h,
                down = new Node() { x = 0, y = rootNode.h, w = rootNode.w, h = h },
                right = rootNode
            };
            Node node = FindNode(rootNode, w, h);
            if (node != null)
            {
                return SplitNode(node, w, h);
            }
            else
            {
                return null;
            }
        }
    }
}
