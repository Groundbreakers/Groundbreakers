namespace Core
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     A generic Priority Queue data structure class.
    /// </summary>
    /// <typeparam name="T">
    ///     Usually this should be Node.
    /// </typeparam>
    public class PriorityQueue<T>
        where T : IComparable<T>
    {
        private List<T> data;

        public PriorityQueue()
        {
            this.data = new List<T>();
        }

        public int Count()
        {
            return this.data.Count;
        }

        public T Dequeue()
        {
            // assumes pq is not empty; up to calling code
            var li = this.data.Count - 1; // last index (before removal)
            var frontItem = this.data[0];
            this.data[0] = this.data[li];
            this.data.RemoveAt(li);

            --li; // last index (after removal)
            var pi = 0; // parent index. start at front of pq
            while (true)
            {
                // left child index of parent
                var ci = pi * 2 + 1;
                if (ci > li)
                {
                    // no children so done
                    break;
                }

                var rc = ci + 1; // right child

                // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                if (rc <= li && this.data[rc].CompareTo(this.data[ci]) < 0)
                {
                    ci = rc;
                }

                if (this.data[pi].CompareTo(this.data[ci]) <= 0)
                {
                    // parent is smaller than (or equal to) smallest child so done
                    break;
                }

                var tmp = this.data[pi];
                this.data[pi] = this.data[ci];
                this.data[ci] = tmp;
                pi = ci;
            }

            return frontItem;
        }

        public bool Empty()
        {
            return this.data.Count <= 0;
        }

        public void Enqueue(T item)
        {
            this.data.Add(item);

            // child index; start at end
            var ci = this.data.Count - 1;
            while (ci > 0)
            {
                // parent index
                var pi = (ci - 1) / 2;
                if (this.data[ci].CompareTo(this.data[pi]) >= 0)
                {
                    break; // child item is larger than (or equal) parent so we're done
                }

                var tmp = this.data[ci];
                this.data[ci] = this.data[pi];
                this.data[pi] = tmp;
                ci = pi;
            }
        }

        // temporary
        public List<T> GetData()
        {
            return this.data;
        }

        public override string ToString()
        {
            var s = string.Empty;

            foreach (var t in this.data)
            {
                s += t + " ";
            }

            s += $"count = {this.data.Count}";
            return s;
        }
    }
}