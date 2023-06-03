using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DHT01;

namespace Experience
{
    class Program
    {
        class Node
        {
            public int id;
            public HashSet<string> bucket = new HashSet<string>();

            public void AddKey(string key)
            {
                bucket.Add(key);
            }

            public void RemoveKey(string key)
            {
                if (bucket.Contains(key))
                {
                    bucket.Remove(key);
                }
            }
        }
        class DHT
        {
            public List<Node> nodes = new List<Node>();
            private HashSet<string> keys = new HashSet<string>();

            public DHT(int numNodes)
            {
                for (int i = 0; i < numNodes; i++)
                {
                    Node node = new Node();
                    node.id = i;
                    nodes.Add(node);
                }
            }

            public int GetNodeIndex(int keyHash)
            {
                keyHash = Math.Abs(keyHash);
                return keyHash % nodes.Count;
            }

            public void SetValue(string key, string value)
            {
                int keyHash = key.GetHashCode();
                int nodeIndex = GetNodeIndex(keyHash);
                Node node = nodes[nodeIndex];
                node.AddKey(key);
                keys.Add(key);
            }

            public string GetValue(string key)
            {
                int keyHash = key.GetHashCode();
                int nodeIndex = GetNodeIndex(keyHash);
                Node node = nodes[nodeIndex];
                if (node.bucket.Contains(key))
                {
                    return $"WSB -- Value: '{key}' in node NO.{node.id}.";
                }
                else
                {
                    return $"WSB -- Value:'{key}' not found";
                }
            }

            public HashSet<string> GetKeys()
            {
                return keys;
            }
        }
        static void Main(string[] args)
        {
            DHT dht = new DHT(100);

            // 用随机分配的键初始化100个节点
            for (int i = 0; i < 100; i++)
            {
                Node node = dht.nodes[i];
                for (int j = 0; j < new Random().Next(0, 10); j++)
                {
                    string key = $"wsb{i}.{j}";
                    node.AddKey(key);
                    dht.GetKeys().Add(key);
                }
            }

            // 生成200个随机字符串并在随机节点中设置它们的值
            List<Tuple<string, string>> keyValuePairs = new List<Tuple<string, string>>();
            for (int i = 0; i < 200; i++)
            {
                string key = $"wsb No.{i}";
                string value = "";
                int length = new Random().Next(5, 21);
                for (int j = 0; j < length; j++)
                {
                    char c = (char)new Random().Next(97, 123);
                    value += c;
                }
                keyValuePairs.Add(new Tuple<string, string>(key, value));
                if (i == 0)
                {
                    dht.SetValue(key, value);
                }
                else
                {
                    int nodeIndex = new Random().Next(0, 100);
                    dht.SetValue(key, value);
                    dht.nodes[nodeIndex].AddKey(key);
                }
            }

            // 选择100个随机键并检索它们的值
            HashSet<string> selectedKeys = new HashSet<string>();
            Random random = new Random();
            while (selectedKeys.Count < 100)
            {
                string key = keyValuePairs[random.Next(0, 200)].Item1;
                if (!selectedKeys.Contains(key))
                {
                    selectedKeys.Add(key);
                }
            }
            foreach (string key in selectedKeys)
            {
                Console.WriteLine(dht.GetValue(key));
            }
            Console.ReadKey();
        }
        #endregion
        

    }
    }
