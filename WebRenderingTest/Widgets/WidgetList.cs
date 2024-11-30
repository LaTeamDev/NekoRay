using System.Collections;
using Yoga;
using Yoga.Interop;

namespace WebRenderingTest.Widgets;

public class WidgetList : ICollection {
    private Widget _widget;

    internal WidgetList(Widget widget) {
        _widget = widget;
    }

    internal Widget? Get(int index) {
        return _widget.Node.Children[index].Context as Widget;
    }

    public Widget? this[int index] => Get(index);
    
    public IEnumerator<Widget?> GetEnumerator() {
        return new WidgetListEnumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public void CopyTo(Array array, int index = 0) {
        foreach (var child in this) {
            array.SetValue(child, index++);
        }
    }

    public int Count => _widget.Node.Children.Count;
    
    public bool IsSynchronized => false;
    public object SyncRoot => this;
}

internal class WidgetListEnumerator(WidgetList widgetList) : IEnumerator<Widget?> {

    public bool MoveNext() {
        if (_cursor < _widgetList.Count)
            _cursor++;
        return _cursor != _widgetList.Count;
    }

    public void Reset() {
        _cursor = -1;
    }

    private int _cursor = -1;
    private WidgetList _widgetList = widgetList;

    public Widget? Current => _widgetList[_cursor];

    object? IEnumerator.Current => Current;

    public void Dispose() {
        Reset();
        _widgetList = null!;
    }
}