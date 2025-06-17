using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Items
{
    /// <summary>
    /// Used to pass inventory item state to pickup item
    /// </summary>
    public class StateBundle
    {
        public StringFloatPair[] _floats = new StringFloatPair[0];
        public void PutFloat(string key, float value)
        {
            var list = _floats.ToList();
            var pairIdx = list.FindIndex(x => x.key == key);
            if(pairIdx != -1)
            {
                list[pairIdx] = new StringFloatPair(key, value);
            }
            else
            {
                list.Add(new StringFloatPair(key, value));
            }
            _floats = list.ToArray();
        }

        public float GetFloat(string key)
        {
            foreach(var f in _floats)
            {
                if(f.key == key) return f.value;
            }
            return -1f;
        }

        public struct StringFloatPair
        {
            public string key;
            public float value; 
            public StringFloatPair(string key, float value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}
