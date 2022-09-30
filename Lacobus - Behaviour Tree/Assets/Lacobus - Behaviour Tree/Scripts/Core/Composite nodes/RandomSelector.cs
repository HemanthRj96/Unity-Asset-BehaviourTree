using Lacobus.BehaviourTree;
using UnityEngine;


public class RandomSelector : CompositeNode
{
    int _currentIndex;

    protected override void OnStart()
    {
        _currentIndex = Random.Range(0, children.Count);
    }

    protected override void OnEnd() { }

    protected override NodeStates OnUpdate()
    {
        var child = children[_currentIndex];
        return child.UpdateNode();
    }
}
