using UnityEngine;

public class PlayerCombar : MonoBehaviour
{
  private LinkedList linkedList = new LinkedList();
  private ArrayQueue<Element> queue = new ArrayQueue<Element>();
  private ArrayStack<Element> stack = new ArrayStack<Element>();

  private Element[] combo = new Element[5];

  void Start()
  {
  }

  void Update()
  {
  }
}
