using System;
using System.Collections.Generic;

public class CircularList<T> : List<T>
{
    private bool _loop;
    private int _index;

    public bool Loop { get { return _loop; } set { _loop = value; } }
    public T Next => (_index >= Count && _loop) ? new Func<T>(() => { _index = 0; return this[_index++]; }).Invoke() : this[_index++];
    public bool HasNext => (Count == 0 || (!_loop && _index >= Count)) ? false : true;
}