using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Events/StringEvent")]
public class StringEvent : ScriptableObject
{
    private UnityAction<string> _listener;

    public void Raise(string value) => _listener?.Invoke(value);
    public void Register(UnityAction<string> listener) => _listener += listener;
    public void Unregister(UnityAction<string> listener) => _listener -= listener;
}