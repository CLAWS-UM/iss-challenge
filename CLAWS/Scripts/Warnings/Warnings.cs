using HoloToolkit.Unity;
using UnityEngine;

public enum warningLevel
{
   Low = 0,
   Medium,
   Urgent
}


namespace Warnings
{
  public class TagalongAction : InteractableAction
  {
    private GameObject warning;

    private void Awake()
    {
      warning = Instantiate(warning);
      warning.SetActive(false);

      Billboard billboard = warning.AddComponent<Billboard>();
      warning.AddComponent<SimpleTagalong>();
    }


  }


}
